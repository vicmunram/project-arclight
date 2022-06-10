using UnityEngine;
using System.Collections;

public class MovableGroup : MonoBehaviour
{
    public bool active;
    public float startDelay;
    public float moveRate;
    public Movable[] auxMovables;
    private Movable[] movables;

    void Start()
    {
        movables = gameObject.GetComponentsInChildren<Movable>();
        if (movables.Length == 0)
        {
            movables = auxMovables;
        }
    }
    void FixedUpdate()
    {
        if (active)
        {
            if (moveRate != 0)
            {
                StartCoroutine(RatedMove());
            }
            else
            {
                StartCoroutine(Move());
            }
        }
    }
    IEnumerator Move()
    {
        yield return new WaitForSeconds(startDelay);

        bool doorsMoving = false;
        foreach (Movable movable in movables)
        {
            if (typeof(Door).IsInstanceOfType(movable) && movable.IsMoving())
            {
                doorsMoving = true;
            }
        }

        foreach (Movable movable in movables)
        {
            if (typeof(Door).IsInstanceOfType(movable) && !doorsMoving)
            {
                movable.Activate();
            }
            else
            {
                movable.Activate();
            }
        }

        active = false;
    }

    IEnumerator RatedMove()
    {
        yield return new WaitForSeconds(startDelay);

        foreach (Movable movable in movables)
        {
            movable.Activate();
            yield return new WaitForSeconds(moveRate);
        }
    }
}