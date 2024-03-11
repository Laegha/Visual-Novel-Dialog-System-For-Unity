using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextEffectsManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    string currLine;
    List<Word> wordsInLine = new List<Word>();
    Dictionary<TextEffect, List<int>> modifiedCharIndexes = new Dictionary<TextEffect, List<int>>();

    void Update()
    {
        //text.ForceMeshUpdate();
        //var textInfo = text.textInfo;
        //var charInfo = textInfo.characterInfo[0];
    }

    public void SetNewLine(string newLine)
    {
        currLine = newLine;

        SeparateWords();
        
        modifiedCharIndexes.Clear();
    }

    void SeparateWords()
    {
        string addingWord = "";
        List<int> wordIndexes = new List<int>();
        for (int i = 0; i < currLine.Length; i++)
        {
            if (currLine[i] == ' ')
            {
                wordsInLine.Add(new Word(addingWord, wordIndexes.ToArray(), wordsInLine.Where(x => x.word == addingWord).Count()));
                wordIndexes.Clear();
                addingWord = "";
                continue;
            }
            if (currLine[i] == '.' || currLine[i] == ',' || currLine[i] == '!' || currLine[i] == '¡' || currLine[i] == '?' || currLine[i] == '¿' || currLine[i] == ':' || currLine[i] == ';')
            {
                wordsInLine.Add(new Word(currLine[i] + "", new int[] { i }, wordsInLine.Where(x => x.word == currLine[i] + "").Count()));
            }
            addingWord += currLine[i];
            wordIndexes.Add(i);
        }
    }

    public void SetNewEffect(string affectedWord, int timesAppearedInLine, TextEffect effect, int[] affectedChars = null)
    {
        Word effectAffectedWord = wordsInLine.Where(x => x.word == affectedWord).Where(x => x.timesAppearedInLine == timesAppearedInLine).ToArray()[0];
        //if(affectdChars != null)
        //añadir todo al affectedCharIndexes. si existe la key del efecto agregar ahi, sino crear una nueva
        //else
        //añadir los chars del array con effectAffectedWord.charIndexesInLine[0] + affectedChars[i]


        if (!modifiedCharIndexes.ContainsKey(effect))
            modifiedCharIndexes.Add(effect, new List<int>());

        foreach (int i in affectedChars != null ? effectAffectedWord.charIndexesInLine : affectedChars) //if the whole word is affected, check all the character indexes, else check only the specified
            modifiedCharIndexes[effect].Add(affectedChars != null ? i : effectAffectedWord.charIndexesInLine[i]); //if the whole word is affected, add the indexes in the array, else add the specified indexes
    }

    public void ApplyEffectsToCharacter(int characterIndex)
    {
        //foreach (KeyValuePair<TextEffect, List<int>> modifiedChars in modifiedCharIndexes)
            //if (modifiedChars.Value.Contains(characterIndex))
                //aplicar el efecto de modifiedChars.Key
    }

    //public void GetEffectsIndex()
    //{
    //    for (int i = 0; i < currLine.Length; i++) //each character in line
    //    {
    //        if (currLine[i] == ' ' || currLine[i] == '.' || currLine[i] == ',' || currLine[i] == '!' || currLine[i] == '¡' || currLine[i] == '?' || currLine[i] == '¿' || currLine[i] == ':' || currLine[i] == ';')
    //            continue;

    //        foreach(KeyValuePair<string, List<TextEffect>> list in effects) //each effect per character
    //        {
    //            List<int> indexes = new List<int>();
    //            for (int j = 0; j < list.Key.Length; j++)
    //            {
    //                int affectedCharIndexInLine = i + j;
    //                if (currLine[affectedCharIndexInLine] != list.Key[j])
    //                {
    //                    indexes.Clear();
    //                    break;
    //                }

    //                indexes.Add(affectedCharIndexInLine);

    //            }

    //        }
    //    }
    //}

}

class Word
{
    public string word;
    public int[] charIndexesInLine;
    public int timesAppearedInLine;

    public Word(string word, int[] charIndexesInLine, int timesAppearedInLine) 
    {
        this.word = word;
        this.charIndexesInLine = charIndexesInLine;
        this.timesAppearedInLine = timesAppearedInLine;
    }
}
#region Basic effect struct
//var textInfo = text.textInfo;
//for(int i = 0; i < textInfo.characterCount; i++) 
//{
//    var charInfo = textInfo.characterInfo[i];
//    if(!charInfo.isVisible)
//        continue;

//    var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
//}
#endregion