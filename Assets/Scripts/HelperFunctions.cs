using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions  {



    public static void PlayRandomNoiseInArray(AudioClip[] array, AudioSource audioSource, float vol)
    {
        if (array.Length > 0)
        {

            if (!audioSource.isPlaying)
            {
                int noiseIndex = Random.Range(0, array.Length);
                AudioClip selectedNoise = array[noiseIndex];
                audioSource.PlayOneShot(selectedNoise, vol);
            }

        }
        else
        {
            Debug.Log("No noises set for " + audioSource.gameObject.name);
        }

    }
}
