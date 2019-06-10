using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stamina : MonoBehaviour
{

    public float currentStamina;
    public float MaxStamina;

    public float StaminaDecayRate;
    public bool canLoseStamina = false;
    public int timeToWakeUp;
    public GameObject fadeToBlackCanvas;
    public TextMeshProUGUI fadeToBlackText;
    public string sleepText;
    public string passOutText;
    private string currentText;

    public Vector3 placeToWakeUp;

    private Combat stats;
    private VillageManager villageManager;
    public FadeToBlackManager fadeToBlack;


    // Start is called before the first frame update
    void Start()
    {
        villageManager = GameObject.Find("SceneManager").GetComponent<VillageManager>();
        
        stats = GetComponent<Combat>();
        currentStamina = MaxStamina;
        currentText = passOutText;

    }

    // Update is called once per frame
    void Update()
    {
        if (canLoseStamina)
        {
            if (currentStamina > 0)
            {
                currentStamina -= StaminaDecayRate * Time.deltaTime;

            }
            else
            {

            }

        }
    }

    public void Sleep()
    {
        villageManager.GoToSleep(transform.position, false);

    }

    public void RestoreStamina(float amount)
    {
        currentStamina += amount;
        if (currentStamina > 100)
        {
            currentStamina = 100;
        }

    }
}
