﻿using UnityEngine;

public class RightCircle : Button
{
    public override void FirstInteraction()
    {
        interactText.text = null;
        defaultMessage = "BLANK";

        RotaryInfinite[] rotaries = GameObject.Find("Right Circle").GetComponentsInChildren<RotaryInfinite>();
        foreach (RotaryInfinite rotary in rotaries)
        {
            rotary.isRotating = false;
        }

        MovableLoop[] movables = GameObject.Find("Movable Blades").GetComponentsInChildren<MovableLoop>();
        foreach (MovableLoop movable in movables)
        {
            movable.Activate();
        }

        GameObject.Find("Second Door").GetComponent<MovableGroup>().active = true;

        PlayerControl playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        playerControl.SetRespawnPoint(new Vector3(0, 0, 0));
        playerControl.hits = 0;
    }
}
