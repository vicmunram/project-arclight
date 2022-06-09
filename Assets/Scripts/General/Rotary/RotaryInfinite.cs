public class RotaryInfinite : Rotary
{
    public bool BackWhenCompleted;
    public override void WhenCompleted()
    {
        if (BackWhenCompleted)
        {
            grades = -grades;
        }
        rotated = 0;
    }

    public override void WhenAuxCompleted() { }

}
