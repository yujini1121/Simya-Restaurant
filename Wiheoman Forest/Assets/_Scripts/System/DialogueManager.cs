using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public int id;
        public string characterNameL;
        public string characterNameR;
        public int speaker;
        public string speakerName;
        public string text;
    }

    [System.Serializable]
    public class DialogueList
    {
        public List<Data> dialogue;
    }

    [Header("Dialogue")]
    [SerializeField] private DialogueList script;
    [SerializeField] private int BeginLine;
    [SerializeField] private int EndLine;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private Button skipButton;

    private bool isDialogueActive = false;
    private int currentLine = 0;

    void Start()
    {
        script = JsonUtility.FromJson<DialogueList>(Resources.Load<TextAsset>("Json Files/Dialogue").text);

        currentLine = BeginLine;

        characterName.text = script.dialogue[currentLine].speakerName;
        dialogueText.text = script.dialogue[currentLine].text;

        skipButton.onClick.AddListener(SkipDialouge);
    }

    void Update()
    {
        if(isDialogueActive)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!isDialogueActive)
            {
                StartDialogue();
                SpriteAction();
            }
            else
            {
                EndDialogue();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            if(currentLine > EndLine)
            {
                EndDialogue();
                return;
            }

            SpriteAction();
            skippingLine();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dialogueUI.SetActive(true);               
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialogueUI.SetActive(false);
    }

    private void SkipDialouge()
    {
        EndDialogue();
    }

    private void SpriteAction()
    {
        if(currentLine < script.dialogue.Count && isDialogueActive)
        {
            GameObject speaker = dialogueUI.transform.GetChild(0).gameObject;
            if (script.dialogue[currentLine].characterNameL != "")
            {
                speaker.SetActive(true);
                speaker.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + script.dialogue[currentLine].characterNameL);
            }
            else
            {
                speaker.SetActive(false);
            }

            speaker = dialogueUI.transform.GetChild(1).gameObject;
            if (script.dialogue[currentLine].characterNameR != "")
            {
                speaker.SetActive(true);
                speaker.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + script.dialogue[currentLine].characterNameR);
            }
            else
            {
                speaker.SetActive(false);
            }

            if (script.dialogue[currentLine].speaker == 0 || script.dialogue[currentLine].speaker == 1)
            {
                dialogueUI.transform.GetChild(script.dialogue[currentLine].speaker).GetComponent<Image>().color = Color.white;
                dialogueUI.transform.GetChild(1 - script.dialogue[currentLine].speaker).GetComponent<Image>().color = Color.gray;                
            }
        }              
    }

    private void skippingLine()
    {
        if(currentLine < script.dialogue.Count && isDialogueActive)
        {
            characterName.text = script.dialogue[currentLine].speakerName;
            dialogueText.text = script.dialogue[currentLine].text;

            currentLine++;
        }       
    }
} 
