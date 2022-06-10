using UnityEngine;

public class MovableRestart : Movable
{
    public bool removeSprite = true;
    public override void WhenCompleted()
    {

        Vector2 auxPosition = currentPosition + direction * -1;
        rb.transform.position = new Vector3(auxPosition.x, auxPosition.y, rb.transform.position.z);

        if (AuxActive())
        {
            Collider2D collider = GetComponent<Collider2D>();
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
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = !collider.enabled;
        if (removeSprite)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.enabled = !renderer.enabled;
        }
    }

}
