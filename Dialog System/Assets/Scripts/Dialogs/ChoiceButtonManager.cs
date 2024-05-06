using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChoiceButtonManager : MonoBehaviour
{
    [SerializeField] GameObject butonPrefab;
    [SerializeField] RectTransform grid;

    [HideInInspector] public Action<int> clickedAction;//this will always be ChoiceHandler.OnOptionSelected, which changes branch from current DialogDriver

    public void GenerateButtons(ChoiceOption[] options)
    {
        foreach(ChoiceOption option in options)
        {
            GameObject instantiatedBtn = Instantiate(butonPrefab, grid);
            ChoiceButton choiceBtn = instantiatedBtn.GetComponent<ChoiceButton>();
            choiceBtn.onClick.AddListener(() => clickedAction.Invoke(choiceBtn.addedBranchIndex));
            choiceBtn.addedBranchIndex = option.branchIndex;

            TextMeshProUGUI text = instantiatedBtn.transform.GetComponentInChildren<TextMeshProUGUI>();
            text.text = option.text;
        }
    }
}
