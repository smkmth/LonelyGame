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
                int index = Random.Range(0, noises.Length);
                AudioClip housenoise = noises[index];
                int sourceindex = Random.Range(0, sources.Length);
                sources[sourceindex].PlayOneShot(housenoise, Random.Range(0.5f,1f));
                Debug.Log("played " + sources[sourceindex].gameObject.name);
            }
        }
        else
        {
            houseTimer -= Time.deltaTime;
        }
    }
}
