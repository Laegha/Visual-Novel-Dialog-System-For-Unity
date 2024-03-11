using UnityEngine;

[CreateAssetMenu(fileName = "NewTextEffect", menuName = "ScriptableObjects/Dialogs/TextEffects/ColorChange", order = 1)]
public class ColorChangeTextEffect : TextEffect
{
    [SerializeField] Color textChangeColor;
    public override void ApplyEffectToChar()
    {
        charInfo.color = textChangeColor;
    }
}
