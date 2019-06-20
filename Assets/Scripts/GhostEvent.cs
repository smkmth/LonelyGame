using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GhostEventType
{
    ActivateGhost
}
[CreateAssetMenu(menuName = "GhostEvent")]
public class GhostEvent : ScriptableEvent
{
    public GhostEventType thisGhostEvent;
   

}
