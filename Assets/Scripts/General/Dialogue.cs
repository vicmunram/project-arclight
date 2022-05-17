using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Dialogue : MonoBehaviour
{
    public string dialogueName;
    public bool active = false;
    public bool lastLine = false;
    public bool closed = false;
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
    private string line = null;
    private string nextLine = null;
    private string nextSentence = "[E] Siguiente";
    private string closeDialogue = "[E] Cerrar";
    private bool reactivateTimer = false;

    private void InitUI()
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

    public void Open()
    {
        InitUI();

        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().Stop();
        player.GetComponent<PlayerControl>().talking = true;

        if (Timer.enabled)
        {
            reactivateTimer = true;
            Timer.enabled = false;
        }

        leftSpeaker.enabled = true;
        rightSpeaker.enabled = true;

        textFile = new StreamReader(mainPath + dialogueName + ".txt");

        line = null;
        nextLine = null;
    }

    public void Read()
    {
        if (!lastLine)
        {
            if (line == null)
            {
                line = textFile.ReadLine();
            }
            else
            {
                line = nextLine;
            }

            nextLine = textFile.ReadLine();

            string[] lineElements = line.Split(';');
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

    public void Close()
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

        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().canMove = true;
        player.GetComponent<PlayerControl>().talking = false;

        closed = false;
        active = false;
    }

    public void Activate()
    {
        active = true;
        Open();
        Read();
    }

    void Update()
    {
        if (active && Input.GetKeyDown(KeyCode.E))
        {
            Read();

            if (closed)
            {
                Close();
            }
        }
    }

}
