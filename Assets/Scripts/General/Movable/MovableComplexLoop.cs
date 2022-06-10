using UnityEngine;

public class MovableComplexLoop : Movable
{
    public Vector2 mainPosition;
    public Vector2[] extraDirections;
    private bool first = true;
    private Vector2 firstDirection;
    private int index = 0;
    public override void WhenCompleted()
    {
        if (first)
        {
            firstDirection = direction;
            first = false;
        }

        if (index == extraDirections.Length)
        {
            direction = mainPosition - currentPosition;
        }
        else if (index == extraDirections.Length + 1)
        {
            direction = firstDirection;
        }
        else
        {
            direction = extraDirections[index];
        }
        targetPosition = currentPosition + direction;
        index++;

        if (index >= extraDirections.Length + 2)
        {
            index = 0;
        }
    }

    public override void WhenAuxCompleted() { }

}
