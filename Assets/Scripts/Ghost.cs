using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{


    public GameObject player;
    public AudioSource ghostAudio;
    public AudioClip ambiant;
    public AudioClip scare;

    public float speed;
    public float stopDistance = 1.0f;

    public bool ghostActive = false;

    public GameObject gameoverCanvas;
    public MeshRenderer meshRenderer;
    // Update is called once per frame
    private float ghostIdleTimer;
    public float timeBetweenNoises;
    public AudioClip[] idleNoises;
    public AudioClip[] breathing;
    public float chanceOfIdleNoise;
    public float breathingVol;
    public float timeBetweenBreath;
    private float breathTimer;

    public AudioClip[] spottedNoises;
    public float spottedVol;

    public AudioClip firstSpottedNoise;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        ghostAudio = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (breathTimer <= 0)
        {
            breathTimer = timeBetweenBreath + Random.Range(0f,1f);
            int breathindex = Random.Range(0, breathing.Length);
            AudioClip breathingNoise = breathing[breathindex];
            ghostAudio.PlayOneShot(breathingNoise, breathingVol);

        }
        else
        {
            breathTimer -= Time.deltaTime;
        }

        if (ghostIdleTimer <= 0)
        {
            ghostIdleTimer = timeBetweenNoises;

            if (Random.Range(0, 100) < chanceOfIdleNoise)
            {
                int index = Random.Range(0, idleNoises.Length);
                AudioClip ghostnoise = idleNoises[index];
                ghostAudio.PlayOneShot(ghostnoise, Random.Range(0.5f, 1f));
                Debug.Log("played " + ghostAudio.gameObject.name);
            }
        }
        else
        {
            ghostIdleTimer -= Time.deltaTime;
        }

        if (ghostActive)
        {



            Vector3 pos = player.transform.position;
            pos.y = 0f;
            transform.LookAt(pos);
            if (Vector3.Distance(transform.position, player.transform.position) > stopDistance)
            {
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, pos, step);
            }
            else
            {
                gameoverCanvas.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (ghostActive)
        {


            if (other.gameObject.tag == "Player")
            {
                 meshRenderer.enabled = true;
                int spottedindex = Random.Range(0, spottedNoises.Length);
                AudioClip breathingNoise = spottedNoises[spottedindex];
                ghostAudio.PlayOneShot(breathingNoise, spottedVol);

                //ghostAudio.PlayOneShot(scare, 1.0f);
            }
        }
    }

    public void OnActivateGhost()
    {
        ghostActive = true;
        ghostAudio.PlayOneShot(firstSpottedNoise, 1.0f);

    }

    private void OnTriggerExit(Collider other)
    {
        if (ghostActive)
        {

            if (other.gameObject.tag == "Player")
            {
                meshRenderer.enabled = false;
        
            }
        }
    }
}
