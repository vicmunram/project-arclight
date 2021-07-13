using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sprRend;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        sprRend = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        string colTag = collision.gameObject.tag;

        if (colTag == "Player")
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            bool isDashing = playerMovement.isDashing;

            if (isDashing)
            {
                if (this.tag == "Breakable")
                {
                    StartCoroutine(DelayedDestruction());
                }
                else if (this.tag == "Breakable One Side")
                {
                    Vector2 normal = VectorUtils.RotateVector(Vector2.up, rb.rotation);
                    float rotation = Mathf.Abs(rb.rotation);
                    if (((rotation == 0 || rb.rotation == 180) && Mathf.Sign(playerMovement.movement.y * normal.y) == -1)
                    || (rotation == 90 && Mathf.Sign(playerMovement.movement.x * normal.x) == -1)
                    || ((rotation != 0 && rotation != 180 && rotation != 90) && (Mathf.Sign(playerMovement.movement.x * normal.x) == -1 || Mathf.Sign(playerMovement.movement.y * normal.y) == -1)))
                    {
                        StartCoroutine(DelayedDestruction());
                    }
                }
            }
        }
    }

    IEnumerator DelayedDestruction()
    {
        if (sprRend.size.x < bc.size.x)
        {
            bc.offset = new Vector2(0, 0);
            bc.size = new Vector2(sprRend.size.x, bc.size.y);
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.35f);
        bc.enabled = false;
        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }
}
