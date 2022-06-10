using UnityEngine;

public class LeftCircle : Button
{
    public override void FirstInteraction()
    {
        interactText.text = null;
        defaultMessage = "BLANK";

        RotaryInfinite[] rotaries = GameObject.Find("Left Circle").GetComponentsInChildren<RotaryInfinite>();
        foreach (RotaryInfinite rotary in rotaries)
        {
            rotary.isRotating = false;
        }

        GameObject.Find("First Door").GetComponent<MovableGroup>().active = true;

        GameObject.Find("Big Blades").GetComponent<RotaryInfinite>().isRotating = true;

        PlayerControl playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        playerControl.SetRespawnPoint(new Vector3(0, 0, 0));
        playerControl.hits = 0;
    }
}
