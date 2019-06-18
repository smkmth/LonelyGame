using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public Vector3 originalLocation;
    public Quaternion originalRotation;
    public string itemname;


	// Use this for initialization
	void Start () {
        originalLocation = transform.position;
        originalRotation = transform.rotation;

    }
	
	
}
