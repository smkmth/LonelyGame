using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpEffect", menuName = "OtherData/SpEffect", order = 52)]
public class SpecialEffect : ScriptableObject
{
    [Tooltip("What type of effect is this")]
    public effectType thisEffectType;
    [Tooltip("How severe is this effect")]
    public int value;
}

