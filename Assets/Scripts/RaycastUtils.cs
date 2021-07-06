using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastUtils : MonoBehaviour
{
    private static RaycastHit2D[] CastParcialCircle(int hits, Vector2 direction, Vector2 origin, float originOffset, float radius, float angle, LayerMask layerMask, bool outer)
    {
        RaycastHit2D[] hit2Ds = new RaycastHit2D[hits];

        Vector2 rayDirection = direction;
        Vector2 rayOrigin = new Vector2(origin.x + rayDirection.x * originOffset, origin.y + rayDirection.y * originOffset);
        float rayLength = radius;
        float rayAngle = angle / 2 / ((hits - 1) / 2);

        hit2Ds[0] = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, layerMask);

        int anglePos = 1;
        for (int i = 1; i < hits; i = i + 2)
        {
            Vector2 rayDirectionR = VectorUtils.RotateVector(rayDirection, -rayAngle * anglePos).normalized;
            Vector2 rayOriginR = outer ? origin + rayDirectionR / 5 : rayOrigin;
            hit2Ds[i] = Physics2D.Raycast(rayOriginR, rayDirectionR, rayLength, layerMask);

            Vector2 rayDirectionL = VectorUtils.RotateVector(rayDirection, rayAngle * anglePos).normalized;
            Vector2 rayOriginL = outer ? origin + rayDirectionL / 5 : rayOrigin;
            hit2Ds[i + 1] = Physics2D.Raycast(rayOriginL, rayDirectionL, rayLength, layerMask);

            anglePos++;

            Debug.DrawLine(new Vector3(rayOriginR.x, rayOriginR.y, 0), new Vector3(rayOriginR.x + rayDirectionR.x * rayLength, rayOriginR.y + rayDirectionR.y * rayLength, 0), Color.blue, 3);
            Debug.DrawLine(new Vector3(rayOriginL.x, rayOriginL.y, 0), new Vector3(rayOriginL.x + rayDirectionL.x * rayLength, rayOriginL.y + rayDirectionL.y * rayLength, 0), Color.blue, 3);

        }

        return hit2Ds;
    }

    private static RaycastHit2D[] CastCircle(int hits, Vector2 origin, float originOffset, float radius, LayerMask layerMask, bool outer)
    {
        RaycastHit2D[] hit2Ds = new RaycastHit2D[hits];
        RaycastHit2D[] hitsFront = CastParcialCircle(hits / 2 + 1, Vector2.up, origin, originOffset, radius, 180, layerMask, outer);
        RaycastHit2D[] hitsBack = CastParcialCircle(hits / 2 - 1, Vector2.down, origin, originOffset, radius, (hits / 2 - 1) * 24, layerMask, outer);
        hitsFront.CopyTo(hit2Ds, 0);
        hitsBack.CopyTo(hit2Ds, hits / 2 + 1);

        return hit2Ds;
    }

    public static RaycastHit2D[] CastOuterHits(int hits, Transform transform, CircleCollider2D cc)
    {
        return CastCircle(hits, transform.position, 0.05f, 0.04f, LayerMask.GetMask("Is Trigger"), true);
    }

    public static RaycastHit2D[] CastInnerHits(int hits, Transform transform, CircleCollider2D cc)
    {
        return CastCircle(hits, transform.position, 0.01f, 0.04f, LayerMask.GetMask("Is Trigger"), true);
    }

    public static RaycastHit2D[] CastHitsMovement(int hits, Vector2 movement, Transform transform, CircleCollider2D cc, bool front)
    {
        Vector2 direction = front ? movement : movement * -1;
        Vector3 origin = front ? transform.position : transform.position + new Vector3(direction.x, direction.y, 0);
        float radius = cc.radius / 4 + 0.25f;
        LayerMask mask = LayerMask.GetMask("Default");

        return CastParcialCircle(hits, direction, origin, 0, radius, 180, mask, false);
    }

    public static RaycastHit2D[] CastHitsMovementNearObjects(int hits, Vector2 movement, Transform transform, CircleCollider2D cc)
    {
        float radius = cc.radius / 4 + 0.25f;
        LayerMask mask = LayerMask.GetMask("Default");
        return CastParcialCircle(hits, movement, transform.position, 0, radius, 30, mask, false);
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
