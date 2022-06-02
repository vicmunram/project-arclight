using UnityEngine;

public class LastCircle : Button
{
    public override void FirstInteraction()
    {
        interactText.text = null;
        defaultMessage = "BLANK";

        ExtraTime[] extras = GameObject.Find("Extras").GetComponentsInChildren<ExtraTime>();
        foreach (ExtraTime extra in extras)
        {
            Destroy(extra.gameObject);
        }

        GameObject blades = GameObject.Find("Movable Blades");

        MovableLoop[] movables = blades.GetComponentsInChildren<MovableLoop>();
        foreach (MovableLoop movable in movables)
        {
            movable.Deactivate();
        }
        RotaryInfinite[] rotaries = blades.GetComponentsInChildren<RotaryInfinite>();
        foreach (RotaryInfinite rotary in rotaries)
        {
            rotary.isRotating = false;
        }


        GameObject.Find("Exit").GetComponent<MovableGroup>().active = true;

        GameProgress.SaveProgress(Timer.globalTime, 0, true);
        GameObject.Find("Player").GetComponent<PlayerControl>().SetRespawnPoint(gameObject.transform.position);
        AudioUtils.StopMusic();
    }

}
