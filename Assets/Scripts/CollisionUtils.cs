using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionUtils : MonoBehaviour
{
    // Raycast Utils

    public static RaycastHit2D[] CastArc(int hits, float angle, Vector2 origin, float originOffset, Vector2 rayDirection, float rayLength, LayerMask layerMask, bool debugMode)
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

    public static RaycastHit2D[] CastCircle(int hits, Vector2 origin, float originOffset, float radius, LayerMask layerMask, bool debugMode)
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
        Vector3 origin = front ? transform.position : transform.position + new Vector3(rayDirection.x * 0.35f, rayDirection.y * 0.35f, 0);
        float rayLength = cc.radius / 4;
        LayerMask layerMask = LayerMask.GetMask("Default");

        return CastArc(hits, 180, origin, 0, rayDirection, rayLength, layerMask, debugMode);
    }

    public static RaycastHit2D[] CastHitsMovementFront(int hits, Vector2 movement, Transform transform, CircleCollider2D cc, bool debugMode)
    {
        RaycastHit2D[] hit2Ds = new RaycastHit2D[hits];
        int hitsFront = 7;
        int hitsSides = (hits - hitsFront) / 2;
        float angleFront = 45;
        float angleSides = (180 - angleFront) / 2f - 0.5f;
        float rotateAngle = angleFront / 2 + 0.5f + angleSides / 2;

        Vector2 rayDirectionR = VectorUtils.RotateVector(movement, -rotateAngle);
        Vector2 rayDirectionL = VectorUtils.RotateVector(movement, rotateAngle);
        LayerMask layerMask = LayerMask.GetMask("Default");

        RaycastHit2D[] hits2DFront = CastArc(hitsFront, angleFront, transform.position, 0, movement, cc.radius / 4 + 0.2f, layerMask, debugMode);
        RaycastHit2D[] hits2DRight = CastArc(hitsSides, angleSides, transform.position, 0, rayDirectionR, cc.radius / 4 - 0.1f, layerMask, debugMode);
        RaycastHit2D[] hits2DLeft = CastArc(hitsSides, angleSides, transform.position, 0, rayDirectionL, cc.radius / 4 - 0.1f, layerMask, debugMode);

        hits2DFront.CopyTo(hit2Ds, 0);
        hits2DRight.CopyTo(hit2Ds, hits2DFront.Length);
        hits2DLeft.CopyTo(hit2Ds, hits2DRight.Length);

        return hit2Ds;
    }

    public static RaycastHit2D[] CastHitsMovementFrontExt(int hits, Vector2 movement, Transform transform, CircleCollider2D cc, bool debugMode)
    {
        return CastArc(hits, 180, transform.position, 0, movement, cc.radius / 4 + 0.05f, LayerMask.GetMask("Default"), debugMode);
    }

    public static int CountCollisions(RaycastHit2D[] hits, string tag)
    {
        int collisions = 0;
        bool filtered = true;

        foreach (RaycastHit2D rc in hits)
        {
            if (rc.collider && tag != "Any")
            {
                filtered = rc.collider.tag == tag;
            }

            if (rc.collider && filtered)
            {
                collisions++;
            }

        }

        return collisions;
    }

    public static RaycastHit2D GetCloserHit(RaycastHit2D[] hits, Transform transform, string tag)
    {
        RaycastHit2D closerHit = new RaycastHit2D();
        bool filtered = true;
        bool first = true;
        float minDistance = 0;

        foreach (RaycastHit2D rc in hits)
        {
            bool hasCollider = rc.collider;

            if (hasCollider && tag != "Any")
            {
                filtered = rc.collider.tag == tag;
            }

            if (first && hasCollider && filtered)
            {
                closerHit = rc;
                Vector3 direction = transform.position - new Vector3(closerHit.point.x, closerHit.point.y, 0);
                minDistance = direction.sqrMagnitude;
                first = false;
            }
            else if (hasCollider && filtered)
            {
                Vector3 direction = transform.position - new Vector3(rc.point.x, rc.point.y, 0);
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

    public static RaycastHit2D GetCloserHit(RaycastHit2D[] hits, Transform transform, List<string> tags)
    {
        RaycastHit2D closerHit = new RaycastHit2D();
        bool first = true;
        bool inTags;
        float minDistance = 0;

        foreach (RaycastHit2D rc in hits)
        {
            inTags = rc.collider && tags.Contains(rc.collider.tag) ? true : false;

            if (first && inTags)
            {
                closerHit = rc;
                Vector3 direction = transform.position - new Vector3(closerHit.point.x, closerHit.point.y, 0);
                minDistance = direction.sqrMagnitude;
                first = false;
            }
            else if (inTags)
            {
                Vector3 direction = transform.position - new Vector3(rc.point.x, rc.point.y, 0);
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

    public static RaycastHit2D GetCloserHitToHit(RaycastHit2D[] hits, RaycastHit2D hit, string tag)
    {
        RaycastHit2D closerHit = new RaycastHit2D();
        bool filtered = true;
        bool first = true;
        float minDistance = 0;

        foreach (RaycastHit2D rc in hits)
        {
            if (rc.collider && tag != "Any")
            {
                filtered = rc.collider.tag == tag;
            }

            if (first && filtered)
            {
                closerHit = rc;
                Vector2 direction = hit.point - new Vector2(closerHit.point.x, closerHit.point.y);
                minDistance = direction.sqrMagnitude;
                first = false;
            }
            else if (filtered)
            {
                Vector2 direction = hit.point - new Vector2(rc.point.x, rc.point.y);
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

    // Basic collisions utils
    public static float RelativePositionToCollider(Transform transform, Collider2D collider)
    {
        float position = 0f;

        float rotation = collider.attachedRigidbody.rotation;
        Vector2 normal = VectorUtils.RotateVector(Vector2.up, rotation);
        Vector2 line = VectorUtils.RotateVector(normal, -90);

        Bounds bounds = collider.bounds;
        Vector2 center = new Vector2(bounds.center.x, bounds.center.y);
        Vector2 borderPoint = center + line * bounds.extents.x * 2;

        Vector2 toLine = new Vector2(borderPoint.x - transform.position.x, borderPoint.y - transform.position.y);

        float crossProduct = line.x * toLine.y - line.y * toLine.x;
        if (crossProduct < 0)
        {
            position = 1f;
        }
        else if (crossProduct > 0)
        {
            position = -1f;
        }

        return position;
    }
}
