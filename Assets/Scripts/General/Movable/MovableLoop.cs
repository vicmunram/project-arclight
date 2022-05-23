using UnityEngine;

public class MovableLoop : Movable
{
    public Vector2 extraDirection;
    private bool first = true;
    public override void WhenCompleted()
    {
        if (first)
        {
            direction = direction + extraDirection;
            first = false;
        }

        direction = direction * -1;
        targetPosition = currentPosition + direction;
    }

    public override void WhenAuxCompleted() { }

}
