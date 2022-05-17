using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RightHit : Button
{
    public override void FirstInteraction()
    {
        Remove();
        interactText.text = null;

        PlayerControl playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        playerControl.SetRespawnPoint(new Vector3(0, 0, 1.75f));
        playerControl.hits = 0;

        GameObject arrows = GameObject.Find("Arrows");
        MovableRestartSwitch[] movables = arrows.GetComponentsInChildren<MovableRestartSwitch>();
        foreach (MovableRestartSwitch movable in movables)
        {
            movable.switchActive = true;
        }
    }
}
