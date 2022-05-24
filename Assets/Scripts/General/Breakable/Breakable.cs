using System.Collections;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public float recoveryTime;
    public bool recovering;
    public bool positioned;
    private Vector2 originalPosition;
    private Quaternion originalAngle;
    private Vector2 originalOffset;
    private Vector2 originalSize;
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    void Start()
    {
        positioned = true;

        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        originalAngle = transform.rotation;

        bc = GetComponent<BoxCollider2D>();
        originalOffset = bc.offset;
        originalSize = bc.size;
    }

    void FixedUpdate()
    {
        if (recovering)
        {
            if (rb.angularVelocity != 0)
            {
                rb.MoveRotation(originalAngle);
                rb.angularVelocity = 0f;
            }

            rb.velocity = (originalPosition - (Vector2)transform.position) * 5;

            if (VectorUtils.Equals(transform.position, originalPosition, 1000) || VectorUtils.Equals(rb.velocity, new Vector2(0, 0)))
            {
                Restart();
                positioned = true;
                recovering = false;
            }
        }
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
                if (this.tag == Tags.breakable)
                {
                    StartCoroutine(DelayedDestruction());
                }
                else if (this.tag == Tags.breakableOneSide)
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

    public void Restart()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = new Vector2(0, 0);

        bc.offset = originalOffset;
        bc.size = originalSize;
    }

    IEnumerator DelayedDestruction()
    {
        positioned = false;
        SpriteRenderer sprRend = GetComponent<SpriteRenderer>();
        if (sprRend.size.x < bc.size.x || sprRend.size.y < bc.size.y)
        {
            bc.offset = new Vector2(0, 0);
            bc.size = new Vector2(sprRend.size.x, sprRend.size.y);
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.1f);
        if (rb.velocity.x == 0f && rb.velocity.y == 0f)
        {
            rb.velocity = new Vector2(Random.Range(-4, 4), Random.Range(-4, 4));
        }
        bc.enabled = false;
        yield return new WaitForSeconds(recoveryTime);
        recovering = true;
    }
}
