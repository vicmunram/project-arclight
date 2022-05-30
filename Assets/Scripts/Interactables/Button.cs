using UnityEngine;
using System.Collections;

public class Button : Interactable
{
    public float timer = 0;
    public bool once;
    private int timeRemaining = 0;
    private bool waiting;
    public GameObject[] interactions;
    void Start()
    {
        if (interactText != null && timer != 0)
        {
            interactText.text = timer.ToString();
        }
    }
    public override void FirstInteraction() { }
    public override void EveryInteraction()
    {
        ActivateInteractions();
        if (timer != 0 && !waiting)
        {
            waiting = true;
            StartCoroutine(WaitTimer(timer));
        }
        if (once)
        {
            Disable();
        }
    }

    private void ActivateInteractions()
    {
        if (!waiting)
        {
            foreach (GameObject interaction in interactions)
            {
                if (interaction.tag == Tags.movableGroup)
                {
                    interaction.GetComponent<MovableGroup>().active = true;
                }
                else if (interaction.tag == Tags.rotaryGroup)
                {
                    interaction.GetComponent<RotaryGroup>().Rotate();
                }
                else if (interaction.tag == Tags.typeSwitchGroup)
                {
                    interaction.GetComponent<TypeSwitchGroup>().Switch();
                }
            }
        }
    }

    public override void OnEnter(Collider2D collision)
    {
        if (timer != 0)
        {
            if (!waiting && timeRemaining == 0)
            {
                timeRemaining = (int)timer;
            }
            interactText.text = timeRemaining.ToString();
        }
    }
    public override void OnExit(Collider2D collision)
    {
        if (timer != 0)
        {
            interactText.text = timeRemaining.ToString();
        }
    }

    IEnumerator WaitTimer(float timer)
    {
        timeRemaining = (int)timer - 1;
        while (timeRemaining > 0)
        {
            interactText.text = timeRemaining.ToString();
            yield return new WaitForSeconds(1);
            timeRemaining--;

        }
        interactText.text = timeRemaining.ToString();
        waiting = false;
        ActivateInteractions();
        waiting = true;

        yield return new WaitForSeconds(1);
        timeRemaining = (int)timer;
        interactText.text = timeRemaining.ToString();
        waiting = false;
    }

}
