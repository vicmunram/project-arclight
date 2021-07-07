﻿using System.Collections;
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
    private bool isRepelled;

    // Dash
    public float dashRate = 0.75f;
    private float nextDash;
    private bool isDashing;

    // Punch
    public float punchSpeed = 30f;
    public float punchCharge = 0.25f;
    public float punchPreparingTime = 2;
    private bool isPunching;
    private bool isPreparing;
    private bool isTrapped;
    private Collider2D enemyHit;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        accInc = defaultAccInc;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isPunching)
        {
            float startTime = Time.time;
            StartCoroutine(Punch(startTime));
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time > nextDash && !isPreparing)
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

        if (colTag == "Solid" || colTag == "Breakable" || (colTag == "Mirror" && !isPunching) || colTag == "Transparent")
        {
            speed = 1;
            ResetPunch();
        }
        if (colTag == "Danger" || colTag == "Enemy")
        {
            ReceiveHit();
            ResetPunch();
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        string colTag = collision.gameObject.tag;
        foreach (ContactPoint2D hitpos in collision.contacts)
        {
            if (Mathf.Sign(movement.x * hitpos.normal.x) == -1)
            {
                movement.x = 0;
            }
            if (Mathf.Sign(movement.y * hitpos.normal.y) == -1)
            {
                movement.y = 0;
            }
        }
    }

    private void CheckCollisions()
    {
        RaycastHit2D[] outerHits = RaycastUtils.CastOuterHits(20, transform, cc, "Is Trigger", false);
        RaycastHit2D[] innerHits = RaycastUtils.CastInnerHits(20, transform, cc, "Is Trigger", false);

        int abysmOuterCollisions = RaycastUtils.CountCollisionsByTag(outerHits, "Abysm");
        int abysmInnerCollisions = RaycastUtils.CountCollisionsByTag(innerHits, "Abysm");
        if (abysmOuterCollisions == 20 || (abysmOuterCollisions > 11 && abysmInnerCollisions > 15))
        {
            this.GetComponent<PlayerControl>().SetHits(0);
        }
    }

    private void CheckPunchCollisions()
    {
        RaycastHit2D[] hitsFront = RaycastUtils.CastHitsMovement(21, movement, transform, cc, true, false);
        RaycastHit2D closerHitFront = RaycastUtils.GetCloserHit(hitsFront, transform);
        RaycastHit2D[] hitsBack = RaycastUtils.CastHitsMovement(19, movement, transform, cc, false, false);
        RaycastHit2D closerHitBack = RaycastUtils.GetCloserHit(hitsBack, transform);

        if (closerHitFront.collider)
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
            else if (colTag != "Checkpoint" && colTag != "Transparent" && colTag != "Abysm")
            {
                cc.enabled = true;
            }
        }

        if (closerHitBack.collider)
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

    private void CheckPunchCollisionsNearObjects()
    {
        RaycastHit2D[] hitsFront = RaycastUtils.CastHitsMovementNearObjects(5, movement, transform, cc, false);
        RaycastHit2D closerHitFront = RaycastUtils.GetCloserHit(hitsFront, transform);

        if (closerHitFront.collider)
        {
            string colTag = closerHitFront.collider.tag;
            if (colTag == "Mirror")
            {
                Reflect(closerHitFront.normal);
            }
            else if (colTag != "Checkpoint" && colTag != "Transparent" && colTag != "Abysm" && colTag != "Enemy")
            {
                cc.enabled = true;
            }
        }
    }

    private void CheckDashCollisions()
    {
        RaycastHit2D[] hits = RaycastUtils.CastHitsMovement(11, movement, transform, cc, true, false);
        RaycastHit2D closerHitDashing = RaycastUtils.GetCloserHit(hits, transform);

        if (closerHitDashing.collider)
        {
            string colTag = closerHitDashing.collider.tag;
            if (colTag == "Breakable")
            {
                Destroy(closerHitDashing.collider.gameObject);
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

    private void ReceiveHit()
    {
        PlayerControl playerControl = this.GetComponent<PlayerControl>();
        int currentHits = isPunching ? 0 : playerControl.GetHits() - 1;
        playerControl.SetHits(currentHits);
        if (currentHits != 0)
        {
            StartCoroutine(Repel(col.GetContact(0).normal));
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
            CheckPunchCollisionsNearObjects();
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
        rb.velocity = movement * speed * 3;
        yield return new WaitForSeconds(0.05f);
        speed = oldSpeed;
        isDashing = false;
    }
}
