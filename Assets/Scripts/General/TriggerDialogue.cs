using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerDialogue : MonoBehaviour
{
    public string dialogueName;
    private Dialogue dialogue;

    void Start()
    {
        dialogue = gameObject.AddComponent<Dialogue>();
        dialogue.dialogueName = dialogueName;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogue.Activate();
        }
    }
}
