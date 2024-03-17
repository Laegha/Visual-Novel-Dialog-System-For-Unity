using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTextEffect", menuName = "ScriptableObjects/Dialogs/TextEffects/ColorChange", order = 1)]
public class ColorChangeTextEffect : TextEffect
{
    [SerializeField] Color textChangeColor;
    public override void ApplyEffect(TextMeshProUGUI text, int charIndex)
    {
        Debug.Log(text.textInfo.characterCount + " " + charIndex);
        var charInfo = text.textInfo.characterInfo[charIndex];
        var meshInfo = text.textInfo.meshInfo[charInfo.materialReferenceIndex];

        for (int i = 0; i < 4; i++)
        {
            var index = charInfo.vertexIndex + i;
            meshInfo.colors32[index] = textChangeColor;
        }
    }
}
