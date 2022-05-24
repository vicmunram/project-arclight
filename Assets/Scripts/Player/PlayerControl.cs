using UnityEngine;
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

    void FixedUpdate()
    {
        if (paused)
        {

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

    public void SetRespawnPoint(Vector3 position)
    {
        checkpoint = position;
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
        SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("Pause");
    }

    IEnumerator RespawnTimer()
    {
        Timer.enabled = false;

        GetComponent<PlayerMovement>().Stop();
        yield return new WaitForSeconds(0.5f);

        PlayerUI playerUI = GameObject.Find("Player UI").GetComponent<PlayerUI>();
        int timeRemaining = 2;
        while (timeRemaining > 0)
        {
            playerUI.formattedTime.text = "Reapareciendo...";
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        string checkpoint = GameProgress.LoadProgress().GetCurrentCheckpointSceneName();
        SceneManager.LoadScene(checkpoint);
        Timer.enabled = true;
        Timer.Reset();
    }

}