using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public bool talking;
    public bool canInteract;
    public bool paused;
    private Vector3 checkpoint;
    public int hits;
    public int maxHits = 1;

    private Vector2 boxSize = new Vector2(0.1f, 0.1f);

    void Start()
    {
        hits = maxHits;
        checkpoint = transform.position;
        CheckHelp();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!talking && canInteract)
            {
                CheckInteraction();
            }
        }
        else if (!talking && Input.GetKeyDown(KeyCode.H))
        {
            AskForHelp();
        }

        if (hits < 1 || Input.GetKeyDown(KeyCode.K))
        {
            Respawn(false);
        }

        if (Timer.enabled)
        {
            if (Timer.Subtract(Time.deltaTime) <= 0)
            {
                Respawn(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Pause();
            }
            else
            {
                Resume();
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
            Dialogue helpDialogue = help.GetComponent<Dialogue>();
            PlayerMovement playerMovement = GetComponent<PlayerMovement>();
            if (!playerMovement.isPreparing && !playerMovement.isPunching && !playerMovement.isDashing && !playerMovement.isRepelled)
            {
                helpDialogue.Activate();
            }
        }
    }

    private void CheckHelp()
    {
        GameObject help = GameObject.Find("Help");
        if (help != null)
        {
            GameObject.Find("Help Button").GetComponent<Image>().enabled = true;
        }
    }

    public void SetRespawnPoint(Vector3 position)
    {
        checkpoint = position;
    }

    public void SetHits(int currentHits)
    {
        string playerNumber = GetComponent<PlayerMovement>().canDash == true ? "3" : "2";
        if (currentHits != 0)
        {
            AudioUtils.PlayEffect("hit");
            if (currentHits == 3)
            {
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Player" + playerNumber + "Barrier2");
            }
            else if (currentHits == 2)
            {
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Player" + playerNumber + "Barrier");
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Player" + playerNumber);
            }
        }
        else
        {
            AudioUtils.PlayEffect("kill");
            string path = "Sprites/Player" + playerNumber + "Barrier";
            if (playerNumber == "3")
            {
                path = path + "2";
            }
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
        }
        hits = currentHits;
    }

    public void Respawn(bool loadCheckpoint)
    {
        if (!loadCheckpoint)
        {
            transform.position = checkpoint;
            hits = maxHits;
        }
        else
        {
            StartCoroutine(RespawnTimer());
        }
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
        GetComponent<PlayerMovement>().canMove = false;
        SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1;
        GetComponent<PlayerMovement>().canMove = true;
        SceneManager.UnloadSceneAsync("Pause");
    }

    IEnumerator RespawnTimer()
    {
        Timer.enabled = false;

        GetComponent<PlayerMovement>().Stop();
        yield return new WaitForSeconds(0.5f);

        PlayerUI playerUI = GameObject.Find("Player UI").GetComponent<PlayerUI>();
        playerUI.fullDisplay.text = Localization.GetLocalizedString("RESPAWNING");
        int timeRemaining = 2;
        while (timeRemaining > 0)
        {
            playerUI.timeDisplay.text = null;
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        string checkpoint = GameProgress.LoadProgress().GetCurrentCheckpointSceneName();
        SceneManager.LoadScene(checkpoint);
        Timer.enabled = true;
        Timer.Reset();
    }

}