using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class RopeGrabber : MonoBehaviour
{
    [SerializeField] Transform desiredPos;
    [SerializeField] Transform maxPos;
    [SerializeField] LeanTweenType snapCurve;
    [SerializeField] float snapDuration = 0.5f;
    [SerializeField] bool grabbed = true;
    [SerializeField] HandGrabInteractor grabber;
    Grabbable Grabbable;
    

    PointableUnityEventWrapper eventWrapper;

    private void Start()
    {
        eventWrapper = GetComponent<PointableUnityEventWrapper>();

        eventWrapper.WhenSelect.AddListener(OnGrab);
        eventWrapper.WhenUnselect.AddListener(OnRelease);

        transform.position = desiredPos.position;
    }

    private void Update()
    {
        if (grabbed)
        {
            if (transform.localPosition.x >= maxPos.localPosition.x)
            {
                grabber.Unselect();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnRelease();
        }
    }

    public void OnGrab()
    {
        grabbed = true;
    }

    public void OnRelease()
    {
        LeanTween.move(gameObject, desiredPos, snapDuration).setEase(snapCurve);
        grabbed = false;
    }
}
