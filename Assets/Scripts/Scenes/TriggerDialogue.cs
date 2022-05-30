using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerDialogue : Changer
{
    public string dialogueName;
    private Dialogue dialogue;

    void Start()
    {
        dialogue = gameObject.AddComponent<Dialogue>();
        dialogue.dialogueName = dialogueName;
    }

    protected override void Change()
    {
        active = false;

        AudioUtils.PlayEffect(gameObject, false);

        dialogue.Activate();
    }
}
