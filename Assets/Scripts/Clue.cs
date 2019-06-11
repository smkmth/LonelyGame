using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ClueType
{
    Document,
    WorldObject,
    PhotoTarget

}
public class Clue : MonoBehaviour {

    public ClueType clueType;
    public string clueText;
    public bool foundClue;
    public float energyValue;

}
