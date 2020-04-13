using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParticleInteractionController : MonoBehaviour
{
    [SerializeField] float shakeIntensity = 10f;
    [SerializeField] float shakeDuration = 1f;
    [SerializeField] int shakeVibrato = 10;
    [SerializeField] float shakeElasticity = 1f;
    [SerializeField] float maxMoveSpeed = 10f;
    [Range(1f, 10f)]
    [SerializeField] float maxPunchMultiplier = 3f;

    [SerializeField] List<AudioClip> leaveSounds = new List<AudioClip>();
    [SerializeField] List<AudioClip> stoneSounds = new List<AudioClip>();
    [Range(0f, 1f)]
    [SerializeField] float minPitchRange = 0.5f;
    [Range(1f, 3f)]
    [SerializeField] float maxPitchRange = 1.5f;

    Vector3 positionLastFrame;
    AudioSource audioSource;



    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        positionLastFrame = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ploppable")
        {
            Vector3 punchDirection = (transform.position - positionLastFrame);
            punchDirection = Vector3.Cross(punchDirection, Vector3.down);
            float punchIntensity = punchDirection.magnitude;
            punchDirection = punchDirection.normalized;
            punchIntensity = punchIntensity.Remap(0f, maxMoveSpeed, 1f, maxPunchMultiplier);

            Debug.DrawLine(other.transform.position, (other.transform.position + punchDirection) * punchIntensity * shakeIntensity, Color.green, 5f);
            other.transform.DOBlendablePunchRotation(punchDirection * punchIntensity * shakeIntensity, shakeDuration, shakeVibrato, shakeElasticity);

            AudioClip sound = null;
            if (other.name.Contains("tree") || other.name.Contains("dead")) sound = leaveSounds[Random.Range(0, leaveSounds.Count)];
            else if (other.name.Contains("stone")) sound = stoneSounds[Random.Range(0, stoneSounds.Count)];

            audioSource.pitch = Random.Range(minPitchRange, maxPitchRange);
            if (sound != null) audioSource.PlayOneShot(sound);
        }
    }
}
