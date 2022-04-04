using UnityEngine;

public class ExtraTime : Interactable
{
    public int time;

    public override void Interact()
    {
        Timer.Add(time);
        Destroy(this.gameObject);
    }
}
