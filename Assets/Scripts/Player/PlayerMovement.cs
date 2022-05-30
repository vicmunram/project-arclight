using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Status
    public bool canMove = true;
    public bool canPunch;
    public bool canDash;

    // Speed control
    public float maxSpeed = 8f;
    public float speed = 0f;
    public float acc = 0.05f;
    public float defaultAccInc = 0.001f;
    private float accInc;
    private bool diagonally;
    private bool wasDiagonally;
    private bool wait;

    // Movement and collisions
    public bool isRepelled;
    public Rigidbody2D rb;
    public CircleCollider2D cc;
    private Vector2 input;
    public Vector2 movement = new Vector2(0, 0);
    private Collision2D col;

    // Dash
    public bool isDashing;
    public float dashRate = 0.75f;
    private float nextDash;

    // Punch
    public bool isPreparing;
    public bool isPunching;
    public bool wasPunching;
    public float punchSpeed = 30f;
    public float punchCharge = 0.25f;
    public float punchPreparingTime = 2;
    private Collider2D anchorHit;
    private Collider2D lastAnchorHit;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        accInc = defaultAccInc;

        SetPlayerState();
    }

    void Update()
    {
        if (canMove)
        {
            if (canPunch && Input.GetKeyDown(KeyCode.Mouse0) && !isPunching && !isDashing)
            {
                float startTime = Time.time;
                StartCoroutine(Punch(startTime));
            }
            else if (canDash && Input.GetKeyDown(KeyCode.Mouse1) && Time.time > nextDash && !isPreparing && !isPunching && !isRepelled)
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
    }

    void FixedUpdate()
    {
        if (canMove)
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
                if (wasPunching)
                {
                    wasPunching = false;
                }
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
    }

    // Input and basic movement
    private void GetPlayerInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        if (diagonally)
        {
            bool notDiagonally = (input.x == 0 && input.y != 0) || (input.x != 0 && input.y == 0);
            if (wait)
            {
                if (notDiagonally)
                {
                    diagonally = false;
                    wait = false;
                }
                else if (input.x != 0 && input.y != 0)
                {
                    wait = false;
                }

            }
            else if (notDiagonally)
            {
                wait = true;
            }
        }

        if (!wait)
        {
            movement = input;
            diagonally = input.SqrMagnitude() > 1;
        }
    }

    private void MovePlayer()
    {
        if (diagonally)
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

    public void Stop()
    {
        rb.velocity = new Vector2(0, 0);
        canMove = false;
    }

    // Collisions
    void OnCollisionEnter2D(Collision2D collision)
    {
        col = collision;
        string colTag = collision.gameObject.tag;
        int colLayer = collision.gameObject.layer;
        PlayerControl playerControl = GetComponent<PlayerControl>();

        if (colLayer == LayerMask.NameToLayer(Layers.solid)
        || colLayer == LayerMask.NameToLayer(Layers.transparent)
        || colLayer == LayerMask.NameToLayer(Layers.punchThrough))
        {
            if (colTag == Tags.danger && !wasPunching && playerControl.hits >= 1)
            {
                ReceiveHit(false);
                wasPunching = false;
            }
            else
            {
                speed = 1;
                if (isPunching)
                {
                    isPunching = false;
                    wasPunching = true;
                }
            }
        }
    }

    private void CheckCollisions()
    {
        // Danger interactions
        RaycastHit2D[] outerHits = CollisionUtils.CastOuterHits(20, transform, cc, Layers.triggered, false);
        RaycastHit2D[] innerHits = CollisionUtils.CastInnerHits(20, transform, cc, Layers.triggered, false);

        RaycastHit2D closerDangerOuterHit = CollisionUtils.CloserHit(outerHits, transform, movement, Tags.danger);
        int dangerOuterCollisions = CollisionUtils.CountCollisions(outerHits, Tags.danger);
        int dangerInnerCollisions = CollisionUtils.CountCollisions(innerHits, Tags.danger);

        if (!isRepelled)
        {
            Vector2 normal = closerDangerOuterHit.normal;
            if (dangerInnerCollisions > 11)
            {
                ReceiveHit(true, normal);
            }
            else if (closerDangerOuterHit.collider)
            {
                if ((Mathf.Sign(movement.x * normal.x) == -1 || Mathf.Sign(movement.y * normal.y) == -1)
                || (Mathf.Sign(movement.x * normal.x) != -1 && Mathf.Sign(movement.y * normal.y) != -1 && dangerOuterCollisions > 7))
                {
                    ReceiveHit(false, normal);
                }
            }
        }

        // Overlapped interactions
        int abysmOuterCollisions = CollisionUtils.CountCollisions(outerHits, Tags.abysm);
        int abysmInnerCollisions = CollisionUtils.CountCollisions(innerHits, Tags.abysm);

        RaycastHit2D[] solidHits = CollisionUtils.CastOuterHits(20, transform, cc, Layers.solid, false);
        int solidCollisions = CollisionUtils.CountCollisions(solidHits, Tags.solid);
        int mirrorCollisions = CollisionUtils.CountCollisions(solidHits, Tags.solid);

        if (abysmOuterCollisions == 20 || (abysmOuterCollisions > 11 && abysmInnerCollisions > 15) || solidCollisions == 20 || mirrorCollisions == 20)
        {
            this.GetComponent<PlayerControl>().SetHits(0);
        }

        // Transparent interactions
        outerHits = CollisionUtils.CastOuterHits(20, transform, cc, Layers.solid, false);
        RaycastHit2D firstOuterHit = CollisionUtils.CloserHit(outerHits, transform, Tags.alwaysSolid);
        innerHits = CollisionUtils.CastInnerHits(20, transform, cc, Layers.transparent, false);
        RaycastHit2D firstInnerHit = CollisionUtils.FirstHit(innerHits, Tags.any);

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
        RaycastHit2D[] frontHits = CollisionUtils.CastHitsMovementFront(25, movement, transform, 0.2f, cc, false);
        RaycastHit2D closestFrontHit = CollisionUtils.CloserHit(frontHits, transform, Tags.any);
        RaycastHit2D[] backHits = CollisionUtils.CastHitsMovementBack(19, movement, transform, cc, false, false);
        RaycastHit2D closestBackHit = CollisionUtils.CloserHit(backHits, transform, Tags.any);

        // Advanced collisions
        RaycastHit2D[] extFrontHits = CollisionUtils.CastHitsMovementFrontExt(25, movement, transform, cc, false);
        RaycastHit2D closestExtFrontHit = CollisionUtils.CloserHit(extFrontHits, transform, Tags.alwaysSolid);
        RaycastHit2D closestExtFrontHitDanger = CollisionUtils.CloserHit(extFrontHits, transform, Tags.danger);

        if (closestExtFrontHit.collider)
        {
            closestExtFrontHitDanger = CollisionUtils.CloserHitToHit(extFrontHits, closestExtFrontHit, Tags.danger);
            float distance = Vector2.SqrMagnitude(closestExtFrontHit.point - closestExtFrontHitDanger.point);
            if (closestExtFrontHitDanger.collider && distance < 0.05f)
            {
                cc.enabled = true;
                ReceiveHit(true);
                ResetPunch();
            }
        }
        else if (closestExtFrontHitDanger.collider)
        {
            cc.enabled = true;
            ReceiveHit(true);
            ResetPunch();
        }

        if (closestFrontHit.collider && cc.enabled == false)
        {
            string colTag = closestFrontHit.collider.tag;
            int colLayer = closestFrontHit.collider.gameObject.layer;

            if (colLayer == LayerMask.NameToLayer(Layers.solid))
            {
                if (colTag == Tags.mirror)
                {
                    Reflect(closestFrontHit.normal);
                }
                else
                {
                    cc.enabled = true;
                }
            }
            else if (colLayer == LayerMask.NameToLayer(Layers.punchThrough))
            {
                anchorHit = closestFrontHit.collider;
            }
            else if (colLayer == LayerMask.NameToLayer(Layers.invisible))
            {
                if (colTag == Tags.sceneChanger)
                {
                    closestFrontHit.collider.GetComponentInParent<Changer>().RequestChange();
                }
            }
        }

        if (closestBackHit.collider && cc.enabled == false && anchorHit != null)
        {
            EndPunchThrough(closestBackHit.collider);
        }

        // Debug collisions
        RaycastHit2D[] outerHits = CollisionUtils.CastOuterHits(20, transform, cc, Layers.solid, true);
        int solidCollisions = CollisionUtils.CountCollisions(outerHits, Tags.solid);
        int mirrorCollisions = CollisionUtils.CountCollisions(outerHits, Tags.mirror);
        if (cc.enabled == false && (mirrorCollisions == 20 || solidCollisions == 20))
        {
            cc.enabled = true;
            ReceiveHit(true);
            ResetPunch();
        }
    }

    private void CheckPunchCollisionsNear()
    {
        RaycastHit2D[] hitsFront = CollisionUtils.CastArc(5, 30, transform.position, 0, movement, cc.radius / 4 + 0.25f, LayerMask.GetMask("Default"), false);
        RaycastHit2D closerHitFront = CollisionUtils.CloserHit(hitsFront, transform, Tags.any);

        if (closerHitFront.collider)
        {
            string colTag = closerHitFront.collider.tag;
            int colLayer = closerHitFront.collider.gameObject.layer;

            if (colLayer == LayerMask.NameToLayer(Layers.solid))
            {
                if (colTag == Tags.mirror)
                {
                    Reflect(closerHitFront.normal);
                }
                else
                {
                    cc.enabled = true;
                }
            }
        }
    }

    private void CheckDashCollisions()
    {
        RaycastHit2D[] hits = CollisionUtils.CastHitsMovementFront(25, movement, transform, 0.4f, cc, false);
        RaycastHit2D closerHitDashing = CollisionUtils.CloserHit(hits, transform, Tags.any);

        if (closerHitDashing.collider)
        {
            string colTag = closerHitDashing.collider.tag;
            if (!colTag.Contains(Tags.breakable))
            {
                speed = 1f;
                rb.velocity = movement * speed;
            }
        }
    }

    public void SetPlayerState()
    {
        if (canPunch && canDash)
        {
            maxSpeed = 7.5f;
            GetComponent<PlayerControl>().maxHits = 3;
            GetComponent<PlayerControl>().hits = 3;
        }
        else if (canPunch || canDash)
        {
            maxSpeed = 5f;
            GetComponent<PlayerControl>().maxHits = 2;
            GetComponent<PlayerControl>().hits = 2;
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Player2");
        }
        else
        {
            maxSpeed = 2.5f;
            GetComponent<PlayerControl>().maxHits = 1;
            GetComponent<PlayerControl>().hits = 1;
        }
    }

    // Auxiliar methods
    private void Reflect(Vector2 normal)
    {
        movement = Vector2.Reflect(movement, normal);
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        AudioUtils.PlayEffect("reflect", 0.5f);
    }

    public void ReceiveHit(bool kill)
    {
        PlayerControl playerControl = this.GetComponent<PlayerControl>();
        int currentHits = kill ? 0 : playerControl.hits - 1;
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

    private void ReceiveHit(bool kill, Vector2 normal)
    {
        PlayerControl playerControl = this.GetComponent<PlayerControl>();
        int currentHits = kill ? 0 : playerControl.hits - 1;
        playerControl.SetHits(currentHits);
        if (currentHits != 0)
        {
            StartCoroutine(Repel(normal));
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

    private void EndPunchThrough(Collider2D backHit)
    {
        string colTag = backHit.tag;
        if (!colTag.Equals(Tags.interactable))
        {
            if (backHit.Equals(anchorHit))
            {
                if (colTag.Equals(Tags.breakable))
                {
                    anchorHit.gameObject.GetComponent<DestroyablePT>().Destroy();
                }
                cc.enabled = true;
                isPunching = false;
                speed = maxSpeed;
            }
        }
        else
        {
            anchorHit.GetComponent<ExtraTime>().Interact();
        }

        anchorHit = null;
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
        if (Mathf.Sign(movement.x * normal.x) == -1 || Mathf.Sign(movement.y * normal.y) == -1)
        {
            Reflect(normal);
        }
        else
        {
            movement = movement + normal;
        }
        speed = speed == 0 ? maxSpeed : speed;
        rb.velocity = movement * speed;
        yield return new WaitForSeconds(0.25f);
        if (!isPunching)
        {
            speed = 0.25f;
        }
        isRepelled = false;
    }

    IEnumerator Dash()
    {
        float oldSpeed = speed;
        isDashing = true;
        CheckDashCollisions();
        rb.velocity = movement * speed * 4f;
        yield return new WaitForSeconds(0.05f);
        speed = oldSpeed;
        yield return new WaitForSeconds(0.05f);
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
