using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    private bool asking;
    private bool canInteract;
    private Vector3 checkpoint;
    private Dialogue helpDialogue;
    public int hits = 2;

    private Vector2 boxSize = new Vector2(0.1f, 0.1f);

    void Start()
    {
        checkpoint = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (asking)
            {
                helpDialogue.read();
                if (helpDialogue.closed)
                {
                    helpDialogue.close();
                    GetComponent<PlayerMovement>().canMove = true;
                    asking = false;
                }
            }
            else if (canInteract)
            {
                CheckInteraction();
            }
        }
        else if (!asking && Input.GetKeyDown(KeyCode.H))
        {
            AskForHelp();
        }
        if (hits < 1 || Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
        if (Timer.enabled)
        {
            if (Timer.Subtract(Time.deltaTime) < 0)
            {
                SceneMaster.Respawn();
            }
        }
    }

    public void OpenInteraction()
    {
        canInteract = true;
    }

    public void CloseInteraction()
    {
        canInteract = false;
    }

    private void CheckInteraction()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0, Vector2.zero);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<Interactable>())
                {
                    rc.transform.GetComponent<Interactable>().Interact();
                    break;
                }
            }
        }
    }

    private void AskForHelp()
    {
        GameObject help = GameObject.Find("Help");
        if (help != null)
        {
            helpDialogue = help.GetComponent<Dialogue>();
            PlayerMovement playerMovement = GetComponent<PlayerMovement>();
            if (!playerMovement.isPreparing && !playerMovement.isPunching && !playerMovement.isDashing && !playerMovement.isRepelled)
            {
                helpDialogue.open();
                helpDialogue.read();
                asking = true;
            }
        }
    }

    public void SetRespawnPoint(Vector3 position)
    {
        checkpoint = position;
    }

    public int GetHits()
    {
        return hits;
    }

    public void SetHits(int hits)
    {
        this.hits = hits;
    }

    public void Respawn()
    {
        transform.position = checkpoint;
        hits = 2;
    }

    public void SaveProgress(Vector2 checkpoint)
    {
        Timer.enabled = false;
        SetRespawnPoint(new Vector3(checkpoint.x, checkpoint.y, transform.position.z));
        PlayerPrefs.SetString("checkpoint", SceneManager.GetActiveScene().name + "B");
        PlayerPrefs.SetFloat("globalTime", Timer.globalTime);
    }

}