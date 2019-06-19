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

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        ghostAudio = GetComponent<AudioSource>();
    }
    void Update()
    {
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

                ghostAudio.PlayOneShot(scare, 1.0f);
            }
        }
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
