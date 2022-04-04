using UnityEngine;
using UnityEngine.SceneManagement;

public class LeftGlove : Interactable
{
    public Dialogue dialogue;
    public override void Interact()
    {
        if (!interacted)
        {
            GetComponent<Renderer>().enabled = false;
            interacted = true;
            dialogue.open();
        }

        dialogue.read();

        if (dialogue.closed)
        {
            dialogue.close();
            GetComponent<Collider2D>().enabled = false;
            PlayerPrefs.SetString("checkpoint", SceneManager.GetActiveScene().name + "B");
            Timer.Start(10f);
        }
    }

}
