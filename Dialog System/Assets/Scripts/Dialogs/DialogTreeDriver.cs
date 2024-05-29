using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogTreeDriver : MonoBehaviour
{
    [SerializeField] DialogTree dialogTree;

    public DialogDriver currDialogDriver;
    public TextEffectsManager textEffectsManager;
    public ChoiceHandler choiceHandler;

    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerNameBox;
    public SerializedDictionary<string, Transform> characterPositions; //this string should be changed for an enum that includes left, mid-left, mid, mid-right, right

    public ChoiceButtonManager choiceButtonManager;

    private void Awake()
    {
        currDialogDriver = new DialogDriver(this, dialogTree.Dialogs["0"]);
    }

    private void Start()
    {
        textEffectsManager = new TextEffectsManager(dialogText);
        choiceHandler = new ChoiceHandler(this);

        choiceButtonManager.clickedAction = choiceHandler.OnOptionSelected;
        currDialogDriver.Start();
    }

    private void Update()
    {
        if (currDialogDriver == null)
            return;
        currDialogDriver.Update();
    }

    public void NextLinearLine(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        currDialogDriver.NextLinearLine();
    }

    public void OnDialogEnded()
    {
        //currDialogDriver = new DialogDriver(this, define based on conditions)
    }
}
