using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;
using UnityEditor.Localization;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "NewDialog", menuName = "ScriptableObjects/Dialogs/Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    public StringTableCollection dialogTable;

    [Header("Pre-Line events in Dialog")]
    [SerializedDictionary("Line", "Events")]
    [SerializeField] SerializedDictionary<int, UnityEvent> dialogEvents;
    [Header("TextEffects in Dialog")]
    [SerializeField] TextEffectApplier[] textEffectAppliers;
    [Header("All choices in dialog")]
    [SerializedDictionary("Line", "Choice")]
    [SerializeField] SerializedDictionary<string, ChoiceOption[]> dialogChoices;

    [HideInInspector] public DialogDriver thisDialogDriver;
    
    Enlarger enlargingCharacter = null;
    float speakerEnlargeTime = .2f;
    float speakerObjectiveEnlargement = 1.15f;

    Shrinker shrinkingCharacter = null;
    float speakerShrinkTime = .25f;


    //these will be affected when editing the tree and used to change between dialogs during runtime
    [HideInInspector] public Dictionary<Dialog, List<DialogChangeCondition>> possibleNextDialogs = new Dictionary<Dialog, List<DialogChangeCondition>>();

    public void Start() => movementHandler = new CharacterMovementHandler(thisDialogDriver);

    public void Update()
    {
        if(movementHandler != null)
            movementHandler.Update();

        if (enlargingCharacter != null)
        {
            if (!thisDialogDriver.lineFinished)
            {
                if (enlargingCharacter.Enlarge())
                    enlargingCharacter = null;

            }
            else
            {
                enlargingCharacter.EndEnlarge();
                enlargingCharacter = null;
            }
        }
        if (shrinkingCharacter != null)
        {
            if (!thisDialogDriver.lineFinished)
            {
                if (shrinkingCharacter.Shrink())
                    shrinkingCharacter = null;

            }
            else
            {
                shrinkingCharacter.EndShrink();
                shrinkingCharacter = null;
            }
        }
    }

    public void CheckEvents(int currLine)
    {
        if (dialogEvents.ContainsKey(currLine))
            dialogEvents[currLine].Invoke();
    }

    public void CheckTextEffects(int currLine)
    {
        if (textEffectAppliers.Where(x => x.line == currLine).Count() <= 0)
            return;

        TextEffectApplier textEffectApplier = textEffectAppliers.Where(x => x.line == currLine).ToArray()[0];
        thisDialogDriver.dialogTreeDriver.textEffectsManager.SetNewEffect(textEffectApplier.word, textEffectApplier.timesAppearedInLine, textEffectApplier.effect, textEffectApplier.affectsAllWord ? null : textEffectApplier.affectedCharsIndexes);

    }

    public bool CheckChoices(string line)
    {
        if(dialogChoices.ContainsKey(line))
        {
            thisDialogDriver.dialogTreeDriver.choiceHandler.DisplayChoiceButtons(dialogChoices[line]);
            return true;
        }
        return false;

    }

    public void ChangeSpeaker(DialogCharacter newSpeaker)
    {
        //text color changes
        thisDialogDriver.dialogTreeDriver.dialogText.color = newSpeaker.textColor;
        //thisDialogDriver.dialogText.material.SetColor("Outline", newSpeaker.textColor);

        //name changes
        thisDialogDriver.dialogTreeDriver.speakerNameBox.color = newSpeaker.textColor;
        thisDialogDriver.dialogTreeDriver.speakerNameBox.text = newSpeaker.characterName;

        //current speaker shrikns
        if (thisDialogDriver.currSpeakerName != null && thisDialogDriver.currSpeakerName != "")
            shrinkingCharacter = new Shrinker(thisDialogDriver.currDialogingCharacters[thisDialogDriver.currSpeakerName].transform, speakerShrinkTime);
        //new speaker enlarges
        enlargingCharacter = new Enlarger(thisDialogDriver.currDialogingCharacters[newSpeaker.characterName].transform, speakerEnlargeTime, speakerObjectiveEnlargement);
        
        thisDialogDriver.currSpeakerName = newSpeaker.characterName;
    }

    DialogCharacter nextUsedCharacter;

    public void SetNextUsedCharacter(DialogCharacter nextUsedCharacter) => this.nextUsedCharacter = nextUsedCharacter;
    public void AddCharacter(string characterPosition)
    {
        GameObject newCharacter = Instantiate(nextUsedCharacter.characterPrefab, thisDialogDriver.dialogTreeDriver.characterPositions[characterPosition]);
        newCharacter.transform.localPosition = new Vector2(newCharacter.transform.localPosition.x, 0);
        newCharacter.transform.SetParent(Camera.main.transform, true); //no se si deberia ser true o false
        thisDialogDriver.currDialogingCharacters.Add(nextUsedCharacter.characterName, newCharacter);
        nextUsedCharacter = null;
    }

    public void ChangeCharacterExpression(string newExpression)
    {
        //change dialoging character's expression to newExpression
        thisDialogDriver.currDialogingCharacters[nextUsedCharacter.characterName].GetComponent<CharacterInstance>().expression.sprite = nextUsedCharacter.facialExpresions[newExpression];
    }

    public void ChangeCharacterPose(string newPose)
    {
        thisDialogDriver.currDialogingCharacters[nextUsedCharacter.characterName].GetComponent<CharacterInstance>().body.sprite = nextUsedCharacter.bodyPoses[newPose].poseSprite;
        thisDialogDriver.currDialogingCharacters[nextUsedCharacter.characterName].GetComponent<CharacterInstance>().expression.transform.position = nextUsedCharacter.bodyPoses[newPose].expressionPosition;
    }

    #region CharacterMovementHandling
    CharacterMovementHandler movementHandler;
    
    public void SetNewMoverName(string characterName) => movementHandler.newMoverName = characterName;
    public void IsNewMoverFast(bool isFast) => movementHandler.nextMovingFast = isFast;

    public void MoveCharacter(string newPosition)
    {
        //moves thisDialogDriver.currDialogingCharacters[characterName] to thisDialogDriver.characterPositions[newPosition]

        movementHandler.StartMovement(movementHandler.newMoverName, newPosition);
    }
    #endregion
}

