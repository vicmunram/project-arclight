using UnityEngine;

public class ExtraTime : Interactable
{
    public int time;

    public override void FirstInteraction() { }
    public override void EveryInteraction()
    {
        Timer.Add(time);
        Destroy(gameObject);
    }

    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }
}
