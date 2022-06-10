using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public string dialogueName;
    public bool active = false;
    public bool loaded = false;
    private Text speaker;
    private Text sentence;
    private Text interaction;
    private Image leftSpeaker;
    private Image rightSpeaker;
    private string leftSpeakerSprite;
    private string rightSpeakerSprite;
    private string dialoguesPath = "Dialogues/";
    private string charactersPath = "Characters/";
    private string[] lines;
    private int index = -1;
    private bool reactivateTimer = false;

    // UI
    private GameObject dialogueUI;

    private void InitUI()
    {
        Cursor.visible = false;
        dialogueUI = GameObject.Find("Dialogue UI");

        Canvas canvas = dialogueUI.GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 1;

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

        lines = Localization.GetLocalizedText(dialoguesPath, dialogueName).text.Split(';');

        string[] lineElements = lines[index].Split('_');
        leftSpeakerSprite = lineElements[0];
        leftSpeaker.sprite = Resources.Load<Sprite>(charactersPath + leftSpeakerSprite);
        rightSpeakerSprite = lineElements[1];
        rightSpeaker.sprite = Resources.Load<Sprite>(charactersPath + rightSpeakerSprite);
        index++;
    }

    public void Read()
    {
        if (index < lines.Length - 1)
        {
            string[] lineElements = lines[index].Split('_');
            if (lineElements[0].Trim() == "L")
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

            speaker.text = AddColorToText(lineElements[3], lineElements[2]);
            sentence.text = lineElements[4];

            index++;
            interaction.text = index < lines.Length - 1 ? Localization.GetLocalizedString("NEXT") : Localization.GetLocalizedString("CLOSE");
        }
        else
        {
            Close();
        }
    }

    public void Close()
    {
        Cursor.visible = true;
        SceneManager.UnloadSceneAsync("Dialogue");

        if (reactivateTimer)
        {
            Timer.enabled = true;
        }

        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().canMove = true;
        player.GetComponent<PlayerControl>().talking = false;

        active = false;
        loaded = false;
        index = -1;
    }

    public void Activate()
    {
        SceneManager.LoadSceneAsync("Dialogue", LoadSceneMode.Additive);
        active = true;
    }

    public Vector2Int GetDialogueLine()
    {
        return new Vector2Int(index, lines.Length - 1);
    }

    void Update()
    {
        if (active)
        {
            if (index == -1)
            {
                StartCoroutine(WaitLoadScene());
                index = 0;
            }

            if (loaded && Input.GetKeyDown(KeyCode.E))
            {
                Read();
                AudioUtils.PlayEffect(dialogueUI, true);
            }
        }
    }

    private string AddColorToText(string text, string color)
    {
        return "<color=" + color + ">" + text + "</color>";
    }

    IEnumerator WaitLoadScene()
    {
        while (!SceneManager.GetSceneAt(1).isLoaded)
        {
            yield return null;
        }
        loaded = true;

        Open();
        Read();
    }

}
