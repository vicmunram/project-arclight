using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

[RequireComponent(typeof(Canvas))]
public class SecurityTerminal : Interactable
{
    public bool decrypted;
    public float stretchTime;
    public Text infoMessage;
    public string path;
    private string sourcePath = "Assets/Resources/Terminals/";
    private StreamReader file = null;
    private string nextLine = null;
    public override void Interact()
    {
        if (!decrypted)
        {
            if (Timer.enabled)
            {
                Timer.enabled = false;

                PlayerPrefs.SetString("checkpoint", SceneManager.GetActiveScene().name + "B");
                GameObject player = GameObject.Find("Player");
                player.GetComponent<PlayerControl>().SetCheckpoint(new Vector3(transform.position.x, transform.position.y, player.transform.position.z));
                PlayerPrefs.SetFloat("globalTime", Timer.globalTime);

                interactText.text = "[E] Desencriptar";
            }
            else
            {
                if (file == null)
                {
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
                    Disable();
                    infoMessage.text = "ALARMA";
                    interactText.text = "Bloqueado";
                    Timer.Restart(stretchTime);
                }
            }
        }
        else
        {
            interactText.text = "Bloqueado";
        }
    }
}
