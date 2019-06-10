using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum controllerPluggedIn
{
    noController,
    PS4Controller,
    XBoxController


}

/// <summary>
/// detects what contoller is currently plugged in
/// </summary>
public static class ControllerDetector  {

    public static controllerPluggedIn CheckController()
    {
        controllerPluggedIn currentController = controllerPluggedIn.noController;
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
           
            if (names[x].Length == 19)
            {
                currentController = controllerPluggedIn.PS4Controller;
            }
            if (names[x].Length == 33)
            {
                currentController = controllerPluggedIn.XBoxController;

            }
        }

        return currentController;


       
    }
}
