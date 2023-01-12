using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using UnityEngine.UI;
using TMPro;

public class Catapult : MonoBehaviour
{
    [SerializeField] Rigidbody projectile;

    [SerializeField] Transform bucket;
    [SerializeField] Transform head;
    [SerializeField] Transform centerPoint;

    [SerializeField] float flingTime = 0.5f;
    [SerializeField] LeanTweenType flingType;

    [SerializeField] Transform leftConnection;
    [SerializeField] Transform rightConnection;

    [SerializeField] LineRenderer line;

    Vector3 shootDirection;

    float shootingPower = 0;
    bool isGrabbed = false;

    [SerializeField] TMP_Text powerText;

    private void Update()
    {
        shootDirection = centerPoint.position - bucket.position;

        HeadRotation();
        CalculatePower();

        line.SetPosition(1, line.transform.InverseTransformPoint(leftConnection.position));
        line.SetPosition(2, line.transform.InverseTransformPoint(rightConnection.position));
    }

    private void LateUpdate()
    {
        bucket.rotation = Quaternion.LookRotation(shootDirection);
    }

    void CalculatePower()
    {
        if (!isGrabbed)
            return;

        float distance = Vector3.Distance(bucket.position, centerPoint.position);

        shootingPower = distance * 900;

        if (powerText != null)
            powerText.text = shootingPower.ToString();
    }

    void HeadRotation()
    {
        var headRotation = shootDirection;
        headRotation.y = 0;

        head.rotation = Quaternion.LookRotation(headRotation);
    }

    public void OnGrab()
    {
        isGrabbed = true;
    }

    public void Release()
    {
        isGrabbed = false;
        LeanTween.move(bucket.gameObject, centerPoint.position, flingTime).setEase(flingType).setOnComplete(Shoot);
    }

    void Shoot()
    {
        projectile.transform.SetParent(null);
        projectile.isKinematic = false;
        projectile.AddForce(shootDirection.normalized * shootingPower);
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
