using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Canvas))]
public class SecurityTerminal : Interactable
{
    public Text auxInteractText;
    public bool login;
    public bool decrypted;
    public float stretchTime;
    public string path;
    private MovableGroup exit;
    private Text infoMessage;
    private string sourcePath = "Terminals/";
    private string[] lines;
    private int index = 0;

    void Start()
    {
        infoMessage = GameObject.Find("Display").GetComponent<Text>();
        exit = GameObject.Find("Exit").GetComponentInChildren<MovableGroup>();

        if (decrypted && !login && Timer.enabled == false)
        {
            Timer.enabled = true;
            Timer.Reset();
        }
    }

    public override void FirstInteraction()
    {
        lines = Localization.GetLocalizedText(sourcePath, path).text.Split(';');
    }
    public override void EveryInteraction()
    {
        if (!decrypted)
        {
            if (Timer.enabled)
            {
                Timer.enabled = false;
                GameProgress.SaveProgress(Timer.globalTime, stretchTime, false);
                SetDefaultMessage("ACCESS");
            }
            else
            {
                if (!login)
                {
                    login = true;
                    infoMessage.text = Localization.GetLocalizedString("PROTECTED_DOC");
                    infoMessage.fontSize = 7;
                    SetDefaultMessage("DECRYPT");
                }
                else
                {
                    if (index == 0)
                    {
                        infoMessage.alignment = TextAnchor.UpperLeft;
                        infoMessage.text = lines[index].Trim();
                        SetInteractText();
                    }
                    else if (index == lines.Length - 1)
                    {
                        decrypted = true;
                        infoMessage.alignment = TextAnchor.MiddleCenter;
                        infoMessage.fontSize = 25;
                        infoMessage.text = null;
                        auxInteractText.text = null;
                        SetDefaultMessage("SHOW_ROOMS");

                        exit.active = true;
                        Timer.Restart(stretchTime);
                        AudioUtils.PlaySectionMusic();
                    }
                }
            }
        }
        else
        {
            SetDefaultMessage("PICK_ROOM");
            InitButtons();
        }
    }

    void Update()
    {
        string time = Timer.GetFormattedTime();
        if (Timer.enabled && !time.Equals(infoMessage.text) && !decrypted)
        {
            infoMessage.text = time;
        }
    }

    public override void OnEnter(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControl>().terminal = true;
        }
    }
    public override void OnExit(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControl>().terminal = false;
        }
    }

    public void InitButtons()
    {
        SaveData saveData = GameProgress.LoadProgress();
        UnityEngine.UI.Button[] buttons = GetComponentsInChildren<UnityEngine.UI.Button>();

        int index = 0;
        foreach (KeyValuePair<int, TimeData> checkpoint in saveData.GetAvailableCheckpoints())
        {
            UnityEngine.UI.Button button = buttons[index];
            button.GetComponent<Image>().enabled = true;
            button.GetComponentInChildren<Text>().text = Localization.GetLocalizedString("SEC_ROOM") + " " + index;
            button.interactable = saveData.currentCheckpointLevel != (checkpoint.Key);
            button.onClick.AddListener(() => OnClick(checkpoint));
            index++;
        }

        void OnClick(KeyValuePair<int, TimeData> checkpoint)
        {
            AudioUtils.PlayEffect(gameObject, false);
            GameProgress.LoadCheckpoint(checkpoint);
            GameObject.Find("Player").GetComponent<PlayerControl>().Respawn(true);
        }
    }

    public void Next()
    {
        if (!decrypted && !Timer.enabled && login)
        {
            if (index >= 0 && index < lines.Length - 1)
            {
                index++;
                infoMessage.text = lines[index].Trim();
                SetInteractText();
            }
        }
    }

    public void Back()
    {
        if (!decrypted && !Timer.enabled && login)
        {
            if (index > 0 && index <= lines.Length - 1)
            {
                index--;
                infoMessage.text = lines[index].Trim();
                SetInteractText();
            }
        }
    }

    private void SetInteractText()
    {
        if (index == 0)
        {
            SetDefaultMessage("BLANK");
            auxInteractText.text = "<color=grey><</color> >";
        }
        else if (index == lines.Length - 1)
        {
            SetDefaultMessage("CLOSE_TERMINAL");
            auxInteractText.text = "< <color=grey>></color>";
        }
        else
        {
            SetDefaultMessage("BLANK");
            auxInteractText.text = "< >";
        }
    }

}
