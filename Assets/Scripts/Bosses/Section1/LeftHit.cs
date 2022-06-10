using UnityEngine;

public class LeftHit : Button
{
    public override void FirstInteraction()
    {
        interactText.text = null;
        defaultMessage = "BLANK";

        DecreaseSize("Left Mirrors");
        DecreaseSize("Right Mirrors");

        PlayerControl playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        playerControl.SetRespawnPoint(new Vector3(0, 0, 0));
        playerControl.hits = 0;
    }

    private void DecreaseSize(string name)
    {
        GameObject mirrors = GameObject.Find(name);
        Transform[] transforms = mirrors.GetComponentsInChildren<Transform>();

        foreach (Transform transform in transforms)
        {
            if (transform.localScale.x != 1)
            {
                transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
