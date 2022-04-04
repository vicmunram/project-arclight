using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Dialogue : MonoBehaviour
{
    public bool closed = false;
    public Text message;
    public Text interactionText;
    public Image leftImage;
    public Image rightImage;
    private string mainPath = "Assets/Resources/Dialogues/";
    public string dialogueName;
    private StreamReader textFile = null;
    private string nextLine = null;
    private string nextMessage = "[E] Siguiente";
    private string closeMessage = "[E] Cerrar";
    private bool reactivateTimer = false;
    private PlayerMovement playerMovement;

    public void open()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerMovement.Stop();
        if (Timer.enabled)
        {
            reactivateTimer = true;
            Timer.enabled = false;
        }

        leftImage.enabled = true;
        rightImage.enabled = true;

        textFile = new StreamReader(mainPath + dialogueName + ".txt");
    }

    public void read()
    {
        bool close = false;
        if (nextLine == null)
        {
            nextLine = textFile.ReadLine();
            if (nextLine == null)
            {
                close = true;
            }
        }

        if (!close)
        {
            message.text = nextLine;
            nextLine = textFile.ReadLine();
            interactionText.text = nextLine != null ? nextMessage : closeMessage;
        }
        else
        {
            closed = true;
        }
    }

    public void close()
    {
        leftImage.enabled = false;
        rightImage.enabled = false;
        message.text = null;
        interactionText.text = null;

        if (reactivateTimer)
        {
            Timer.enabled = true;
        }
        playerMovement.canMove = true;
        closed = false;
    }

}
