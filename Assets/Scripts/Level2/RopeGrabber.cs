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
    [SerializeField] HandGrabInteractor[] grabbers;
    [SerializeField] HandGrabInteractor heldByGrabber = null;

    HandGrabInteractable[] grabbables;

    PointableUnityEventWrapper eventWrapper;

    [HideInInspector]public Dildo dildo;

    private void Start()
    {
        grabbables = GetComponentsInChildren<HandGrabInteractable>();
        grabbers = FindObjectsOfType<HandGrabInteractor>();

        eventWrapper = GetComponent<PointableUnityEventWrapper>();

        eventWrapper.WhenSelect.AddListener(OnGrab);
        eventWrapper.WhenUnselect.AddListener(OnRelease);

        eventWrapper.WhenSelect.AddListener(dildo.PausePointsDecrease);

        transform.position = desiredPos.position;
    }

    private void Update()
    {
        if (grabbed)
        {
            if (transform.localPosition.x >= maxPos.localPosition.x)
            {
                heldByGrabber.Disable();
                dildo.Pulled();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnRelease();
        }
    }

    public void OnGrab()
    {
        dildo.grabbedPos = transform.position;

        for (int y = 0; y < grabbables.Length; y++)
        {
            for (int i = 0; i < grabbers.Length; i++)
            {
                if (grabbers[i].SelectedInteractable == grabbables[y])
                {
                    heldByGrabber = grabbers[i];
                }
            }
        }

        grabbed = true;
    }

    public void OnRelease()
    {
        heldByGrabber.Enable();
        heldByGrabber = null;
        LeanTween.move(gameObject, desiredPos, snapDuration).setEase(snapCurve);
        grabbed = false;
    }
}
