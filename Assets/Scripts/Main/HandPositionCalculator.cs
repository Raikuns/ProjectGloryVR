using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using FIMSpace.FTail;
using UnityEditor;

public class HandPositionCalculator : MonoBehaviour
{
    [SerializeField] Transform[] bones;
    [SerializeField] Transform grabbable;
    TailAnimator2 tail;

    public Transform closest1, closest2;
    Transform lastClosest;

    public float lerpTime = 0.25f;

    float lerpedY;

    [HideInInspector] public Penis penis;

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

        float maxX = bones[0].transform.position.x;
        float minX = bones[bones.Length - 1].transform.position.x;

        if (!penis.mirrored)
        {
            if (grabbable.transform.position.x > maxX)
            {
                SetPosition(maxX);
            }
            else if (grabbable.transform.position.x < minX)
            {
                SetPosition(minX);
            }
            else
            {
                SetPosition(grabbable.position.x);
            }
        }
        else
        {
            if (grabbable.transform.position.x < maxX)
            {
                SetPosition(maxX);
            }
            else if (grabbable.transform.position.x > minX)
            {
                SetPosition(minX);
            }
            else
            {
                SetPosition(grabbable.position.x);
            }
        }
    }

    void SetPosition(float x)
    {
        grabbable.transform.position = new Vector3(x, lerpedY, closest1.position.z);
    }

    void SetY()
    {
        if (lastClosest == null)
        {
            lerpedY = grabbable.transform.position.y;
            return;
        }

        LeanTween.value(gameObject, lastClosest.position.y, closest1.position.y, .3f).setOnUpdate(LerpY);
    }

    void LerpY(float y)
    {
        lerpedY = y;
    }

    void GetClosest()
    {
        Transform bestTarget = null;
        int boneID = 0;
        float closestDistanceSqr = Mathf.Infinity;

        for (int b = 0; b < bones.Length; b++)
        {
            float dist = Vector3.Distance(grabbable.transform.position, bones[b].position);
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

        if (bestTarget != lastClosest)
        {
            SetY();
        }

        lastClosest = closest1;
        closest1 = bestTarget;

        Vector3 worldPosition = closest1.TransformPoint(closest1.localPosition);

        if (boneID != 0 && boneID != bones.Length - 1)
        {
            if (grabbable.transform.position.z > worldPosition.z)
            {
                if (boneID > 1)
                {
                    closest2 = bones[boneID - 2];
                }
            }
            else if (grabbable.transform.position.z < worldPosition.z)
            {
                closest2 = bones[boneID];
            }
        }
    }

    public void OnGrab()
    {
        //tail.enabled = false;
    }

    public void OnRelease()
    {
        //tail.enabled = true;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawBezier(grabbable.transform.position, closest1.position, grabbable.transform.position, closest1.position, Color.green, null, 3);
        Handles.DrawBezier(grabbable.transform.position, closest2.position, grabbable.transform.position, closest2.position, Color.blue, null, 4);
    }
#endif
}
