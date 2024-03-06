using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "ScriptableObjects/Dialogs/Character", order = 1)]
public class DialogCharacter : ScriptableObject
{
    public GameObject characterPrefab;
    public SerializedDictionary<string, BodyPose> bodyPoses;
    public SerializedDictionary<string, Sprite> facialExpresions;
    public Color textColor;
    public string characterName;
}

[System.Serializable]
public class BodyPose
{
    public Sprite poseSprite;
    public Vector2 expressionPosition;
}