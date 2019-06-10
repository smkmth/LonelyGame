using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/DialogueLine", order = 52)]
public class DialogueLine : ScriptableObject
{
    public string speakerName;
    public string lineToSay;
    public Sprite speakerImage;

}

