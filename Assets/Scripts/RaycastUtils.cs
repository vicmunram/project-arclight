using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastUtils : MonoBehaviour
{
    public static RaycastHit2D[] CastHitsPunching(int hits, Vector2 movement, Transform transform, CircleCollider2D cc, bool front)
    {
        RaycastHit2D[] hit2Ds = new RaycastHit2D[hits + 1];
        float angle = 90 / ((hits - 1) / 2);
        Vector2 direction = front ? movement : movement * -1;
        Vector3 origin = front ? transform.position : transform.position + new Vector3(direction.x, direction.y, 0);
        float distance = cc.radius / 1.5f;

        hit2Ds[0] = Physics2D.Raycast(origin, direction, distance);

        int anglePos = 1;
        for (int i = 1; i < hits; i = i + 2)
        {
            Vector2 vectorR = VectorUtils.RotateVector(direction, angle * anglePos);
            Vector2 vectorL = VectorUtils.RotateVector(direction, -angle * anglePos);
            hit2Ds[i] = Physics2D.Raycast(origin, vectorR, distance);
            hit2Ds[i + 1] = Physics2D.Raycast(origin, vectorL, distance);
            anglePos++;
        }

        return hit2Ds;
    }

    public static RaycastHit2D GetCloserHit(RaycastHit2D[] hits, Transform transform)
    {
        RaycastHit2D resHit = new RaycastHit2D();
        bool first = true;
        float minDistance = 0;

        foreach (RaycastHit2D rc in hits)
        {
            if (first)
            {
                resHit = rc;
                Vector3 direction = transform.position - new Vector3(resHit.point.x, resHit.point.y, 0);
                minDistance = direction.sqrMagnitude;
                first = false;
            }
            else
            {
                Vector3 direction = transform.position - new Vector3(resHit.point.x, resHit.point.y, 0);
                float distance = direction.sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    resHit = rc;
                }
            }
        }

        return resHit;
    }
}
