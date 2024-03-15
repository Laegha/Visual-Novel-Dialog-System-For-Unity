using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;

public class TextEffectsManager
{
    TextMeshProUGUI text;
    string currLine;
    List<Word> wordsInLine = new List<Word>();
    Dictionary<TextEffect, List<int>> modifiedCharIndexes = new Dictionary<TextEffect, List<int>>();

    public TextEffectsManager(TextMeshProUGUI text)
    {
        this.text = text;
    }

    public void Update()
    {
        text.ForceMeshUpdate();
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

        if (!modifiedCharIndexes.ContainsKey(effect))
            modifiedCharIndexes.Add(effect, new List<int>());

        foreach (int i in affectedChars == null ? effectAffectedWord.charIndexesInLine : affectedChars) //if the whole word is affected, check all the character indexes, else check only the specified
            modifiedCharIndexes[effect].Add(affectedChars == null ? i : effectAffectedWord.charIndexesInLine[i]); //if the whole word is affected, add the indexes in the array, else add the specified indexes
    }

    public void ApplyEffectsToCharacter(int characterIndex)
    {
        if (modifiedCharIndexes.Count <= 0)
            return;
        foreach (KeyValuePair<TextEffect, List<int>> modifiedChars in modifiedCharIndexes)
            if (modifiedChars.Value.Contains(characterIndex))
                modifiedChars.Key.GetNewChar(text);
    }
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