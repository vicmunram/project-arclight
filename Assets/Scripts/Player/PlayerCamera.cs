using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //Basic
    public Transform player;
    public float offset;
    public Vector2 xRange;
    public bool xFixedMin;
    public bool xFixedMax;
    public Vector2 yRange;
    public bool yFixedMin;
    public bool yFixedMax;

    //Resizing
    private bool resizing;
    private Camera main;
    private float t = 0.0f;
    private float cameraSize;

    void Start()
    {
        xFixedMin = xFixedMin || xRange.x != 0;
        xFixedMax = xFixedMax || xRange.y != 0;
        yFixedMin = yFixedMin || yRange.x != 0;
        yFixedMax = yFixedMax || yRange.y != 0;

        this.transform.position = new Vector3(FixCoordinate(player.position.x, xRange, xFixedMin, xFixedMax), FixCoordinate(player.position.y, yRange, yFixedMin, yFixedMax), -2);
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
        this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, FixCoordinate(player.position.x, xRange, xFixedMin, xFixedMax), Time.deltaTime * offset),
                                          Mathf.Lerp(this.transform.position.y, FixCoordinate(player.position.y, yRange, yFixedMin, yFixedMax), Time.deltaTime * offset),
                                          -2);
    }

    private float FixCoordinate(float playerCoordinate, Vector2 range, bool fixedMin, bool fixedMax)
    {
        float coordinate = playerCoordinate;

        if (fixedMin && coordinate < range.x)
        {
            coordinate = range.x;
        }
        else if (fixedMax && coordinate > range.y)
        {
            coordinate = range.y;
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

    public void ChangeRange(Vector2 newXRange, Vector2 newYRange)
    {
        if (!xFixedMin)
        {
            xFixedMin = newXRange.x != xRange.x;
        }
        if (!xFixedMax)
        {
            xFixedMax = xFixedMax || newXRange.y != xRange.y;
        }
        xRange = newXRange;

        if (!yFixedMin)
        {
            yFixedMin = yFixedMin || newYRange.x != yRange.x;
        }
        if (!yFixedMax)
        {
            yFixedMax = yFixedMax || newYRange.y != yRange.y;
        }
        yRange = newYRange;
    }
}
