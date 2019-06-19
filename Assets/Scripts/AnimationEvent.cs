using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AnimationEvent")]
public class AnimationEvent : ScriptableEvent
{
    
    public AnimationClip animator;
    public string parameterToTrigger;


}
