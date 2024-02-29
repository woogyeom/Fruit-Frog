using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;
    public Transform maxTarget;
    public int curHealth;

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
        maxTarget = GetMax();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }

    Transform GetMax()
    {
        Transform result = null;
        int health = 0;

        foreach (RaycastHit2D target in targets)
        {
            if (target.collider.GetComponent<Enemy>() != null)
            {
                curHealth = target.collider.GetComponent<Enemy>().health;
            }
            else
            {
                curHealth = target.collider.GetComponent<Boss>().health;
            }
            
            if (curHealth > health)
            {
                health = curHealth;
                result = target.transform;
            }
        }

        return result;
    }
}
