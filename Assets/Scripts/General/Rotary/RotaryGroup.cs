using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class RotaryGroup : MonoBehaviour
{
    public Rotary[] auxRotaries;
    public void Rotate()
    {
        Rotary[] rotaries = gameObject.GetComponentsInChildren<Rotary>();
        if (rotaries.Length == 0)
        {
            rotaries = auxRotaries;
        }

        bool loopsRotating = false;
        foreach (Rotary rotary in rotaries)
        {
            if (typeof(RotaryLoop).IsInstanceOfType(rotary) && rotary.isRotating)
            {
                loopsRotating = true;
            }
        }

        foreach (Rotary rotary in rotaries)
        {
            if (typeof(RotaryLoop).IsInstanceOfType(rotary) && !loopsRotating)
            {
                rotary.isRotating = true;
            }
            if (typeof(RotaryInfinite).IsInstanceOfType(rotary))
            {
                rotary.isRotating = !rotary.isRotating;
            }

        }
    }

}
