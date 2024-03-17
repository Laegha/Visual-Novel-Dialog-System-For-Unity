using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class TextEffect : ScriptableObject
{
    [HideInInspector] public TMP_CharacterInfo charInfo;
    //public void GetNewChar(TextMeshProUGUI text)
    //{
    //    var textInfo = text.textInfo;
    //    charInfo = textInfo.characterInfo[textInfo.characterInfo.Length -1];
    //}
    public abstract void ApplyEffect(TextMeshProUGUI text, int charIndex);
}
