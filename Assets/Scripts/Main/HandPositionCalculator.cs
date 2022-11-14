using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using FIMSpace.FTail;
using UnityEditor;
using System.Linq;

public class HandPositionCalculator : MonoBehaviour
{
    [SerializeField] Transform baseBone;
    [SerializeField] Vector3 offset;
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
        GetClosest();
    }

    private void LateUpdate()
    {
        //LeanTween.moveZ(bones[0].gameObject, grabbable.position.z, 0f);
        //LeanTween.moveY(bones[0].gameObject, grabbable.position.y, 0f);

        DickDirection();

        //LeanTween.rotate(bones[0].gameObject, grabbable.eulerAngles, 0f);
    }

    void DickDirection()
    {
        Vector3 dir = (baseBone.position - grabbable.position).normalized;

        print(dir);

        baseBone.rotation = Quaternion.LookRotation(-dir);
        Debug.DrawRay(baseBone.position, -dir, Color.green, 1);
        Debug.DrawRay(baseBone.position, baseBone.eulerAngles, Color.red, 2);

        Vector3 handDir = dir + offset;

        grabbable.rotation = Quaternion.LookRotation(-handDir);
    }

    void SetMinAndMaxPoints()
    {
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
        grabbable.transform.position = new Vector3(x, lerpedY, grabbable.transform.position.z);
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
        lastClosest = bones[0];
        bones = bones.OrderBy((d) => (d.position - grabbable.position).sqrMagnitude).ToArray();

        if (bones[0] != lastClosest)
        {
            SetY();
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
        Handles.DrawBezier(grabbable.transform.position, bones[0].position, grabbable.transform.position, bones[0].position, Color.green, null, 3);
        Handles.DrawBezier(grabbable.transform.position, bones[1].position, grabbable.transform.position, bones[1].position, Color.blue, null, 4);
    }
#endif
}
