using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

[RequireComponent(typeof(Canvas))]
public class SecurityTerminal : Interactable
{
    public bool login;
    public bool decrypted;
    public float stretchTime;
    public Text infoMessage;
    public string path;
    public MovableGroup doors;
    private string sourcePath = "Assets/Resources/Terminals/";
    private StreamReader file = null;
    private string nextLine = null;
    public override void FirstInteraction() { }
    public override void EveryInteraction()
    {
        if (!decrypted)
        {
            if (Timer.enabled)
            {
                GameObject.Find("Player").GetComponent<PlayerControl>().SaveProgress(new Vector2(transform.position.x, transform.position.y));
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

                        doors.Move();
                        Timer.Restart(stretchTime);
                    }
                }
            }
        }
        else
        {
            interactText.text = "Bloqueado";
        }
    }

    void Update()
    {
        string time = Timer.GetFormattedTime();
        if (Timer.enabled && !time.Equals(infoMessage.text))
        {
            infoMessage.text = time;
        }
    }

    public override void OnEnter(Collider2D collision) { }
    public override void OnExit(Collider2D collision) { }
}
