using UnityEngine;

public abstract class Movable : MonoBehaviour
{
    public bool isMoving;
    public bool active = true;
    public float speed;
    public Vector2 direction;
    public Vector2 auxDirection;
    protected Rigidbody2D rb;
    protected Vector2 currentPosition;
    protected Vector2 auxPosition;
    protected Vector2 targetPosition;
    private bool auxCompleted;

    void Start()
    {
        rb = gameObject.GetComponentInParent<Rigidbody2D>();
        currentPosition = rb.transform.position;
        if (AuxActive())
        {
            auxPosition = currentPosition + auxDirection;
        }
        targetPosition = currentPosition + direction;
    }

    void FixedUpdate()
    {
        if (isMoving && active)
        {
            currentPosition = rb.transform.position;
            Move();
            CheckAuxCompleted();
            CheckCompleted();
        }
    }

    private void Move()
    {
        if (direction.SqrMagnitude() > 1)
        {
            rb.velocity = direction.normalized * speed;
        }
        else
        {
            rb.velocity = direction * speed;
        }
    }

    public void CheckCompleted()
    {
        if (ReachedOrPassed(targetPosition))
        {
            WhenCompleted();
            auxCompleted = false;
        }
    }

    public void CheckAuxCompleted()
    {
        if (AuxActive() && !auxCompleted && ReachedOrPassed(auxPosition))
        {
            WhenAuxCompleted();
            auxCompleted = true;
        }
    }

    public abstract void WhenCompleted();
    public abstract void WhenAuxCompleted();

    private bool ReachedOrPassed(Vector2 target)
    {
        Vector2 distance = target - currentPosition;
        bool passed = Mathf.Sign(direction.x * distance.x) != 1 || Mathf.Sign(direction.y * distance.y) != 1;
        return VectorUtils.Equals(currentPosition, target) || passed;
    }

    protected bool AuxActive()
    {
        return auxDirection.x != 0 || auxDirection.y != 0;
    }

}
