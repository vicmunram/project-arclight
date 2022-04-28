using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MovableRestartSwitch : Movable
{
    public bool switchActive;
    public TypeSwitchBasic typeSwitchBasic;
    public bool removeSprite = true;
    public override void WhenCompleted()
    {
        if (typeSwitchBasic != null && switchActive)
        {
            typeSwitchBasic.Switch();
        }

        Vector2 auxPosition = currentPosition + direction * -1;
        rb.transform.position = new Vector3(auxPosition.x, auxPosition.y, rb.transform.position.z);
        if (AuxActive())
        {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.enabled = !collider.enabled;
            if (removeSprite)
            {
                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                renderer.enabled = !renderer.enabled;
            }
        }
    }

    public override void WhenAuxCompleted()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.enabled = !collider.enabled;
        if (removeSprite)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.enabled = !renderer.enabled;
        }
    }

}
