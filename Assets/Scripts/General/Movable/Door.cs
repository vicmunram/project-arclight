using UnityEngine;

public class Door : Movable
{
    public bool inactiveWhenCompleted;
    public override void WhenCompleted()
    {
        rb.velocity = new Vector2(0, 0);
        direction = direction * -1;
        targetPosition = currentPosition + direction;
        isMoving = false;
        active = !inactiveWhenCompleted;
    }

    public override void WhenAuxCompleted() { }

}
