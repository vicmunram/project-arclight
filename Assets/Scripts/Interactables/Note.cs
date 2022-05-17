using UnityEngine;

public class Note : Interactable
{
    public string dialogueName;
    private Dialogue dialogue;

    public override void FirstInteraction()
    {
        dialogue = gameObject.AddComponent<Dialogue>();
        dialogue.dialogueName = dialogueName;
    }

    public override void EveryInteraction()
    {
        if (interactText.text != null)
        {
            interactText.text = null;
        }

        if (!dialogue.active)
        {
            dialogue.Activate();
        }
    }

    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }

}
