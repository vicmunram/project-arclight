using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastUtils : MonoBehaviour
{
    private static RaycastHit2D[] CastArc(int hits, float angle, Vector2 origin, float originOffset, Vector2 rayDirection, float rayLength, LayerMask layerMask, bool debugMode)
    {
        RaycastHit2D[] hit2Ds = new RaycastHit2D[hits];

        Vector2 rayOrigin = new Vector2(origin.x + rayDirection.x * originOffset, origin.y + rayDirection.y * originOffset);
        float rayAngle = angle / 2 / ((hits - 1) / 2);

        hit2Ds[0] = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, layerMask);

        if (debugMode)
        {
            Debug.DrawLine(new Vector3(rayOrigin.x, rayOrigin.y, 0), new Vector3(rayOrigin.x + rayDirection.x * rayLength, rayOrigin.y + rayDirection.y * rayLength, 0), Color.blue, 3);
        }

        int anglePos = 1;
        for (int i = 1; i < hits; i = i + 2)
        {
            Vector2 rayDirectionR = VectorUtils.RotateVector(rayDirection, -rayAngle * anglePos).normalized;
            Vector2 rayOriginR = originOffset != 0 ? origin + rayDirectionR * originOffset : rayOrigin;
            hit2Ds[i] = Physics2D.Raycast(rayOriginR, rayDirectionR, rayLength, layerMask);

            Vector2 rayDirectionL = VectorUtils.RotateVector(rayDirection, rayAngle * anglePos).normalized;
            Vector2 rayOriginL = originOffset != 0 ? origin + rayDirectionL * originOffset : rayOrigin;
            hit2Ds[i + 1] = Physics2D.Raycast(rayOriginL, rayDirectionL, rayLength, layerMask);

            anglePos++;

            if (debugMode)
            {
                Debug.DrawLine(new Vector3(rayOriginR.x, rayOriginR.y, 0), new Vector3(rayOriginR.x + rayDirectionR.x * rayLength, rayOriginR.y + rayDirectionR.y * rayLength, 0), Color.blue, 3);
                Debug.DrawLine(new Vector3(rayOriginL.x, rayOriginL.y, 0), new Vector3(rayOriginL.x + rayDirectionL.x * rayLength, rayOriginL.y + rayDirectionL.y * rayLength, 0), Color.blue, 3);
            }
        }

        return hit2Ds;
    }

    private static RaycastHit2D[] CastCircle(int hits, Vector2 origin, float originOffset, float radius, LayerMask layerMask, bool debugMode)
    {
        RaycastHit2D[] hit2Ds = new RaycastHit2D[hits];
        int hitsFront = hits / 2 + 1;
        int hitsBack = hits / 2 - 1;
        float angleBack = 180 - 2 * (90 / ((hitsFront - 1) / 2));
        RaycastHit2D[] hits2DFront = CastArc(hitsFront, 180, origin, originOffset, Vector2.up, radius, layerMask, debugMode);
        RaycastHit2D[] hits2DBack = CastArc(hitsBack, angleBack, origin, originOffset, Vector2.down, radius, layerMask, debugMode);
        hits2DFront.CopyTo(hit2Ds, 0);
        hits2DBack.CopyTo(hit2Ds, hits2DFront.Length);

        return hit2Ds;
    }

    public static RaycastHit2D[] CastOuterHits(int hits, Transform transform, CircleCollider2D cc, string layerMask, bool debugMode)
    {
        return CastCircle(hits, transform.position, 0.2f, 0.1f, LayerMask.GetMask(layerMask), debugMode);
    }

    public static RaycastHit2D[] CastInnerHits(int hits, Transform transform, CircleCollider2D cc, string layerMask, bool debugMode)
    {
        return CastCircle(hits, transform.position, 0.1f, 0.025f, LayerMask.GetMask(layerMask), debugMode);
    }

    public static RaycastHit2D[] CastHitsMovement(int hits, Vector2 movement, Transform transform, CircleCollider2D cc, bool front, bool debugMode)
    {
        Vector2 rayDirection = front ? movement : movement * -1;
        Vector3 origin = front ? transform.position : transform.position + new Vector3(rayDirection.x, rayDirection.y, 0);
        float rayLength = cc.radius / 4 + 0.4f;
        LayerMask layerMask = LayerMask.GetMask("Default");

        return CastArc(hits, 180, origin, 0, rayDirection, rayLength, layerMask, debugMode);
    }

    public static RaycastHit2D[] CastHitsMovementNearObjects(int hits, Vector2 movement, Transform transform, CircleCollider2D cc, bool debugMode)
    {
        float rayLength = cc.radius / 4 + 0.25f;
        LayerMask layerMask = LayerMask.GetMask("Default");
        return CastArc(hits, 30, transform.position, 0, movement, rayLength, layerMask, debugMode);
    }

    public static int CountCollisionsByTag(RaycastHit2D[] hits, string tag)
    {
        int collisions = 0;

        foreach (RaycastHit2D rc in hits)
        {
            if (tag != "Any")
            {
                if (rc.collider && rc.collider.tag == tag)
                {
                    collisions++;
                }
            }
            else
            {
                collisions++;
            }
        }

        return collisions;
    }

    public static RaycastHit2D GetCloserHit(RaycastHit2D[] hits, Transform transform)
    {
        RaycastHit2D closerHit = new RaycastHit2D();
        bool first = true;
        float minDistance = 0;

        foreach (RaycastHit2D rc in hits)
        {
            if (first)
            {
                closerHit = rc;
                Vector3 direction = transform.position - new Vector3(closerHit.point.x, closerHit.point.y, 0);
                minDistance = direction.sqrMagnitude;
                first = false;
            }
            else
            {
                Vector3 direction = transform.position - new Vector3(closerHit.point.x, closerHit.point.y, 0);
                float distance = direction.sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closerHit = rc;
                }
            }
        }

        return closerHit;
    }
}
