using UnityEngine;

public abstract class Changer : MonoBehaviour
{
    public bool active = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && active)
        {
            Change();
        }
    }

    public void RequestChange()
    {
        if (active)
        {
            Change();
        }
    }
    protected abstract void Change();
}
