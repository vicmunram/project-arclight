using UnityEngine;
using UnityEngine.SceneManagement;

public class LeftGlove : Interactable
{
    public Dialogue dialogue;
    public MovableGroup doors;

    public override void FirstInteraction()
    {
        GetComponent<Renderer>().enabled = false;
        interactText.text = null;
        dialogue.open();
    }

    public override void EveryInteraction()
    {
        dialogue.read();

        if (dialogue.lastLine)
        {
            doors.Move();
        }

        if (dialogue.closed)
        {
            dialogue.close();

            GetComponent<Collider2D>().enabled = false;
            PlayerPrefs.SetString("checkpoint", SceneManager.GetActiveScene().name + "B");

            PlayerMovement playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
            playerMovement.maxSpeed = 5f;
            playerMovement.canPunch = true;

            Timer.Start(30f);
        }
    }

    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }

}
