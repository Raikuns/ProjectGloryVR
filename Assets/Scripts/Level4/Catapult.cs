using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    [SerializeField] Transform bucket;
    [SerializeField] Transform head;
    [SerializeField] Transform centerPoint;

    Vector3 shootDirection;

    private void Update()
    {
        shootDirection = centerPoint.position - bucket.position;

        HeadRotation();
    }

    private void LateUpdate()
    {
        bucket.rotation = Quaternion.LookRotation(shootDirection);
    }

    void HeadRotation()
    {
        var headRotation = shootDirection;
        headRotation.y = 0;

        head.rotation = Quaternion.LookRotation(headRotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(bucket.position, shootDirection);
    }
}
