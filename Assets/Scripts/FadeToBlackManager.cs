using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FadeToBlackManager : MonoBehaviour {

    public GameObject fadeToBlackCanvas;
    public TextMeshProUGUI fadeToBlackText;
    private Stamina stamina;
    public int timeToWakeUp;
    public Vector3 placeToWakeUp;
    public string stringToDisplay;

    public void Start()
    {
        stamina = GameObject.Find("Player").GetComponent<Stamina>();
    }
    public void Sleep(Vector3 placeToWake)
    {
        placeToWakeUp = placeToWake;
        StartCoroutine(GoToSleep(stringToDisplay));
        TimeManager.TimeManagerInstance.JumpForwardInTime(timeToWakeUp);

    }



    IEnumerator GoToSleep(string textToDisplay)
    {
        fadeToBlackCanvas.SetActive(true);
        fadeToBlackText.text = textToDisplay;
        yield return new WaitForSeconds(2);
        stamina.canLoseStamina = false;
        stamina.currentStamina = stamina.MaxStamina;
        transform.position = placeToWakeUp;
        stamina.canLoseStamina = true;
        fadeToBlackCanvas.SetActive(false);
        yield return null;
    }


}
