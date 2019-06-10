using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {


    Rigidbody rb;
    Rigidbody childRb;
    public bool detectingCollision = true;
    public int damage;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        childRb = GetComponentInChildren<Rigidbody>();
	}
  



    private void OnTriggerEnter(Collider collision)
    {
        if (detectingCollision)
        {
            rb.velocity = Vector3.zero;
            childRb.velocity = Vector3.zero;

            rb.constraints = RigidbodyConstraints.FreezeAll;
            childRb.constraints = RigidbodyConstraints.FreezeAll;
        
            rb.isKinematic = true;
            childRb.isKinematic = true;

            transform.parent = collision.transform;
            detectingCollision = false;
            // move the arrow deep inside the enemy or whatever it sticks to
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<Combat>().TakeDamage(damage, 1.0f);

            }

            
        }

        
    }
}
