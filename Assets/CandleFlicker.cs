using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleFlicker : MonoBehaviour {

    Light candleLight;
    float currentTime;
    public float timeToMove;
    public float randomLow;
    public float randomHigh;
    public bool initial =true;

    // Use this for initialization
    void Start () {

        candleLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {

        if (currentTime <= timeToMove)
        {
            
            currentTime += Time.deltaTime * 10.0f;
            if (initial)
            {
                candleLight.intensity = Mathf.Lerp(randomLow, randomHigh, currentTime / timeToMove);
                initial = false;

            }
            else
            {
                candleLight.intensity = Mathf.Lerp(randomHigh, randomLow, currentTime / timeToMove);
            }

        }
        else
        {
            if (!initial)
            {
                initial = true;
                timeToMove = Random.Range(0.3f, 5.0f);
                randomLow = Random.Range(1.5f, 1.7f);
                randomHigh = Random.Range(1.7f, 1.85f);

            }
            candleLight.intensity = 1.7f;
            currentTime = 0f;
        }


    }

}
