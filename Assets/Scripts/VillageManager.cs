using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VillageManager : MonoBehaviour
{

    public TimeManager time;
    public List<string> CurrentCharacters;
    public List<Building> currentBuildings;
    public FadeToBlackManager fadeToBlack;
    public UnityEvent OnEnemyAttack;
    public int invasionDay;

 
    public VillageInfo RequestVillageInfo()
    {
        VillageInfo villageInfo = new VillageInfo(TimeManager.TimeManagerInstance.currentHour, TimeManager.TimeManagerInstance.currentDay, CurrentCharacters);

        return villageInfo;
    }

    public void AttackVillage()
    {
        foreach (Building building in currentBuildings)
        {
            if (building.riskFactor < 10.0f)
            {
                building.UpdateBuilding(buildingState.destroyed);

            }
        }
    }

    public void GoToSleep(Vector3 currentPos, bool passedOut)
    {
        if (passedOut)
        {
            fadeToBlack.stringToDisplay = "You passed out.";
        }
        else
        {
            fadeToBlack.stringToDisplay = "You slept soundly.";

        }
        if (TimeManager.TimeManagerInstance.currentDay == invasionDay)
        {
            AttackVillage();
            fadeToBlack.stringToDisplay += "The village was attacked.";
            fadeToBlack.Sleep(currentPos);
            OnEnemyAttack.Invoke();
        }
        else
        {
            fadeToBlack.stringToDisplay += "A peacefull night";
            fadeToBlack.Sleep(currentPos);
        }

    }
}

public class VillageInfo
{
    public int CurrentHour;
    public int CurrentDay;
    public List<string> Characters;

    public VillageInfo(int currentHour, int currentDay, List<string> characters)
    {
        CurrentHour = currentHour;
        CurrentDay = currentDay;
        Characters = characters;
    }
}
