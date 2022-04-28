using UnityEngine;


public class LeftGlove : Interactable
{
    public string dialogueName;
    public float stretchTime;
    private Dialogue dialogue;
    private MovableGroup exit;
    private bool openedExit;

    void Update()
    {
        if (dialogue != null && dialogue.lastLine && !openedExit)
        {
            exit.active = true;
            openedExit = true;
        }
    }

    public override void FirstInteraction()
    {
        GetComponent<Renderer>().enabled = false;
        interactText.text = null;

        dialogue = gameObject.AddComponent<Dialogue>();
        dialogue.dialogueName = dialogueName;

        exit = GameObject.Find("Exit").GetComponent<MovableGroup>();
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
            playerControl.maxHits = 2;
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.maxSpeed = 5f;
            playerMovement.canPunch = true;

            GameProgress.SaveProgress(0, stretchTime, true);

            Timer.Start(stretchTime);
        }
    }

    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }

}
