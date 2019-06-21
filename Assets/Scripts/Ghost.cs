using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GhostState
{
    Patrolling,
    Chasing
}
public class Ghost : MonoBehaviour
{


    private AudioSource ghostAudio;
    public AudioClip ambiant;
    public AudioClip scare;
    public GameObject ghostMesh;
    public GameObject gameoverCanvas;

    [Header("Ghost Chase AI")]

    public Transform target;
    public bool ghostActive = false;
    public float currentMoveSpeed;
    public float minMoveSpeed;
    public float maxMoveSpeed;
    public float speedDropMod;
    public float stopDistance = 1.0f;
    public float chaseDistance = 10.0f;
    public float slowdownDistance = 5.0f;
    public GhostState currentGhostState;

    [Header("Ghost Patrol AI")]
    public Transform[] points;
    private int destPoint;

    [Header("Ghost Audio")]
    public float timeBetweeIdleNoises;
    private float ghostIdleTimer;
    public float chanceOfIdleNoise;
    public AudioClip[] idleNoises;
    public AudioClip[] breathingNoises;
    public float breathingVol;
    public float timeBetweenBreath;
    private float breathTimer;

    public AudioClip[] spottedNoises;
    public float spottedVol;

    public AudioClip firstSpottedNoise;

    public GameObject[] targets;
    int targetIndex =1;

    [Header("Ghost Light")]
    public Light ghostLight;
    public float ghostLightInitIntensity;
    public float ghostLightDuration;

    [Header("Ghost Alpha")]
    [Range(0,1)]
    public float ghostInitAlpha;

    public float ghostFadeDuration;
    public float distanceToTarget;
    Renderer ghostRenderer;

    Coroutine fadeOut;
    Coroutine lightFade;

    private void Start()
    {
        ghostAudio = GetComponent<AudioSource>();
        
    }
    public void OnFirstActivateGhost()
    {
        ghostActive = true;
        ghostAudio.PlayOneShot(firstSpottedNoise, 1.0f);

    }

    public void ToggleGhost(bool ghostOn)
    {
        if (ghostOn)
        {
            if (fadeOut != null)
            {
                StopCoroutine(fadeOut);
            }
            if (lightFade != null)
            {
                StopCoroutine(lightFade);

            }
            //turn ghost on and move to correct position and rotation
            ghostMesh.SetActive(true);
            ghostMesh.transform.position = transform.position;
            ghostMesh.transform.rotation = transform.rotation;

            //set up light itencity and alpha
            ghostLight.intensity = ghostLightInitIntensity;
            Color col = ghostMesh.GetComponent<Renderer>().material.color;
            col.a = ghostInitAlpha;
            if (ghostRenderer == null)
            {
                ghostRenderer = ghostMesh.GetComponent<Renderer>();

            }
            ghostRenderer.material.color = col;
            fadeOut = StartCoroutine(FadeTo(ghostRenderer.material, 0f, ghostFadeDuration));
            lightFade = StartCoroutine(LightFade(0f, ghostLightDuration));


        }
        else
        {

            ghostMesh.SetActive(false);

        }
    }

    public void ChangeGhostState(GhostState newGhostState)
    {
        currentMoveSpeed = maxMoveSpeed;

        switch (newGhostState)
        {
            case GhostState.Chasing:
                break;
            case GhostState.Patrolling:
                break;
        }
        currentGhostState = newGhostState;

    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        target = points[destPoint].transform;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }
    void Update()
    {

        GhostAudio();

     

        if (ghostActive)
        {
            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            Vector3 pos = target.position;
            pos.y = 0f;
            transform.LookAt(pos);

            switch (currentGhostState)
            {
                case GhostState.Chasing:
                    if (distanceToTarget > stopDistance)
                    {
                        float step = currentMoveSpeed * Time.deltaTime; // calculate distance to move
                        transform.position = Vector3.MoveTowards(transform.position, pos, step);
                    }
                    if (distanceToTarget > slowdownDistance)
                    {
                        if (currentMoveSpeed <= maxMoveSpeed)
                        {
                            currentMoveSpeed += Time.deltaTime * speedDropMod;
                        }
                    }
                    else
                    {
                        if (currentMoveSpeed >= minMoveSpeed)
                        {
                            currentMoveSpeed -= Time.deltaTime * speedDropMod;
                        }

                    }
                    if (distanceToTarget > chaseDistance)
                    {
                        ChangeGhostState(GhostState.Patrolling);
                    }
                    break;
                case GhostState.Patrolling:
 
                    if (distanceToTarget > stopDistance)
                    {
                        float step = currentMoveSpeed * Time.deltaTime; // calculate distance to move
                        transform.position = Vector3.MoveTowards(transform.position, pos, step);
                    }
                    else
                    {
                        GotoNextPoint();

                    }
                    break;
            }
        
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ghostActive)
        {
       
            if (other.gameObject.tag == "Player")
            {
                HelperFunctions.PlayRandomNoiseInArray(spottedNoises, ghostAudio, spottedVol);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ghostActive)
        {

            if (other.gameObject.tag == "Player")
            {

            }
        }
    }



    IEnumerator LightFade( float targetOpacity, float duration)
    {

        float startBrightness = ghostLight.intensity; 
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float blend = Mathf.Clamp(time, 0,  duration);
            ghostLight.intensity = Mathf.Lerp(startBrightness, targetOpacity, blend);
            yield return null;
        }
    }

    IEnumerator FadeTo(Material material, float targetOpacity, float duration)
    {

        Color color = material.color;
        float startOpacity = color.a;

        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float blend = Mathf.Clamp01(time / duration);
            color.a = Mathf.Lerp(startOpacity, targetOpacity, blend);
            material.color = color;
            yield return null;
        }
    }

    public void GhostAudio()
    {
        ghostIdleTimer -= Time.deltaTime;
        breathTimer -= Time.deltaTime;

        if (breathTimer <= 0)
        {
            breathTimer = timeBetweenBreath;
            HelperFunctions.PlayRandomNoiseInArray(breathingNoises, ghostAudio, breathingVol);
        }
        else
        { 

            if (ghostIdleTimer <= 0)
            {
                ghostIdleTimer = timeBetweeIdleNoises;

                if (Random.Range(0, 100) < chanceOfIdleNoise)
                {
                    HelperFunctions.PlayRandomNoiseInArray(idleNoises, ghostAudio, Random.Range(0.5f, 1f));
                }
            }
        
        }

    }
}
