using UnityEngine;

public class RotaryInfinite : Rotary
{
    public override void WhenCompleted()
    {
        rotated = 0;
    }

    public override void WhenAuxCompleted() { }

}
