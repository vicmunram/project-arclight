using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Dialogue : MonoBehaviour
{
    public bool closed = false;
    public bool lastLine = false;
    public string dialogueName;
    private Text speaker;
    private Text sentence;
    private Text interaction;
    private Image leftSpeaker;
    private Image rightSpeaker;
    private string leftSpeakerSprite;
    private string rightSpeakerSprite;
    private string mainPath = "Assets/Resources/Dialogues/";
    private string charactersPath = "Characters/";
    private StreamReader textFile = null;
    private string nextLine = null;
    private string nextSentence = "[E] Siguiente";
    private string closeDialogue = "[E] Cerrar";
    private bool reactivateTimer = false;
    private PlayerMovement playerMovement;

    private void initUI()
    {
        GameObject dialogueUI = GameObject.Find("Dialogue UI");
        foreach (Text text in dialogueUI.GetComponentsInChildren<Text>())
        {
            switch (text.name)
            {
                case "Speaker":
                    speaker = text;
                    break;
                case "Sentence":
                    sentence = text;
                    break;
                case "Interaction":
                    interaction = text;
                    break;
            }
        }
        foreach (Image image in dialogueUI.GetComponentsInChildren<Image>())
        {
            switch (image.name)
            {
                case "Left Speaker":
                    leftSpeaker = image;
                    break;
                case "Right Speaker":
                    rightSpeaker = image;
                    break;
            }
        }

    }

    public void open()
    {
        initUI();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerMovement.Stop();
        if (Timer.enabled)
        {
            reactivateTimer = true;
            Timer.enabled = false;
        }

        leftSpeaker.enabled = true;
        rightSpeaker.enabled = true;

        textFile = new StreamReader(mainPath + dialogueName + ".txt");
    }

    public void read()
    {
        if (!lastLine)
        {
            if (nextLine == null)
            {
                nextLine = textFile.ReadLine();
            }

            string[] lineElements = nextLine.Split(';');
            if (lineElements[0] == "L")
            {
                if (leftSpeakerSprite != lineElements[1])
                {
                    leftSpeakerSprite = lineElements[1];
                    leftSpeaker.sprite = Resources.Load<Sprite>(charactersPath + leftSpeakerSprite);
                }
            }
            else
            {
                if (rightSpeakerSprite != lineElements[1])
                {
                    rightSpeakerSprite = lineElements[1];
                    rightSpeaker.sprite = Resources.Load<Sprite>(charactersPath + rightSpeakerSprite);
                }
            }

            speaker.text = lineElements[2];
            sentence.text = lineElements[3];
            nextLine = textFile.ReadLine();
            interaction.text = nextLine != null ? nextSentence : closeDialogue;

            if (nextLine == null)
            {
                lastLine = true;
            }
        }
        else
        {
            lastLine = false;
            closed = true;
        }
    }

    public void close()
    {
        leftSpeaker.enabled = false;
        rightSpeaker.enabled = false;
        sentence.text = null;
        interaction.text = null;
        speaker.text = null;

        if (reactivateTimer)
        {
            Timer.enabled = true;
        }
        playerMovement.canMove = true;
        closed = false;
    }

}
