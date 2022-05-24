using UnityEngine;

public class CameraChanger : Changer
{
    public float size;
    public Vector2 xRange;
    public Vector2 yRange;
    private PlayerCamera playerCamera;

    void Start()
    {
        playerCamera = GameObject.Find("Player Camera").GetComponent<PlayerCamera>();
    }

    protected override void Change()
    {
        playerCamera.Resize(size);
        playerCamera.xRange = xRange;
        playerCamera.yRange = yRange;
    }
}
