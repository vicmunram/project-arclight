using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : Interactable
{
    public override void Interact()
    {
        GameObject.Find("Player").GetComponent<PlayerControl>().SetCheckpoint(this.transform.position);
    }
}
