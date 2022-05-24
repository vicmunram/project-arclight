using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //Basic
    public Transform player;
    public float offset;
    public Vector2 xRange;
    public Vector2 yRange;

    //Resizing
    private bool resizing;
    private Camera main;
    private float t = 0.0f;
    private float cameraSize;

    //Resolution
    float defaultWidth;
    void Start()
    {
        this.transform.position = new Vector3(FixCoordinate(player.position.x, xRange.x, xRange.y), FixCoordinate(player.position.y, yRange.x, yRange.y), -2);
        main = Camera.main;
    }

    void Update()
    {
        if (resizing)
        {
            main.orthographicSize = Mathf.Lerp(main.orthographicSize, cameraSize, t);
            t += 0.25f * Time.deltaTime;

            if (t > 1)
            {
                t = 0.0f;
                resizing = false;
            }
        }
    }

    void FixedUpdate()
    {
        this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, FixCoordinate(player.position.x, xRange.x, xRange.y), Time.deltaTime * offset),
                                          Mathf.Lerp(this.transform.position.y, FixCoordinate(player.position.y, yRange.x, yRange.y), Time.deltaTime * offset),
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

    public void Resize(float size)
    {
        if (main.orthographicSize != size)
        {
            resizing = true;
            t = 0.0f;
            cameraSize = size;
        }
    }
}
