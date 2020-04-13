using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        PlopUpController.plopInCompleted += Play;
    }

    private void OnDisable()
    {
        PlopUpController.plopInCompleted -= Play;
    }

    // Update is called once per frame
    void Play()
    {
        audioSource.Play(3f);
    }
}
