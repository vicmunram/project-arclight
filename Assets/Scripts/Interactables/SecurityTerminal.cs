using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Canvas))]
public class SecurityTerminal : Interactable
{
    public bool login;
    public bool decrypted;
    public float stretchTime;
    public string path;
    private MovableGroup exit;
    private Text infoMessage;
    private string sourcePath = "Terminals/";
    private string[] lines;
    private int index = 0;

    public void Start()
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
                interactText.text = Localization.GetLocalizedString("ACCESS");
            }
            else
            {
                if (!login)
                {
                    login = true;
                    infoMessage.text = Localization.GetLocalizedString("PROTECTED_DOC");
                    infoMessage.fontSize = 9;
                    interactText.text = Localization.GetLocalizedString("DECRYPT");
                }
                else
                {
                    if (index < lines.Length - 1)
                    {
                        infoMessage.text = lines[index];
                        index++;
                        interactText.text = index < lines.Length - 1 ? Localization.GetLocalizedString("NEXT") : Localization.GetLocalizedString("CLOSE");
                    }
                    else
                    {
                        infoMessage.alignment = TextAnchor.MiddleCenter;
                        infoMessage.fontSize = 25;
                        decrypted = true;
                        defaultMessage = null;
                        interactText.text = Localization.GetLocalizedString("SHOW_ROOMS");
                        infoMessage.text = null;
                        defaultMessage = "SHOW_ROOMS";

                        exit.active = true;
                        Timer.Restart(stretchTime);
                        AudioUtils.PlaySectionMusic();
                    }
                }
            }
        }
        else
        {
            interactText.text = null;
            defaultMessage = "BLANK";
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

    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }

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

}
