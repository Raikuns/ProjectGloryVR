using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dick : MonoBehaviour
{
    Rigidbody rb;
    PointableUnityEventWrapper eventWrapper;

    bool checkForSpeed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        eventWrapper = GetComponent<PointableUnityEventWrapper>();

        eventWrapper.WhenSelect.AddListener(OnGrab);
        eventWrapper.WhenUnselect.AddListener(OnRelease);
    }

    private void Update()
    {
        if (!checkForSpeed)
            return;
    }

    public void OnGrab()
    {
        checkForSpeed = true;
    }

    public void OnRelease()
    {
        checkForSpeed = false;
    }
}
