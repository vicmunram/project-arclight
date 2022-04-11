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

        GameObject.Find("Player").GetComponent<PlayerControl>().SaveProgress(new Vector2(0, 1.75f));
    }

    private void StopMovables(string name)
    {
        Movable[] movables = GameObject.Find(name).GetComponentsInChildren<Movable>();
        foreach (Movable movable in movables)
        {
            movable.rb.velocity = new Vector2(0, 0);
            movable.isMoving = false;
        }
    }
}
