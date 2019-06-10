using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable {



    public override void UseInteractable(GameObject user)
    {
        Vector3 pos = user.transform.position;
        user.GetComponent<Stamina>().Sleep();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
