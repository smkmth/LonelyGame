using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{

    //tooltips to the thing below for inspector layout

    private AnimationManager animator;
    private EquipmentHolder equipmentHolder;
    private Rigidbody rb;
    private WeaponHit weapon;

    [HideInInspector]
    public Vector3 startPos;

    [Header("Health Settings")]                                         [Tooltip("Max Health - How much damage character can take")]
    public int MaxHealth;                                               [Tooltip("Current Health - DONT CHANGE THIS - Set to Max Health value on start up - in inspector for debug reasons")]
    public int Health;                                                  [Tooltip("Current Effect - DONT CHANGE THIS - Any damage effects applied to player are set here")]
    public effectType currentEffect = effectType.nothing;

                          //is the player currently attacking?


    GameObject sceneManager;


    private void Start()
    {
        sceneManager = GameObject.Find("SceneManager");
        equipmentHolder = GetComponent<EquipmentHolder>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<AnimationManager>();
        weapon = GetComponentInChildren<WeaponHit>();
        Health = MaxHealth;
        startPos = transform.position;

    }
 



    public void TakeDamage(int amount, float force)
    {

        Health -= amount;
        rb.AddForce(-transform.forward * force, ForceMode.Impulse);


        if (Health <= 0)
        {

            Destroy(gameObject);
        }




    }


   
    private void Update()
    {

       
        switch (currentEffect)
        {
            case effectType.bleeding:
                break;
            case effectType.instaKill:
                break;
            case effectType.poison:
                break;
            case effectType.slowed:
                break;
            case effectType.staggered:
  
                break;

        }
    }

}


