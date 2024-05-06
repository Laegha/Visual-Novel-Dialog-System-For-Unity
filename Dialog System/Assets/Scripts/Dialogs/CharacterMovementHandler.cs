using UnityEngine;

public class CharacterMovementHandler
{
    DialogDriver thisDialogDriver;

    public string newMoverName;
    public bool nextMovingFast;
    Mover[] movers = new Mover[3];

    float normalMoveTime = .5f;
    float fastMoveTime = .2f;

    public CharacterMovementHandler(DialogDriver dialogDriver) => thisDialogDriver = dialogDriver;

    public void StartMovement(string characterName, string newPosition)
    {
        //move thisDialogDriver.currDialogingCharacters[characterName] to thisDialogDriver.characterPositions[newPosition]
        if (movers[0] == null)
            movers[0] = new Mover(thisDialogDriver.dialogTreeDriver.characterPositions[newPosition].localPosition.x, nextMovingFast ? fastMoveTime : normalMoveTime, thisDialogDriver.currDialogingCharacters[characterName].transform);
        else if (movers[1] == null)
            movers[1] = new Mover(thisDialogDriver.dialogTreeDriver.characterPositions[newPosition].localPosition.x, nextMovingFast ? fastMoveTime : normalMoveTime, thisDialogDriver.currDialogingCharacters[characterName].transform);
        else if (movers[2] == null)
            movers[2] = new Mover(thisDialogDriver.dialogTreeDriver.characterPositions[newPosition].localPosition.x, nextMovingFast ? fastMoveTime : normalMoveTime, thisDialogDriver.currDialogingCharacters[characterName].transform);

    }

    public void Update()
    {
        for (int i = 0; i < movers.Length; i++)
            if (movers[i] != null)
            {
                if (!thisDialogDriver.lineFinished)
                {
                    if (movers[i].MoveTo())
                        movers[i] = null;

                }
                else
                {
                    movers[i].EndMovement();
                    movers[i] = null;
                }
            }
    }
}
class Mover
{
    int direction;//-1 or 1 as it only moves in the x axis
    float objectivePosition;
    float speed;
    Transform movedObject;
    public Mover(float objectivePosition, float moveTime, Transform movedObject)
    {
        distance = objectivePosition - movedObject.position.x;
        direction = distance < 0 ? -1 : 1;
        this.objectivePosition = objectivePosition;
        speed = (distance > 0 ? distance : distance * -1) / moveTime;
        this.movedObject = movedObject;
    }

    float elapsedDistance;
    float distance;
    public bool MoveTo()
    {
        if (elapsedDistance >= distance)
        {
            EndMovement();
            return true;
        }

        float delta = speed * Time.deltaTime;
        movedObject.Translate(direction * delta, 0f, 0f);
        elapsedDistance += delta;
        return false;
    }

    public void EndMovement() => movedObject.transform.localPosition = new Vector3(objectivePosition, movedObject.transform.localPosition.y, movedObject.transform.localPosition.z);
}