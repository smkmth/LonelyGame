using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum buildingState
{
    unbuilt,
    active,
    destroyed 
}
[System.Serializable]
public class AttackPoint
{
    public bool isOccupied;
    public Transform location;
}

public class Building : MonoBehaviour {

    public GameObject unbuiltBuilding;
    public GameObject normalBuilding;
    public GameObject destroyedBuilding;
    public AttackPoint[] attackPoints;
    public buildingState currentBuildingState;

    public float riskFactor;

    private void Start()
    {
        UpdateBuilding(buildingState.active);

    }

    public void UpdateBuilding(buildingState newBuildingState)
    {
        currentBuildingState = newBuildingState;
        switch (currentBuildingState)
        {
            case buildingState.active:
                unbuiltBuilding.SetActive(false);
                normalBuilding.SetActive(true);
                destroyedBuilding.SetActive(false);

                break;
            case buildingState.destroyed:
                unbuiltBuilding.SetActive(false);
                normalBuilding.SetActive(false);
                destroyedBuilding.SetActive(true);
                break;
            case buildingState.unbuilt:
                unbuiltBuilding.SetActive(true);
                normalBuilding.SetActive(false);
                destroyedBuilding.SetActive(false);
                break;

        }
    }






}
