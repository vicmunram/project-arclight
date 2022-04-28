using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public float offset;
    public float xMax;
    public float xMin;
    public float yMax;
    public float yMin;
    void Start()
    {
        this.transform.position = new Vector3(FixCoordinate(player.position.x, xMin, xMax), FixCoordinate(player.position.y, yMin, yMax), -2);
    }

    void FixedUpdate()
    {
        this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, FixCoordinate(player.position.x, xMin, xMax), Time.deltaTime * offset),
                                               Mathf.Lerp(this.transform.position.y, FixCoordinate(player.position.y, yMin, yMax), Time.deltaTime * offset),
                                               -2);
    }

    private float FixCoordinate(float playerCoordinate, float min, float max)
    {
        float coordinate = playerCoordinate;

        if (min != 0 && coordinate < min)
        {
            coordinate = min;
        }
        else if (max != 0 && coordinate > max)
        {
            coordinate = max;
        }

        return coordinate;
    }
}
