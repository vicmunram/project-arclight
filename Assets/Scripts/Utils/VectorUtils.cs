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

    public static bool Equals(Vector2 v1, Vector2 v2)
    {
        float xDiff = Mathf.Abs(Mathf.Round(v1.x * 100f) - Mathf.Round(v2.x * 100f));
        float yDiff = Mathf.Abs(Mathf.Round(v1.y * 100f) - Mathf.Round(v2.y * 100f));
        return xDiff == 0 && yDiff == 0;
    }

    public static bool Equals(Vector2 v1, Vector2 v2, float precision)
    {
        float xDiff = Mathf.Abs(Mathf.Round(v1.x * precision) - Mathf.Round(v2.x * precision));
        float yDiff = Mathf.Abs(Mathf.Round(v1.y * precision) - Mathf.Round(v2.y * precision));
        return xDiff == 0 && yDiff == 0;
    }
}
