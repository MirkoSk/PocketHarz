using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class PlopUpController : MonoBehaviour
{
    [System.Serializable]
    public class Ploppable
    {
        public Transform transform;
        public Vector3 originalScale;

        public Ploppable(Transform transform, Vector3 originalScale)
        {
            this.transform = transform;
            this.originalScale = originalScale;
        }
    }

    public delegate void plopInCompletedHandler();
    public static event plopInCompletedHandler plopInCompleted;

    [Header("Timing")]
    [SerializeField] float plopDuration = 0.1f;
    [SerializeField] AnimationCurve plopAnimation = new AnimationCurve();
    [SerializeField] float initialDelay = 2f;
    [SerializeField] float maxTimeBetweenPlops = 1f;
    [SerializeField] AnimationCurve timeBetweenPlops = new AnimationCurve();
    [Tooltip("Offset for the plop delays. Given as percentage from the calculated offset from the AnimationCurve.")]
    [Range(0f, 0.8f)]
    [SerializeField] float timeBetweenPlopsOffset = 0f;

    [Header("Sounds")]
    [SerializeField] AudioClip plopSound = null;
    [Tooltip("Delay of the plop sound as percentage of the plopDuration.")]
    [Range(0f,1f)]
    [SerializeField] float plopSoundDelay = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] float minPitchRange = 0.5f;
    [Range(1f, 3f)]
    [SerializeField] float maxPitchRange = 1.5f;

    [Space]
    [SerializeField] List<Ploppable> ploppables = new List<Ploppable>();

    AudioSource audioSource;



    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Setup Ploppables list
        GameObject[] ploppableGOs = GameObject.FindGameObjectsWithTag("Ploppable");
        foreach (GameObject ploppableGO in ploppableGOs)
        {
            ploppables.Add(new Ploppable(ploppableGO.transform, ploppableGO.transform.localScale));
            ploppableGO.transform.localScale = Vector3.zero;
        }
    }

    void Start()
    {
        StartCoroutine(PlopInScene());
    }


    IEnumerator PlopInScene()
    {
        yield return new WaitForSeconds(initialDelay);

        for (int i = 0; i < ploppables.Count; i++)
        {
            ploppables[i].transform.DOScale(ploppables[i].originalScale, plopDuration).SetEase(plopAnimation);

            audioSource.pitch = Random.Range(minPitchRange, maxPitchRange);
            yield return new WaitForSeconds((plopDuration * plopSoundDelay) / audioSource.pitch);

            audioSource.PlayOneShot(plopSound);

            float waitTime = maxTimeBetweenPlops * timeBetweenPlops.Evaluate(((float) i / (float) ploppables.Count));
            float offset = 1 - Random.Range(-timeBetweenPlopsOffset, timeBetweenPlopsOffset);
            waitTime *= offset;
            yield return new WaitForSeconds(waitTime - (plopDuration * plopSoundDelay));
        }

        plopInCompleted?.Invoke();
    }
}
