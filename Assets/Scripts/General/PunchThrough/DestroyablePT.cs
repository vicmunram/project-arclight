using System.Collections;
using UnityEngine;

public class DestroyablePT : MonoBehaviour
{
    public float recoveryTime;

    public void Destroy()
    {
        StartCoroutine(Destruction());
    }

    IEnumerator Destruction()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(recoveryTime);
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
