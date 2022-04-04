using UnityEngine;

public class VectorUtils : MonoBehaviour
{
    public static Vector2 RotateVector(Vector2 v, float angle)
    {
        float radianAngle = angle * Mathf.Deg2Rad;
        float x = v.x * Mathf.Cos(radianAngle) - v.y * Mathf.Sin(radianAngle);
        float y = v.x * Mathf.Sin(radianAngle) + v.y * Mathf.Cos(radianAngle);
        return new Vector2(x, y);
    }
}
