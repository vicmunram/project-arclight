﻿using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private bool canInteract;
    private Vector3 checkpoint;

    private Vector2 boxSize = new Vector2(0.1f, 0.1f);

    void Start()
    {
        this.checkpoint = this.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.CheckInteraction();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            this.transform.position = checkpoint;
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

    public void SetCheckpoint(Vector3 position)
    {
        checkpoint = position;
    }

}