using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using FIMSpace.FTail;
using UnityEditor;

public class HandPositionCalculator : MonoBehaviour
{
    [SerializeField] Transform[] bones;
    [SerializeField] OneGrabTranslateTransformer limiter;
    TailAnimator2 tail;

    public Transform closest1, closest2;

    public float lerpTime = 0.25f;

    private void Start()
    {
        tail = FindObjectOfType<TailAnimator2>();
    }

    private void Update()
    {
        SetMinAndMaxPoints();
    }

    void SetMinAndMaxPoints()
    {
        GetClosest();

        limiter.Constraints.MinX.Value = closest1.position.x;
        limiter.Constraints.MaxX.Value = closest1.position.x;

        SetYMax();

        limiter.Constraints.MaxZ.Value = bones[0].transform.position.z;
        limiter.Constraints.MinZ.Value = bones[bones.Length - 1].transform.position.z;
    }

    void GetClosest()
    {
        Transform bestTarget = null;
        int boneID = 0;
        float closestDistanceSqr = Mathf.Infinity;

        for (int b = 0; b < bones.Length; b++)
        {
            float dist = Vector3.Distance(limiter.transform.position, bones[b].position);
            if (dist < closestDistanceSqr)
            {
                if (b > 0)
                {
                    if (b == bones.Length - 1)
                    {
                        bestTarget = bones[bones.Length - 1];
                    }
                    else
                    {
                        bestTarget = bones[b - 1];
                    }
                }
                else
                {
                    bestTarget = bones[0];
                }
                closestDistanceSqr = dist;
                boneID = b;
            }
        }

        closest1 = bestTarget;

        Vector3 worldPosition = closest1.TransformPoint(closest1.localPosition);

        if (boneID != 0 && boneID != bones.Length - 1)
        {
            if (limiter.transform.position.z > worldPosition.z)
            {
                if (boneID > 1)
                {
                    closest2 = bones[boneID - 2];
                }
            }
            else if (limiter.transform.position.z < worldPosition.z)
            {
                closest2 = bones[boneID];
            }
        }
    }

    void SetYMax()
    {
        //Vector3 desiredPos = FindNearestPointOnLine(closest1.position, closest2.position, limiter.transform.position);
        float lerpedY = Mathf.Lerp(limiter.Constraints.MinY.Value, closest1.position.y, Time.deltaTime / lerpTime);

        limiter.Constraints.MaxY.Value = lerpedY;
        limiter.Constraints.MinY.Value = lerpedY;
    }

    public void OnGrab()
    {
        //tail.enabled = false;
    }

    public void OnRelease()
    {
        //tail.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Handles.DrawBezier(limiter.transform.position, closest1.position, limiter.transform.position, closest1.position, Color.green, null, 3);
        Handles.DrawBezier(limiter.transform.position, closest2.position, limiter.transform.position, closest2.position, Color.blue, null, 4);
    }

    public Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 end, Vector3 hand)
    {
        //Get heading
        Vector3 heading = (end - origin);
        float magnitudeMax = heading.magnitude;
        heading.Normalize();

        //Do projection from the point but clamp it
        Vector3 lhs = hand - origin;
        float dotP = Vector3.Dot(lhs, heading);
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
        return origin + heading * dotP;
    }
}
