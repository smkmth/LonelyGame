using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour {

    public Weapon weaponData;
    public string target;
    public bool doDamage;
    public Combat thisCombat;


    public void Start()
    {
        thisCombat = GetComponentInParent<Combat>();

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.gameObject.tag == target)
        {
            if (doDamage)
            {
                Combat combat = collision.transform.gameObject.GetComponent<Combat>();
                combat.TakeDamage(weaponData.attackDamage, 3.0f );
            }
        }
    }
}
