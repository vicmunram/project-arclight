using UnityEngine;
using UnityEngine.UI;

public class LeftGlove : Interactable
{
    public string dialogueName;
    public float stretchTime;
    private Dialogue dialogue;
    private MovableGroup exit;
    private bool displayActive;
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
            if (dialogueLine.x == dialogueLine.y - 1 && !displayActive)
            {
                GameObject timerDisplays = GameObject.Find("Timer Displays");
                timerDisplays.GetComponent<Image>().enabled = true;
                timerDisplays.GetComponentsInChildren<Text>()[0].text = "00:00";
                displayActive = true;
            }
            if (dialogueLine.x == dialogueLine.y && !exitOpened)
            {
                exit.active = true;
                exitOpened = true;
            }
        }

        if (Timer.enabled == false && !dialogue.active && exitOpened)
        {
            GetComponent<Collider2D>().enabled = false;

            GameProgress.SaveProgress(0, stretchTime, true);

            Timer.Start(stretchTime);
            AudioUtils.PlaySectionMusic();
        }
    }

    public override void FirstInteraction()
    {
        Remove();
        interactText.text = null;

        PlayerMovement playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerMovement.canPunch = true;
        playerMovement.SetPlayerState();

        dialogue.Activate();
    }

    public override void EveryInteraction() { }
    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }

}
