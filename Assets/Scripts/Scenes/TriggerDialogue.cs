using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerDialogue : Changer
{
    public string dialogueName;
    private Dialogue dialogue;
    private bool dialogueClosed = false;

    void Start()
    {
        dialogue = gameObject.AddComponent<Dialogue>();
        dialogue.dialogueName = dialogueName;
    }
    void Update()
    {
        if (dialogue.loaded)
        {
            Vector2Int dialogueLine = dialogue.GetDialogueLine();
            if (dialogueLine.x == dialogueLine.y)
            {
                dialogueClosed = true;
            }
        }
    }

    protected override void Change()
    {
        active = false;
        dialogue.Activate();
    }
}
