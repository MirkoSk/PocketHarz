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

    Vector3 positionLastFrame;



    private void FixedUpdate()
    {
        positionLastFrame = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ploppable")
        {
            Vector3 punchDirection = (transform.position - positionLastFrame);
            float punchIntensity = punchDirection.magnitude;
            punchDirection = punchDirection.normalized;
            punchIntensity = punchIntensity.Remap(0f, maxMoveSpeed, 1f, maxPunchMultiplier);
            other.transform.DOBlendablePunchRotation(punchDirection * punchIntensity * shakeIntensity, shakeDuration, shakeVibrato, shakeElasticity);
        }
    }
}
