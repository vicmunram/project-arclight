using UnityEngine;

public class SecondTrap : Button
{
    public string dialogueName;
    private Dialogue dialogue;
    private bool lastLine = true;

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
            if (dialogueLine.x == dialogueLine.y && lastLine)
            {
                lastLine = false;

                GameObject.Find("Last Trap Door").GetComponent<MovableGroup>().active = true;
            }
        }
    }
    public override void FirstInteraction()
    {
        Remove();
        interactText.text = null;

        dialogue.Activate();
    }
}
