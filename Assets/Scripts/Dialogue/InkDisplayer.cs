using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using TMPro;

public class InkDisplayer : MonoBehaviour {

    [SerializeField]
    private TextAsset inkJSONAsset;
    private Story story;

    public PlayerInteraction player;

    [SerializeField]
    private Image canvas;

    [SerializeField]
    private GameObject dialogueWindow;

    [SerializeField]
    private Image characterPortrait;

    // UI Prefabs
    //private Text textPrefab;

    [SerializeField]
    private TextMeshProUGUI textPrefab;
    [SerializeField]
    private Button buttonPrefab;

    public List<string> calledTags;
    public List<Button> activeButtons;

    private DialogueContainer currentCharacter;

    public void Start()
    {

        // Remove the default message
        RemoveChildren();
        dialogueWindow.SetActive(false);

    }

 
    public void StartStory(TextAsset asset, VillageInfo villageInfo, CharacterInfo characterInfo, DialogueContainer character)
    {
        Debug.Log("start dialogue");
        dialogueWindow.SetActive(true);
        inkJSONAsset = asset;
        story = new Story(inkJSONAsset.text);


        //how to start a story form a specific knot
       // story.ChoosePathString(startPoint);

        //set up the variables in the story file
        story.variablesState["timeOfDay"] = villageInfo.CurrentHour;
        story.variablesState["currentDay"] = villageInfo.CurrentDay;
        story.variablesState["characterName"] = characterInfo.Name;
        story.variablesState["_characterDisposition"] = characterInfo.Disposition;

        //point the ink displayer to the character object so it can be updated from story
        currentCharacter = character;
        RefreshView();


    }

    void UpdateVariables()
    {
        int characterDisposition = (int)story.variablesState["_characterDisposition"];
        currentCharacter.characterInfo.Disposition = characterDisposition;

    }


    void EndStory()
    {
        UpdateVariables();
        currentCharacter = null;
        RemoveChildren();
        player.SetDialogueMode();
        dialogueWindow.SetActive(false);

    }


    // This is the main function called every time the story changes. It does a few things:
    // Destroys all the old content and choices.
    // Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
    void RefreshView()
    {
        // Remove all the UI on screen
        RemoveChildren();
        // Read all the content until we can't continue any more
        while (story.canContinue)
        {
      
            // Continue gets the next line of the story
            calledTags = story.currentTags;
            string text = story.Continue();
            // This removes any white space from the text.
            text = text.Trim();
            // Display the text on screen!
            CreateContentView(text);

        }

        // Display all the choices, if there are any!
        if (story.currentChoices.Count > 0)
        {
            foreach (Button button in activeButtons)
            {
                Destroy(button);
            }
            activeButtons.Clear();
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];
                Button button = CreateChoiceView(choice.text.Trim());
                activeButtons.Add(button);

                // Tell the button what to do when we press it
                button.onClick.AddListener(delegate {
                    OnClickChoiceButton(choice);
                });
            }
            dialogueWindow.GetComponent<ButtonHighlighter>().ActivateButtons(activeButtons[0].gameObject);


        }
        // If we've read all the content and there's no choices, the story is finished!
        else
        {
            Button choice = CreateChoiceView("End...");
            choice.onClick.AddListener(delegate {
                EndStory();
            });
            dialogueWindow.GetComponent<ButtonHighlighter>().ActivateButtons(choice.gameObject);

        }
  

    }

    // Creates a button showing the choice text
    void CreateContentView(string text)
    {
        TextMeshProUGUI storyText = Instantiate(textPrefab) as TextMeshProUGUI;
        storyText.text = text;
        storyText.transform.SetParent(canvas.transform, false);
    }
    // Creates a button showing the choice text
    Button CreateChoiceView(string text)
    {

        // Creates the button from a prefab
        Button choice = Instantiate(buttonPrefab) as Button;
        choice.transform.SetParent(canvas.transform, false);


        // Gets the text from the button prefab
        TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
        choiceText.text = text;

        // Make the button expand to fit the text
        HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.childForceExpandHeight = false;

        return choice;
    }

    // Destroys all the children of this gameobject (all the UI)
    void RemoveChildren()
    {
        int childCount = canvas.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            GameObject.Destroy(canvas.transform.GetChild(i).gameObject);
        }
    }
    // When we click the choice button, tell the story to choose that choice!
    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        RefreshView();
        Debug.Log("this");
    }



}
