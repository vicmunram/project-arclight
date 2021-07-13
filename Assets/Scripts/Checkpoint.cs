using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : Interactable
{
    public override void Interact()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerControl>().SetCheckpoint(new Vector3(transform.position.x, transform.position.y, player.transform.position.z));
    }
}
