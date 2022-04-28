using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LastHit : Button
{
    public override void FirstInteraction()
    {
        Remove();
        interactText.text = null;

        StopMovables("Arrows");
        StopMovables("Left Mirrors");
        StopMovables("Right Mirrors");
        StopMovables("Dangers");

        ExtraTime[] extras = GameObject.Find("Extras").GetComponentsInChildren<ExtraTime>();
        foreach (ExtraTime extra in extras)
        {
            Destroy(extra.gameObject);
        }

        GameProgress.SaveProgress(Timer.globalTime, 0, true);
        GameObject.Find("Player").GetComponent<PlayerControl>().SetRespawnPoint(new Vector3(0, 1.75f, 0));
    }

    private void StopMovables(string name)
    {
        Movable[] movables = GameObject.Find(name).GetComponentsInChildren<Movable>();
        foreach (Movable movable in movables)
        {
            movable.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            movable.isMoving = false;
        }
    }
}
