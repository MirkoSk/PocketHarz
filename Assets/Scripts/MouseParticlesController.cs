using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseParticlesController : MonoBehaviour
{

    [SerializeField] float spawnDelay = 1f;
    [SerializeField] new ParticleSystem particleSystem = null;
    [SerializeField] float flightHeight = 1f;
    [SerializeField] LayerMask rayLayerMask = new LayerMask();

    RaycastHit hit;
    bool active;
    AudioSource audioSource;


    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PlopUpController.plopInCompleted += Activate;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PlopUpController.plopInCompleted -= Activate;
    }

    // Start is called before the first frame update
    private void OnDrawGizmos()
    {
        if (!active) return;

        Gizmos.DrawWireSphere(hit.point, 0.05f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!active) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 30.0f, rayLayerMask))
        {
            if (!particleSystem.isPlaying) particleSystem.Play();

            particleSystem.transform.position = hit.point + Vector3.up * flightHeight;
        }
        else if (particleSystem.isPlaying)
        {
            particleSystem.Stop();
        }
    }



    void Activate()
    {
        StartCoroutine(Wait(spawnDelay, () =>
        {
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        particleSystem.Play();
        audioSource?.Play();
        StartCoroutine(Wait(2f, () => { active = true; }));
        }));
    }

    IEnumerator Wait(float delay, System.Action onComplete)
    {
        yield return new WaitForSeconds(delay);

        onComplete.Invoke();
    }
}
