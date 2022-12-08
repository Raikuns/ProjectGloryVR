using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    [SerializeField] Rigidbody projectile;

    [SerializeField] Transform bucket;
    [SerializeField] Transform head;
    [SerializeField] Transform centerPoint;

    [SerializeField] float flingTime = 0.5f;
    [SerializeField] LeanTweenType flingType;

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

    public void Release()
    {
        LeanTween.move(bucket.gameObject, centerPoint.position, flingTime).setEase(flingType).setOnComplete(Shoot);
    }

    void Shoot()
    {
        projectile.transform.SetParent(null);
        projectile.isKinematic = false;
        projectile.AddForce(shootDirection.normalized * 1000);
    }

    public void SetDickParent(Transform dick)
    {
        dick.SetParent(bucket);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(bucket.position, shootDirection);
    }
}
