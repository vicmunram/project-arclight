using UnityEngine;

public class FirstTrap : Button
{
    public string dialogueName;
    public GameObject[] extraInteractions;
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

                GameObject.Find("Switch").GetComponent<TypeSwitchGroup>().Switch();
                Destroy(GameObject.Find("Fake"));

                extraInteractions[0].SetActive(true);
            }
        }
    }
    public override void FirstInteraction()
    {
        Remove();
        interactText.text = null;

        PlayerCamera playerCamera = GameObject.Find("Player Camera").GetComponent<PlayerCamera>();
        playerCamera.Resize(14);
        playerCamera.ChangeRange(new Vector2(-1, 1), new Vector2(26.5f, 27.5f));
        GameObject.Find("Main Resizer").GetComponent<CameraChanger>().active = true;
        GameObject.Find("Black PTs").GetComponent<MovableGroup>().active = true;

        Destroy(GameObject.Find("Destroy"));

        dialogue.Activate();

        PlayerControl playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        playerControl.SetRespawnPoint(transform.position);
    }
}
