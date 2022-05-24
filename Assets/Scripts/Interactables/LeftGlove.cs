using UnityEngine;


public class LeftGlove : Interactable
{
    public string dialogueName;
    public float stretchTime;
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

    public override void FirstInteraction()
    {
        GetComponent<Renderer>().enabled = false;
        interactText.text = null;

        dialogue.Activate();
    }

    public override void EveryInteraction() { }
    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }

}
