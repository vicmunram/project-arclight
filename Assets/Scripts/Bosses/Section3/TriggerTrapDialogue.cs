using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerTrapDialogue : Changer
{
    public string dialogueName;
    private Dialogue dialogue;
    private MovableGroup exit;

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
            if (dialogueLine.x == dialogueLine.y && !exit.active)
            {
                exit.active = true;
            }
        }

        if (!dialogue.active && exit.active)
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
