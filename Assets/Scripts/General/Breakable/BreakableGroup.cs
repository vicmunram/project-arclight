using UnityEngine;
using System.Collections;

public class BreakableGroup : MonoBehaviour
{
    public GameObject CheckBlock;
    private Breakable[] breakables;
    private bool broken = false;

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

        if (positioneds < breakables.Length && !broken)
        {
            broken = true;
            AudioUtils.PlayEffect("broken");
        }

        if (positioneds == breakables.Length && broken)
        {
            StartCoroutine(Repair());
            foreach (Breakable breakable in breakables)
            {
                BoxCollider2D bc = breakable.GetComponent<BoxCollider2D>();
                if (bc.enabled == false)
                {
                    bc.enabled = true;
                }
            }
            broken = false;
        }
    }

    IEnumerator Repair()
    {
        CheckBlock.GetComponent<SpriteRenderer>().enabled = true;
        AudioUtils.PlayEffect(gameObject, true, "repaired", false);
        yield return new WaitForSeconds(0.5f);
        CheckBlock.GetComponent<SpriteRenderer>().enabled = false;
    }
}