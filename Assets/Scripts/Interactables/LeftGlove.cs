using UnityEngine;
using UnityEngine.UI;

public class LeftGlove : Interactable
{
    public string dialogueName;
    public float stretchTime;
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

        if (Timer.enabled == false && !dialogue.active && exitOpened)
        {
            GetComponent<Collider2D>().enabled = false;

            PlayerMovement playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
            playerMovement.canPunch = true;
            playerMovement.SetPlayerState();

            GameProgress.SaveProgress(0, stretchTime, true);

            GameObject.Find("Timer Displays").GetComponent<Image>().enabled = true;
            Timer.Start(stretchTime);
            AudioUtils.PlaySectionMusic();
        }
    }

    public override void FirstInteraction()
    {
        Remove();
        interactText.text = null;

        GameObject.Find("Player").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Player2");

        dialogue.Activate();
    }

    public override void EveryInteraction() { }
    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }

}
