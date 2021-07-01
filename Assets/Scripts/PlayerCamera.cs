using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public float offset;

    void FixedUpdate()
    {
        this.transform.position = new Vector3 (Mathf.Lerp(this.transform.position.x, player.position.x, Time.deltaTime*offset),
                                               Mathf.Lerp(this.transform.position.y, player.position.y, Time.deltaTime*offset),
                                               -2);
    }
}
