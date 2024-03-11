using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogDriver : MonoBehaviour
{
    [SerializeField] Dialog dialog;

    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerNameBox;
    public SerializedDictionary<string, Transform> characterPositions; //this string should be changed for an enum that includes left, mid-left, mid, mid-right, right
    public Dictionary<string, GameObject> currDialogingCharacters = new Dictionary<string, GameObject>(); //this is suposed to store each active prefab with the name of the character it belongs to
    [HideInInspector] public string currSpeakerName;

    static readonly float timeBetweenCharacter = 0.05f;
    static readonly float timeAfterComma = 0.1f;
    static readonly float timeAfterDot = 0.2f;

    int currCharIndex = 0;
    int currLineIndex = 0;

    [HideInInspector] public bool lineFinished = false;

    TextEffectsManager textEffectsManager;

    private void Awake()
    {
        dialog.thisDialogDriver = this;
    }

    private void Start() 
    {
        dialog.Start();
        StartNewLine(dialog.dialogTable.StringTables[0][currLineIndex.ToString()].Value);
    }
    private void Update()
    {
        if (dialog != null)
            dialog.Update();
    }

    public void NextLinearLine(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if(lineFinished)
        {
            StartNewLine(dialog.dialogTable.StringTables[0][currLineIndex.ToString()].Value);
        }
        else
            SkipCurrentLine();
    }

    void StartNewLine(string newLine)
    {
        dialogText.text = "";
        currCharIndex = 0;

        dialog.CheckEvents(currLineIndex);

        lineFinished = false;
        StartCoroutine(SpeakCycle(newLine));
    }

    void SkipCurrentLine()
    {
        dialogText.text = dialog.dialogTable.StringTables[0][currLineIndex.ToString()].Value;
        LineFinished();
    }

    IEnumerator SpeakCycle(string line)
    {
        char currChar = line[currCharIndex];

        if (lineFinished)
            yield break;

        dialogText.text += currChar;

        if (currChar == ',')
            yield return new WaitForSeconds(timeAfterComma);
        if (currChar == '.' || currChar == '?' || currChar == '!')
            yield return new WaitForSeconds(timeAfterDot);
        else
            yield return new WaitForSeconds(timeBetweenCharacter);
        
        currCharIndex++;

        if (currCharIndex >= line.Length)
        {
            LineFinished();
            yield break;
        }

        StartCoroutine(SpeakCycle(line));
    }

    void LineFinished()
    {
        lineFinished = true;
        currLineIndex++;
    }
}

[System.Serializable]
public class CharacterGFX
{
    public Transform character;
    public Image characterBody;
    public Image characterFace;
}