using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueContainer : MonoBehaviour
{
    public List<DialogueLine> dialogue;
    public VillageManager villageManager;
    [SerializeField]
    public TextAsset story;
    public CharacterInfo characterInfo;
    private InkDisplayer dialogueDisplayer;

    public void Start()
    {
        dialogueDisplayer = GameObject.Find("SceneManager").GetComponent<InkDisplayer>();
        villageManager = GameObject.Find("SceneManager").GetComponent<VillageManager>();

    }

    public void Talk()
    {
        dialogueDisplayer.StartStory(story, villageManager.RequestVillageInfo(), characterInfo, this);
    }
}

[System.Serializable]
public class CharacterInfo
{
    public string Name;
    public int Disposition;
    public List<Sprite> faces;
    public List<string> faceIndexes;

}

