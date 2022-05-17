using UnityEngine;
using UnityEngine.UI;
using System.IO;
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
    private string sourcePath = "Assets/Resources/Terminals/";
    private StreamReader file = null;
    private string nextLine = null;

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
    public override void FirstInteraction() { }
    public override void EveryInteraction()
    {
        if (!decrypted)
        {
            if (Timer.enabled)
            {
                Timer.enabled = false;
                GameProgress.SaveProgress(Timer.globalTime, stretchTime, false);
                interactText.text = "[E] Acceder";
            }
            else
            {
                if (!login)
                {
                    login = true;
                    infoMessage.text = "Documento restringido";
                    infoMessage.fontSize = 9;
                    interactText.text = "[E] Desencriptar";
                }
                else
                {
                    if (file == null)
                    {
                        infoMessage.alignment = TextAnchor.UpperLeft;
                        file = new StreamReader(sourcePath + path + ".txt");
                    }

                    bool close = false;
                    if (nextLine == null)
                    {
                        nextLine = file.ReadLine();
                        if (nextLine == null)
                        {
                            close = true;
                        }
                    }

                    if (!close)
                    {
                        infoMessage.text = nextLine;
                        nextLine = file.ReadLine();
                        interactText.text = nextLine != null ? "[E] Siguiente" : "[E] Cerrar";
                    }
                    else
                    {
                        infoMessage.alignment = TextAnchor.MiddleCenter;
                        infoMessage.fontSize = 25;
                        decrypted = true;
                        defaultMessage = null;
                        interactText.text = null;
                        infoMessage.text = null;

                        exit.active = true;
                        Timer.Restart(stretchTime);
                    }
                }
            }
        }
        else
        {
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
            button.GetComponentInChildren<Text>().text = "Sala de seguridad " + index;
            button.interactable = saveData.currentCheckpointLevel != (checkpoint.Key);
            button.onClick.AddListener(() => OnClick(checkpoint));
            index++;
        }

        void OnClick(KeyValuePair<int, TimeData> checkpoint)
        {
            GameProgress.LoadCheckpoint(checkpoint);
            GameObject.Find("Player").GetComponent<PlayerControl>().Respawn(true);
        }

    }

}
