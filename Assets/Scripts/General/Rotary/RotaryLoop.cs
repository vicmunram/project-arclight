using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class RotaryLoop : Rotary
{
    public float extraGrades;
    private bool first = true;
    public override void WhenCompleted()
    {
        isRotating = false;
        rotated = 0;
        if (first)
        {
            grades = (Mathf.Abs(grades) + extraGrades) * Mathf.Sign(grades);
            targetRotated = Mathf.Abs(grades) / speed;
            first = false;
        }
        grades = grades * -1;
    }

    public override void WhenAuxCompleted() { }

}
