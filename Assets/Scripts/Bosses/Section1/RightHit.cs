using UnityEngine;

public class RightHit : Button
{
    public override void FirstInteraction()
    {
        interactText.text = null;
        defaultMessage = "BLANK";

        PlayerControl playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        playerControl.SetRespawnPoint(new Vector3(0, 0, 0));
        playerControl.hits = 0;

        GameObject arrows = GameObject.Find("Arrows");
        MovableRestartSwitch[] movables = arrows.GetComponentsInChildren<MovableRestartSwitch>();
        foreach (MovableRestartSwitch movable in movables)
        {
            movable.switchActive = true;
        }
    }
}
