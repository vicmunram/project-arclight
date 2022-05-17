using UnityEngine;

public class BreakableGroup : MonoBehaviour
{
    private Breakable[] breakables;

    void Start()
    {
        breakables = gameObject.GetComponentsInChildren<Breakable>();
    }
    void FixedUpdate()
    {
        int positioneds = 0;
        foreach (Breakable breakable in breakables)
        {
            if (breakable.positioned)
            {
                positioneds++;
            }
        }

        if (positioneds == breakables.Length)
        {
            foreach (Breakable breakable in breakables)
            {
                BoxCollider2D bc = breakable.GetComponent<BoxCollider2D>();
                if (bc.enabled == false)
                {
                    bc.enabled = true;
                }
            }
        }
    }
}