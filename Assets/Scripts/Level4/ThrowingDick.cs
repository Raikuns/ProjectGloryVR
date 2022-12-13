using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingDick : MonoBehaviour
{
    Vector3 startPos;
    Rigidbody rb;
    [SerializeField] Manager4 manager;
    int currentCredit;

    bool addForce = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (addForce)
            rb.AddForce(new Vector3(0, 5f, 0));
    }

    public void ResetPosition()
    {
        manager.catapult.SetDickParent(transform);

        transform.localPosition = startPos;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = true;
    }

    public void OnGrab()
    {
        addForce = false;
    }

    public void OnRelease()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        addForce = true;
    }
}
