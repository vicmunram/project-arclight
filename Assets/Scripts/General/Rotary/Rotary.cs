using UnityEngine;

public abstract class Rotary : MonoBehaviour
{
    public bool isRotating;
    public float speed;
    public bool auxActive;
    public float grades;
    public float auxGrades;
    protected Rigidbody2D rb;
    protected float currentRotation;
    protected float rotated = 0;
    protected float auxRotated;
    protected float targetRotated;
    private bool auxCompleted;

    void Start()
    {
        rb = gameObject.GetComponentInParent<Rigidbody2D>();
        currentRotation = rb.rotation;
        auxRotated = Mathf.Abs(auxGrades) / speed;
        targetRotated = Mathf.Abs(grades) / speed;
    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            currentRotation = rb.rotation;
            Rotate();
            CheckAuxCompleted();
            CheckCompleted();
        }
    }
    private void Rotate()
    {
        float rotation = speed;
        if (rotated + 1 > targetRotated)
        {
            rotation = (Mathf.Abs(grades) - rotated * speed);
        }

        rb.rotation = currentRotation + rotation * Mathf.Sign(grades);
        rotated++;

    }

    public void CheckCompleted()
    {
        if (ReachedOrPassed(targetRotated))
        {
            WhenCompleted();
            auxCompleted = false;
        }
    }

    public void CheckAuxCompleted()
    {
        if (auxActive && !auxCompleted && ReachedOrPassed(auxRotated))
        {
            WhenAuxCompleted();
            auxCompleted = true;
        }
    }

    public abstract void WhenCompleted();
    public abstract void WhenAuxCompleted();


    private bool ReachedOrPassed(float target)
    {
        return rotated.CompareTo(target) >= 0;
    }
}
