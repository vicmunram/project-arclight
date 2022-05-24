using UnityEngine;

public class PlayerChanger : Changer
{
    public bool canPunch;
    public bool canDash;
    public float maxSpeed;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    protected override void Change()
    {
        playerMovement.canPunch = canPunch;
        playerMovement.canDash = canDash;
        playerMovement.maxSpeed = maxSpeed;
    }
}
