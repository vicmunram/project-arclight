using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour
{
    public Text interactText;
    public string defaultMessage;
    public bool interacted = false;
    public abstract void FirstInteraction();
    public abstract void EveryInteraction();
    public abstract void OnEnter(Collider2D collision);
    public abstract void OnExit(Collider2D collision);

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControl>().OpenInteraction();
            if (interactText != null)
            {
                interactText.text = defaultMessage;
            }
        }
        OnEnter(collision);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControl>().CloseInteraction();
            if (interactText != null)
            {
                interactText.text = null;
            }
        }
        OnExit(collision);
    }

    public void Interact()
    {
        if (!interacted)
        {
            FirstInteraction();
            interacted = true;
        }

        EveryInteraction();
    }

    public void Disable()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    public void Remove()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }
}