class Enlarger
{
    Transform objectToEnlarge;
    Vector2 originalSize;
    float enlargementSpeed;
    float objectiveIncrementPercent;

    public Enlarger(Transform objectToEnlarge, float enlargeTime, float objectiveIncrementMultiplier)
    {
        this.objectToEnlarge = objectToEnlarge;
        originalSize = new Vector2(objectToEnlarge.localScale.x, objectToEnlarge.localScale.y);
        enlargementSpeed = (objectiveIncrementMultiplier - 1) / enlargeTime;
        objectiveIncrementPercent = objectiveIncrementMultiplier;
    }

    float elapsedIncrease;

    public bool Enlarge()
    {
        if(originalSize.x + elapsedIncrease >= originalSize.x * objectiveIncrementPercent) 
        {
            EndEnlarge();
            return true;
        }
        float increase = enlargementSpeed * Time.deltaTime;
        objectToEnlarge.localScale = new Vector3(objectToEnlarge.localScale.x + increase, objectToEnlarge.localScale.y + increase, objectToEnlarge.localScale.z);
        elapsedIncrease += increase;
        return false;
    }

    public void EndEnlarge() => objectToEnlarge.localScale = originalSize * objectiveIncrementPercent;
}

class Shrinker
{
    Transform objectToShrink;
    float shrinkSpeed;

    public Shrinker(Transform objectToShrink, float shrinkTime)
    {
        this.objectToShrink = objectToShrink;
        shrinkSpeed = (objectToShrink.localScale.x - 1) / shrinkTime;
    }

    public bool Shrink()
    {
        if (objectToShrink.localScale.x <= 1)
        {
            EndShrink();
            return true;
        }

        float decrease = shrinkSpeed * Time.deltaTime;
        objectToShrink.localScale = new Vector3(objectToShrink.localScale.x - decrease, objectToShrink.localScale.y - decrease, objectToShrink.localScale.z);
        return false;
    }

    public void EndShrink() => objectToShrink.localScale = Vector2.one;
}

[System.Serializable]
class TextEffectApplier
{
    public int line;
    public string word;
    public int timesAppearedInLine;
    public bool affectsAllWord;
    public int[] affectedCharsIndexes;
    public TextEffect effect;
}