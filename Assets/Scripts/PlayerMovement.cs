using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float maxSpeed = 10f;
    public float speed = 0f;
    public float acc = 0.1f;

    public Rigidbody2D rb;
    public CircleCollider2D cc;
    Vector2 input;
    public Vector2 movement = new Vector2(0,0);

    public float dashRate = 0.1f;
    private float nextDash;
    private bool isDashing;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update(){
        GetPlayerInput();
        if(Input.GetKeyDown(KeyCode.Mouse1) && Time.time > nextDash) {
            nextDash = Time.time + dashRate;
            StartCoroutine(Dash());
        }
        else if(!isDashing){
            SpeedControl();
        }
    }

    void FixedUpdate(){
        if(!isDashing) {
            MovePlayer();
        }
    }

    private void GetPlayerInput() {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        if(input.x != 0 || input.y !=0){
            movement = input;
        }
    }

    private void MovePlayer() {
        if(movement.x != 0 && movement.y !=0){
            rb.velocity = movement.normalized * speed;
        }
        else{
            rb.velocity = movement * speed;
        }
    }

    private void SpeedControl() {
        if(input.x != 0 || input.y !=0){
            if(speed <= maxSpeed) {
                speed = speed + acc;
                if (speed > maxSpeed) {
                    speed = maxSpeed;
                }
            }
        }
        else{
            if(speed != 0){
                speed = speed/2;
                if(speed < 0.1){
                    speed = 0;
                }
            }
        }
    }

    IEnumerator Dash () {
        float oldSpeed = speed;
        isDashing = true;
        speed = 30;
        rb.velocity = movement * speed;
        yield return new WaitForSeconds(0.03f);
        speed = oldSpeed;
        isDashing = false;
    }
}
