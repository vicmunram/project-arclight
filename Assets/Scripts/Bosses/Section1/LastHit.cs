using UnityEngine;

public class LastHit : Button
{
    public override void FirstInteraction()
    {
        interactText.text = null;
        defaultMessage = "BLANK";

        StopMovables("Arrows");
        StopMovables("Dangers");
        StopMovables("Crystals");

        ExtraTime[] extras = GameObject.Find("Extras").GetComponentsInChildren<ExtraTime>();
        foreach (ExtraTime extra in extras)
        {
            Destroy(extra.gameObject);
        }

        GameProgress.SaveProgress(Timer.globalTime, 0, true);
        GameObject.Find("Player").GetComponent<PlayerControl>().SetRespawnPoint(new Vector3(0, 0, 0));
        AudioUtils.StopMusic();
    }

    private void StopMovables(string name)
    {
        Movable[] movables = GameObject.Find(name).GetComponentsInChildren<Movable>();
        foreach (Movable movable in movables)
        {
            movable.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            movable.Deactivate();
        }
    }
}
