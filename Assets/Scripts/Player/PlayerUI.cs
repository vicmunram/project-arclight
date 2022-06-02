using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    // TIMER
    public Text timeDisplay;
    public Text extraDisplay;
    private float extraTime;
    private bool fading;
    public Text fullDisplay;
    // MENUS
    public Texture2D crosshair;
    public Texture2D cursor;
    private bool cursorChanged;
    private bool wasTalking;
    private PlayerControl playerControl;

    void Start()
    {
        Cursor.SetCursor(crosshair, new Vector2(crosshair.width / 2, crosshair.height / 2), CursorMode.Auto);
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        timeDisplay.text = Timer.enabled ? Timer.GetFormattedTime() : null;
    }

    void Update()
    {
        string time = Timer.GetFormattedTime();
        if (Timer.enabled && !time.Equals(timeDisplay.text))
        {
            timeDisplay.text = time;
        }

        if (extraTime != 0 && !fading)
        {
            StartCoroutine(FadeExtra());
        }

        ChangeCursor();
    }

    public void AddTime(float time)
    {
        if (extraTime != 0)
        {
            extraTime += time;
        }
        else
        {
            extraTime = time;
        }
        extraDisplay.text = "+" + time;
    }

    public void ChangeCursor()
    {
        if (playerControl.paused && !cursorChanged)
        {
            Cursor.visible = true;
            Cursor.SetCursor(cursor, new Vector2(0, 0), CursorMode.Auto);
            cursorChanged = true;
        }
        if (!playerControl.paused && cursorChanged)
        {
            Cursor.SetCursor(crosshair, new Vector2(crosshair.width / 2, crosshair.height / 2), CursorMode.Auto);
            cursorChanged = false;
            if (playerControl.talking && Cursor.visible)
            {
                Cursor.visible = false;
            }
        }
    }

    private IEnumerator FadeExtra()
    {
        fading = true;
        float fadeTime = 2f;
        float currentExtraTime = extraTime;

        extraDisplay.CrossFadeAlpha(0.0f, fadeTime, false);

        while (extraDisplay.canvasRenderer.GetAlpha() > 0.01f)
        {
            if (currentExtraTime != extraTime)
            {
                currentExtraTime = extraTime;
                extraDisplay.text = "+" + currentExtraTime;
                extraDisplay.CrossFadeAlpha(1.0f, 0, false);
                extraDisplay.CrossFadeAlpha(0.0f, fadeTime, false);
            }
            yield return null;
        }

        extraTime = 0;
        extraDisplay.text = null;
        extraDisplay.CrossFadeAlpha(1.0f, 0, false);
        fading = false;
    }
}
