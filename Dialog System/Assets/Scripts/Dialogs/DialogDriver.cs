using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogDriver
{
    public DialogTreeDriver dialogTreeDriver;
    Dialog dialog;

    public Dictionary<string, GameObject> currDialogingCharacters = new Dictionary<string, GameObject>(); //this is suposed to store each active prefab with the name of the character it belongs to
    [HideInInspector] public string currSpeakerName;

    static readonly float timeBetweenCharacter = 0.05f;
    static readonly float timeAfterComma = 0.1f;
    static readonly float timeAfterDot = 0.2f;

    string currBranch = "Branch: 0";
    int currCharIndex = 0;
    int currLineIndex = 0;

    [HideInInspector] public bool lineFinished = false;

    TextMeshProUGUI dialogText;
    TextEffectsManager textEffectsManager;

    public DialogDriver(DialogTreeDriver dialogTreeDriver, Dialog dialog)
    {
        this.dialogTreeDriver = dialogTreeDriver;
        this.dialog = dialog;
        dialog.thisDialogDriver = this;

        dialogText = dialogTreeDriver.dialogText;
        textEffectsManager = dialogTreeDriver.textEffectsManager;
    }

    public void Start() 
    {
        dialog.Start();
        StartNewLine(dialog.dialogTable.StringTables[0][currLineIndex.ToString()].Value);
    }

    public void Update()
    {
        if (dialog != null)
            dialog.Update();
        if(lineFinished)
            dialogTreeDriver.textEffectsManager.Update();
    }
    #region LineManagement
    public void NextLinearLine()
    {

        if(!lineFinished)
        {
            SkipCurrentLine();
            return;
        }

        if (currLineIndex >= dialog.dialogTable.StringTables[0].Count)
        {
            EndDialog();
            return;
        }

        StartNewLine(dialog.dialogTable.StringTables[0][currLineIndex.ToString()].Value);
    }

    void StartNewLine(string newLine)
    {
        dialogText.text = "";
        currCharIndex = 0;

        textEffectsManager.SetNewLine(newLine);
        dialog.CheckTextEffects(currLineIndex);
        dialog.CheckEvents(currLineIndex);

        lineFinished = false;
        dialogTreeDriver.StartCoroutine(SpeakCycle(newLine));
    }

    void SkipCurrentLine()
    {
        dialogText.text = dialog.dialogTable.StringTables[0][currLineIndex.ToString()].Value;
        LineFinished();
    }

    void LineFinished()
    {
        dialog.CheckChoices(currBranch + ": " + currLineIndex);//the format is "Branch: (branchIndexes separated by "-"): currLineIndex
        currLineIndex++;
        lineFinished = true;
    }

    public void OnBranchChanged(int newBranchIndex)
    {
        currBranch += "-" + newBranchIndex;
        currLineIndex = 0;
    }

    #endregion

    void EndDialog()
    {

    }

    #region SpeakCycle
    IEnumerator SpeakCycle(string line)
    {
        if (lineFinished)
            yield break;

        char currChar = line[currCharIndex];

        dialogText.text += currChar;
        textEffectsManager.Update();

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

        dialogTreeDriver.StartCoroutine(SpeakCycle(line));
    }
    #endregion
    
}

[System.Serializable]
public class CharacterGFX
{
    public Transform character;
    public Image characterBody;
    public Image characterFace;
}