using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

    private Combat playerStats;
    private Stamina playerStamina;
    public GameObject playerHUD;
    public Slider healthBar;
    public Slider staminaBar;


    // Start is called before the first frame update
    void Start()
    {

        playerStats = GetComponent<Combat>();
        playerStamina = GetComponent<Stamina>();

        healthBar.maxValue = playerStats.MaxHealth;
        staminaBar.maxValue = playerStamina.MaxStamina;
        playerHUD.SetActive(true);
        
    }
    public void ToggleHUD(bool hudOn)
    {
        if (!playerHUD.activeSelf)
        {

            playerHUD.SetActive(true);
        }
        else
        {
            playerHUD.SetActive(false);

        }


    }

    // wastefull atm, find way to make this occur when it needs to 
    void Update()
    {
        healthBar.value = playerStats.Health;
        staminaBar.value = playerStamina.currentStamina;
    }
}
