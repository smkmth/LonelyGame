using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSoundGen : MonoBehaviour {


    public AudioClip[] noises;
    public AudioSource[] sources;
    public float chanceOfNoise;
    public float noisesPerSecond;
    private float houseTimer;
  
	
	// Update is called once per frame
	void Update () {

        if (houseTimer <= 0)
        {
            houseTimer = noisesPerSecond;
            if (Random.Range(0,100) < chanceOfNoise)
            {
                int sourceindex = Random.Range(0, sources.Length);
                HelperFunctions.PlayRandomNoiseInArray(noises, sources[sourceindex], Random.Range(0.5f, 1f));
            }
        }
        else
        {
            houseTimer -= Time.deltaTime;
        }
    }
}
