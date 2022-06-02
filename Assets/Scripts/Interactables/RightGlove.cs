using UnityEngine;

public class RightGlove : Interactable
{
    public string dialogueName;
    public float stretchTime;
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

        if (Timer.enabled == false && !dialogue.active && dialogueClosed)
        {
            GetComponent<Collider2D>().enabled = false;

            PlayerMovement playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
            playerMovement.canDash = true;
            playerMovement.SetPlayerState();

            GameProgress.SaveProgress(2, stretchTime, true);
            AudioUtils.PlaySectionMusic();
        }
    }

    public override void FirstInteraction()
    {
        GetComponent<Renderer>().enabled = false;
        interactText.text = null;
        Timer.Start(stretchTime);

        dialogue.Activate();
    }

    public override void EveryInteraction() { }
    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }

}
