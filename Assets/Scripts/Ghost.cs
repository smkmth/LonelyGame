using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {


    public GameObject player;
    public AudioSource ghostAudio;
    public AudioClip ambiant;
    public AudioClip scare;

    public float speed;
    public float stopDistance =3.0f;
    // Update is called once per frame

    private void Start()
    {
        ghostAudio = GetComponent<AudioSource>();
    }
    void Update () {
        //transform.LookAt(player.transform);
        Quaternion rot = Quaternion.LookRotation(transform.position - player.transform.position, Vector3.up);
        transform.rotation = rot;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        
        if (Vector3.Distance(transform.position, player.transform.position) > stopDistance)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ghostAudio.PlayOneShot(scare, 1.0f);
        }
    }
}
