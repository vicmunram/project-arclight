using UnityEngine;

public class RightGlove : Interactable
{
    public string dialogueName;
    public float stretchTime;
    private Dialogue dialogue;

    public override void FirstInteraction()
    {
        GetComponent<Renderer>().enabled = false;
        interactText.text = null;

        dialogue = gameObject.AddComponent<Dialogue>();
        dialogue.dialogueName = dialogueName;
    }

    public override void EveryInteraction()
    {
        if (!dialogue.active)
        {
            dialogue.Activate();
        }

        if (dialogue.closed)
        {
            GetComponent<Collider2D>().enabled = false;

            GameObject player = GameObject.Find("Player");
            PlayerControl playerControl = player.GetComponent<PlayerControl>();
            playerControl.maxHits = 3;
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.maxSpeed = 7.5f;
            playerMovement.canDash = true;

            GameProgress.SaveProgress(2, stretchTime, true);

            Timer.Start(stretchTime);
        }
    }

    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }

}
