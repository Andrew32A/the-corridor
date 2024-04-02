using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundTrigger : MonoBehaviour
{
    public AudioSource ambientSoundSource;

    private void Start()
    {
        ambientSoundSource = ambientSoundSource.GetComponent<AudioSource>();

        if (ambientSoundSource == null)
        {
            Debug.LogError("No AudioSource component found on the ambientSound GameObject.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && ambientSoundSource != null)
        {
            ambientSoundSource.Pause();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && ambientSoundSource != null)
        {
            // starts playing the sound if it's not already playing, go back to refactor to play on awake settings if needed
            if (!ambientSoundSource.isPlaying)
            {
                ambientSoundSource.Play();
            }
            ambientSoundSource.UnPause();
        }
    }
}
