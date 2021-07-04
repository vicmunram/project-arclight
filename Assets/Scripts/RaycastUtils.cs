using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastUtils : MonoBehaviour
{
    private static RaycastHit2D[] CastSemiCircle(int hits, Vector2 direction, Vector2 origin, float radius, LayerMask layerMask, bool outer)
    {
        RaycastHit2D[] hit2Ds = new RaycastHit2D[hits];
        float angle = 90 / ((hits - 1) / 2);

        Vector2 firstOrigin = outer ? new Vector2(origin.x, origin.y + radius) : origin;
        float distance = outer ? 0.01f : radius;

        hit2Ds[0] = Physics2D.Raycast(firstOrigin, direction, distance, layerMask);

        int anglePos = 1;
        for (int i = 1; i < hits; i = i + 2)
        {
            Vector2 vectorR = VectorUtils.RotateVector(direction, angle * anglePos);
            Vector2 vectorL = VectorUtils.RotateVector(direction, -angle * anglePos);
            Vector2 originR = outer ? origin + vectorR / 5 : firstOrigin;
            Vector2 originL = outer ? origin + vectorL / 5 : firstOrigin;
            hit2Ds[i] = Physics2D.Raycast(originR, vectorR, distance, layerMask);
            hit2Ds[i + 1] = Physics2D.Raycast(originL, vectorL, distance, layerMask);
            anglePos++;
        }

        return hit2Ds;
    }

    private static RaycastHit2D[] CastCircle(int hits, Vector2 origin, float radius, LayerMask layerMask, bool outer)
    {
        RaycastHit2D[] hit2Ds = new RaycastHit2D[hits];
        RaycastHit2D[] hitsFront = CastSemiCircle(hits / 2 + 1, Vector2.up, origin, radius, layerMask, outer);
        RaycastHit2D[] hitsBack = CastSemiCircle(hits / 2 - 1, Vector2.down, origin, outer ? -radius : radius, layerMask, outer);
        hitsFront.CopyTo(hit2Ds, 0);
        hitsBack.CopyTo(hit2Ds, hits / 2 + 1);

        return hit2Ds;
    }

    public static RaycastHit2D[] CastHits(int hits, Transform transform, CircleCollider2D cc)
    {
        return CastCircle(hits, transform.position, cc.radius / 5, LayerMask.GetMask("Is Trigger"), true);
    }

    public static RaycastHit2D[] CastHitsMovement(int hits, Vector2 movement, Transform transform, CircleCollider2D cc, bool front)
    {
        Vector2 direction = front ? movement : movement * -1;
        Vector3 origin = front ? transform.position : transform.position + new Vector3(direction.x, direction.y, 0);
        float radius = cc.radius / 1.5f;
        LayerMask mask = LayerMask.GetMask("Default");
        return CastSemiCircle(hits, direction, origin, radius, mask, false);
    }

    public static int CountCollisionsByTag(RaycastHit2D[] hits, string tag)
    {
        int collisions = 0;

        foreach (RaycastHit2D rc in hits)
        {
            if (rc.collider && rc.collider.tag == tag)
            {
                collisions++;
            }
        }

        return collisions;
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
