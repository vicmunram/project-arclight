using UnityEngine;
using UnityEngine.SceneManagement;

public class Note : Interactable
{
    public Dialogue dialogue;

    public override void FirstInteraction()
    {
        interactText.text = null;
        dialogue.open();
    }

    public override void EveryInteraction()
    {
        dialogue.read();

        if (dialogue.closed)
        {
            dialogue.close();
            interacted = false;
        }
    }

    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }

}
