using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float speed = 0f;
    public float acc = 0.05f;
    public float accInc = 0.001f;

    public Rigidbody2D rb;
    public CircleCollider2D cc;
    Vector2 input;
    public Vector2 movement = new Vector2(0, 0);

    public float dashRate = 0.1f;
    private float nextDash;
    private bool isDashing;
    public float punchSpeed = 20f;
    public float punchCharge = 0.25f;
    public float punchPreparingTime = 2;
    private bool isPunching;
    private bool isPreparing;
    private bool isRepelled;
    private Collision2D col;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time > nextDash)
        {
            nextDash = Time.time + dashRate;
            StartCoroutine(Dash());
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && !isPunching)
        {
            float startTime = Time.time;
            StartCoroutine(Punch(startTime));
        }
        else if (!isPunching && !isDashing && !isRepelled)
        {
            GetPlayerInput();
            SpeedControl();
        }
    }

    void FixedUpdate()
    {
        if (!isPunching && !isDashing && !isRepelled)
        {
            MovePlayer();
            RotationControl();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            speed = 1;
        }
        if (collision.gameObject.tag == "SpikeWall")
        {
            PlayerControl playerControl = this.GetComponent<PlayerControl>();
            int currentHits = isPunching ? 0 : playerControl.GetHits() - 1;
            playerControl.SetHits(currentHits);
            col = collision;
            StartCoroutine(Repel());
        }
        if (isPunching)
        {
            isPunching = false;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
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
    }

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
            speed = 1;
        }
        else
        {
            if (input.x != 0 || input.y != 0)
            {
                if (speed <= maxSpeed)
                {
                    accInc = accInc + 0.001f;
                    speed = speed + acc * Mathf.Pow(1.001f, accInc);
                    if (speed >= maxSpeed)
                    {
                        speed = maxSpeed;
                        accInc = 0.001f;
                        acc = 0.05f;
                    }
                }
            }
            else
            {
                if (speed != 0)
                {
                    acc = 0.05f;
                    accInc = 0.001f;
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
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

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
            isPunching = true;
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = Input.mousePosition - pos;
            movement = new Vector2(dir.x, dir.y).normalized;
            speed = punchSpeed;
            rb.velocity = movement * speed;
        }
    }

    IEnumerator Dash()
    {
        float oldSpeed = speed;
        isDashing = true;
        rb.velocity = movement * speed * 3;
        yield return new WaitForSeconds(0.03f);
        speed = oldSpeed;
        isDashing = false;
    }

    IEnumerator Repel()
    {
        isRepelled = true;
        ContactPoint2D hitpos = col.GetContact(0);
        speed = 6.5f;
        movement = Vector2.Reflect(movement, hitpos.normal);
        rb.velocity = movement * speed;
        yield return new WaitForSeconds(0.25f);
        speed = 0.25f;
        isRepelled = false;
    }
}
