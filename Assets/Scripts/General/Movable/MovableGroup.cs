using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MovableGroup : MonoBehaviour
{
    public Movable[] auxMovables;
    public void Move()
    {
        Movable[] movables = gameObject.GetComponentsInChildren<Movable>();
        if (movables.Length == 0)
        {
            movables = auxMovables;
        }

        bool doorsMoving = false;
        foreach (Movable movable in movables)
        {
            if (typeof(Door).IsInstanceOfType(movable) && movable.isMoving)
            {
                doorsMoving = true;
            }
        }

        foreach (Movable movable in movables)
        {
            if (typeof(Door).IsInstanceOfType(movable) && !doorsMoving)
            {
                movable.isMoving = true;
            }
        }
    }

}
