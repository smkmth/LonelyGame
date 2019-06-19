using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Sound,
    Animation,
    Debug

}

public class ScriptableEvent : ScriptableObject
{
    public EventType eventType;

}
