using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTracker : MonoBehaviour
{
    public float _desiredSpeed = 5;

    //Refs
    Rigidbody rb;
    PointableUnityEventWrapper eventWrapper;
    Penis penis;
    public PlessureMeter meter;

    //Speed variables
    bool checkForSpeed = false;
    Vector3 lastPosition = Vector3.zero;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        eventWrapper = GetComponent<PointableUnityEventWrapper>();

        eventWrapper.WhenSelect.AddListener(OnGrab);
        eventWrapper.WhenUnselect.AddListener(OnRelease);

        meter.desiredSpeed = _desiredSpeed;
    }

    private void FixedUpdate()
    {
        if (!checkForSpeed)
            return;

        float rawSpeed = (transform.position - lastPosition).magnitude / Time.fixedDeltaTime;
        lastPosition = transform.position;

        meter.speed = rawSpeed * 10;
    }

    public void OnGrab()
    {
        checkForSpeed = true;

        meter.isGrabbed = true;
    }

    public void OnRelease()
    {
        checkForSpeed = false;

        meter.isGrabbed = false;
    }
}
