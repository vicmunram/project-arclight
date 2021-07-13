using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Speed control
    public float maxSpeed = 10f;
    public float speed = 0f;
    public float acc = 0.05f;
    public float defaultAccInc = 0.001f;
    public float accInc;

    // Movement and collisions
    public Rigidbody2D rb;
    public CircleCollider2D cc;
    Vector2 input;
    public Vector2 movement = new Vector2(0, 0);
    private Collision2D col;
    public bool isRepelled;
    private List<string> alwaysSolid = new List<string> { "Solid", "Breakable", "Breakable One Side", "Mirror" };
    private List<string> solidWhenNoPunch = new List<string> { "Solid", "Breakable", "Breakable One Side", "Mirror", "Transparent" };
    private List<string> notBreakable = new List<string> { "Solid", "Mirror", "Transparent", "Danger" };
    private List<string> triggeredWhenPunch = new List<string> { "Checkpoint", "Abysm", "Transparent" };

    // Dash
    public float dashRate = 0.75f;
    private float nextDash;
    public bool isDashing;

    // Punch
    public float punchSpeed = 30f;
    public float punchCharge = 0.25f;
    public float punchPreparingTime = 2;
    public bool isPunching;
    public bool isPreparing;
    private Collider2D enemyHit;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        accInc = defaultAccInc;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isPunching && !isDashing)
        {
            float startTime = Time.time;
            StartCoroutine(Punch(startTime));
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time > nextDash && !isPreparing && !isPunching)
        {
            nextDash = Time.time + dashRate;
            StartCoroutine(Dash());
        }
        else if (!isPunching && !isDashing && !isRepelled)
        {
            GetPlayerInput();
            SpeedControl();
        }
    }

    void FixedUpdate()
    {
        if (isPunching)
        {
            CheckPunchCollisions();
        }
        else if (isDashing)
        {
            CheckDashCollisions();
        }
        else
        {
            CheckCollisions();
        }

        if (!isDashing && !isRepelled)
        {
            MovePlayer();
            if (!isPunching)
            {
                RotationControl();
            }
        }
    }

    // Input and basic movement
    private void GetPlayerInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        if (input.x != 0 || input.y != 0)
        {
            movement = input;
        }
    }

    private void MovePlayer()
    {
        if (movement.x != 0 && movement.y != 0)
        {
            rb.velocity = movement.normalized * speed;
        }
        else
        {
            rb.velocity = movement * speed;
        }
    }

    private void SpeedControl()
    {
        if (isPreparing)
        {
            speed = speed >= 1 ? 1 : speed;
        }
        else
        {
            if (input.x != 0 || input.y != 0)
            {
                if (speed <= maxSpeed)
                {
                    accInc = accInc + defaultAccInc;
                    speed = speed + acc * Mathf.Pow(1.001f, accInc);
                    if (speed >= maxSpeed)
                    {
                        speed = maxSpeed;
                        accInc = defaultAccInc;
                    }
                }
            }
            else
            {
                if (speed != 0)
                {
                    accInc = defaultAccInc;
                    speed = speed / 2;
                    if (speed < 0.1)
                    {
                        speed = 0;
                    }
                }
            }
        }
    }

    private void RotationControl()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 lookingAt = Input.mousePosition - pos;
        float angle = Mathf.Atan2(lookingAt.y, lookingAt.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    // Collisions
    void OnCollisionEnter2D(Collision2D collision)
    {
        col = collision;
        string colTag = collision.gameObject.tag;
        PlayerControl playerControl = GetComponent<PlayerControl>();

        if (solidWhenNoPunch.Contains(colTag))
        {
            speed = 1;
            ResetPunch();
        }
        if ((colTag == "Danger" && !isPunching && playerControl.hits >= 1) || colTag == "Enemy")
        {
            ReceiveHit(false);
            ResetPunch();
        }
    }


    private void CheckCollisions()
    {
        RaycastHit2D[] outerHits = CollisionUtils.CastOuterHits(20, transform, cc, "Is Trigger", false);
        RaycastHit2D[] innerHits = CollisionUtils.CastInnerHits(20, transform, cc, "Is Trigger", false);

        int abysmOuterCollisions = CollisionUtils.CountCollisions(outerHits, "Abysm");
        int abysmInnerCollisions = CollisionUtils.CountCollisions(innerHits, "Abysm");
        if (abysmOuterCollisions == 20 || (abysmOuterCollisions > 11 && abysmInnerCollisions > 15))
        {
            this.GetComponent<PlayerControl>().SetHits(0);
        }

        outerHits = CollisionUtils.CastOuterHits(20, transform, cc, "Default", false);
        RaycastHit2D firstOuterHit = CollisionUtils.CloserHit(outerHits, transform, alwaysSolid);
        innerHits = CollisionUtils.CastInnerHits(20, transform, cc, "Transparent", false);
        RaycastHit2D firstInnerHit = CollisionUtils.FirstHit(innerHits, "Transparent");

        if (firstOuterHit.collider && firstInnerHit.collider)
        {
            float positionOuter = CollisionUtils.RelativePositionToCollider(transform, firstOuterHit.collider);
            Vector2 directionOuter = VectorUtils.RotateVector(Vector2.up, firstOuterHit.collider.attachedRigidbody.rotation).normalized * positionOuter;
            float positionInner = CollisionUtils.RelativePositionToCollider(transform, firstInnerHit.collider);
            Vector2 directionInner = VectorUtils.RotateVector(Vector2.up, firstInnerHit.collider.attachedRigidbody.rotation).normalized * positionInner;
            Vector2 direction = (directionOuter + directionInner).normalized;

            movement = direction;
            StartCoroutine(DebugTransparent());
        }
    }

    private void CheckPunchCollisions()
    {
        // Basic collisions
        RaycastHit2D[] hitsFront = CollisionUtils.CastHitsMovementFront(25, movement, transform, 0.2f, cc, false);
        RaycastHit2D closerHitFront = CollisionUtils.CloserHit(hitsFront, transform, "Any");
        RaycastHit2D[] hitsBack = CollisionUtils.CastHitsMovementBack(19, movement, transform, cc, false, false);
        RaycastHit2D closerHitBack = CollisionUtils.CloserHit(hitsBack, transform, "Any");

        // Advanced collisions
        RaycastHit2D[] hitsFrontExt = CollisionUtils.CastHitsMovementFrontExt(25, movement, transform, cc, false);
        RaycastHit2D closerHitFrontExt = CollisionUtils.CloserHit(hitsFrontExt, transform, alwaysSolid);
        RaycastHit2D closerHitFrontExtDanger = CollisionUtils.CloserHit(hitsFrontExt, transform, "Danger");

        if (closerHitFrontExt.collider)
        {
            closerHitFrontExtDanger = CollisionUtils.CloserHitToHit(hitsFrontExt, closerHitFrontExt, "Danger");
            float distance = Vector2.SqrMagnitude(closerHitFrontExt.point - closerHitFrontExtDanger.point);
            if (closerHitFrontExtDanger.collider && distance < 0.05f)
            {
                cc.enabled = true;
                ReceiveHit(true);
                ResetPunch();
            }
        }
        else if (closerHitFrontExtDanger.collider)
        {
            cc.enabled = true;
            ReceiveHit(true);
            ResetPunch();
        }

        if (closerHitFront.collider && cc.enabled == false)
        {
            string colTag = closerHitFront.collider.tag;
            if (colTag == "Mirror")
            {
                Reflect(closerHitFront.normal);
            }
            else if (colTag == "Enemy")
            {
                enemyHit = closerHitFront.collider;
            }
            else if (!triggeredWhenPunch.Contains(colTag))
            {
                cc.enabled = true;
            }
        }

        if (closerHitBack.collider && cc.enabled == false)
        {
            string colTag = closerHitBack.collider.tag;
            if (closerHitBack.collider.Equals(enemyHit))
            {
                Destroy(enemyHit.gameObject);
                cc.enabled = true;
                isPunching = false;
                speed = maxSpeed;
            }
        }
    }

    private void CheckPunchCollisionsNear()
    {
        RaycastHit2D[] hitsFront = CollisionUtils.CastArc(5, 30, transform.position, 0, movement, cc.radius / 4 + 0.25f, LayerMask.GetMask("Default"), false);
        RaycastHit2D closerHitFront = CollisionUtils.CloserHit(hitsFront, transform, "Any");

        if (closerHitFront.collider)
        {
            string colTag = closerHitFront.collider.tag;
            if (colTag == "Mirror")
            {
                Reflect(closerHitFront.normal);
            }
            else if (!triggeredWhenPunch.Contains(colTag))
            {
                cc.enabled = true;
            }
        }
    }

    private void CheckDashCollisions()
    {
        RaycastHit2D[] hits = CollisionUtils.CastHitsMovementFront(25, movement, transform, 0.4f, cc, false);
        RaycastHit2D closerHitDashing = CollisionUtils.CloserHit(hits, transform, "Any");

        if (closerHitDashing.collider)
        {
            string colTag = closerHitDashing.collider.tag;
            if (notBreakable.Contains(colTag))
            {
                speed = 1f;
            }
        }
    }

    // Auxiliar methods
    private void Reflect(Vector2 normal)
    {
        movement = Vector2.Reflect(movement, normal);
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    private void ReceiveHit(bool kill)
    {
        PlayerControl playerControl = this.GetComponent<PlayerControl>();
        int currentHits = kill ? 0 : playerControl.GetHits() - 1;
        playerControl.SetHits(currentHits);
        if (currentHits != 0)
        {
            StartCoroutine(Repel(col.GetContact(0).normal));
        }
        else
        {
            speed = 0;
        }
    }

    private void ResetPunch()
    {
        if (isPunching)
        {
            isPunching = false;
        }
    }

    // Coroutines
    IEnumerator Punch(float startTime)
    {
        while (Input.GetKey(KeyCode.Mouse0) && Time.time - startTime <= punchPreparingTime)
        {
            isPreparing = true;
            yield return null;
        }
        isPreparing = false;
        if (Time.time - startTime >= punchCharge && Time.time - startTime < punchPreparingTime)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = Input.mousePosition - pos;
            movement = new Vector2(dir.x, dir.y).normalized;
            speed = punchSpeed;
            cc.enabled = false;
            CheckPunchCollisionsNear();
            rb.velocity = movement * speed;
            isPunching = true;
        }
    }

    IEnumerator Repel(Vector2 normal)
    {
        isRepelled = true;
        speed = 6.5f;
        Reflect(normal);
        rb.velocity = movement * speed;
        yield return new WaitForSeconds(0.25f);
        speed = 0.25f;
        isRepelled = false;
    }

    IEnumerator Dash()
    {
        float oldSpeed = speed;
        isDashing = true;
        CheckDashCollisions();
        rb.velocity = movement * speed * 2.5f;
        yield return new WaitForSeconds(0.05f);
        speed = oldSpeed;
        isDashing = false;
    }

    IEnumerator DebugTransparent()
    {
        cc.enabled = false;
        isDashing = true;
        rb.velocity = movement * 10;
        yield return new WaitForSeconds(0.005f);
        isDashing = false;
        cc.enabled = true;
    }
}
