using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LeftHit : Button
{
    public override void FirstInteraction()
    {
        Remove();
        interactText.text = null;

        DecreaseSize("Left Mirrors");
        DecreaseSize("Right Mirrors");

        PlayerControl playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        playerControl.SetRespawnPoint(new Vector3(0, 0, 1.75f));
        playerControl.SetHits(0);
    }

    private void DecreaseSize(string name)
    {
        GameObject mirrors = GameObject.Find(name);
        Transform[] transforms = mirrors.GetComponentsInChildren<Transform>();

        foreach (Transform transform in transforms)
        {
            if (transform.localScale.y != 1)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
            }
        }
    }
}
