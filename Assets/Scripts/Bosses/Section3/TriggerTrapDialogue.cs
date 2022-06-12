using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerTrapDialogue : Changer
{
    public string dialogueName;
    private Dialogue dialogue;
    private MovableGroup exit;
    private bool exitOpened;

    void Start()
    {
        dialogue = gameObject.AddComponent<Dialogue>();
        dialogue.dialogueName = dialogueName;

        exit = GameObject.Find("Exit").GetComponent<MovableGroup>();
    }

    void Update()
    {
        if (dialogue.loaded)
        {
            Vector2Int dialogueLine = dialogue.GetDialogueLine();
            if (dialogueLine.x == dialogueLine.y && !exitOpened)
            {
                exit.active = true;
                exitOpened = true;
            }
        }

        if (!dialogue.active && exitOpened)
        {
            GameProgress.SaveProgress(Timer.globalTime, 0, true);
        }
    }

    protected override void Change()
    {
        active = false;
        dialogue.Activate();
    }
}
