using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using BNG;
using FIMSpace.FTail;

public class FallingDick : MonoBehaviour
{
    Rigidbody rb;
    Collider col;
    public float gravityMultiplier = -5;
    [HideInInspector] public Manager3 manager;
    public float appearSpeed = 1f;

    PointableUnityEventWrapper events;
    public LeanTweenType appearCurve;
    TailAnimator2 tail;

    public GameObject[] disableParticles;
    public GameObject[] grabbedParticles;
    bool grabbed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        events = GetComponent<PointableUnityEventWrapper>();
        events.WhenUnselect.AddListener(OnRelease);
        tail = GetComponent<TailAnimator2>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, gravityMultiplier, 0));
    }

    public void Init(Manager3 man)
    {
        manager = man;
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
        tail.enabled = false;
    }

    public void Missed()
    {
        manager.Missed();
        Dissapear(true);
    }

    public void Dissapear(bool turnStatic)
    {
        if (turnStatic)
        {
            rb.isKinematic = turnStatic;
            col.enabled = false;
        }
        LeanTween.scale(gameObject, Vector3.zero, appearSpeed).setOnComplete(DisableMe).setEase(appearCurve);
    }

    public void Appear(bool startStatic)
    {
        grabbed = false;

        if (startStatic)
        {
            ToggleStatic(true);
        }

        LeanTween.scale(gameObject, Vector3.one, appearSpeed).setEase(appearCurve).setOnComplete(TurnDynamic);
    }

    void TurnDynamic()
    {
        ToggleStatic(false);
        tail.enabled = true;
    }

    void ToggleStatic(bool isStatic)
    {
        if (isStatic)
        {
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
            rb.useGravity = true;
        }

        rb.isKinematic = isStatic;
    }

    void DisableMe()
    {
        gameObject.SetActive(false);

        if (!grabbed)
        {
            var obj = Instantiate(disableParticles[Random.Range(0, disableParticles.Length)], transform.position, Quaternion.identity);
            obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
        else
        {
            var obj = Instantiate(grabbedParticles[Random.Range(0, 2)], transform.position, Quaternion.identity);
            obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    //public void OnGrab()
    //{
    //    rb.isKinematic = true;
    //}

    public void OnRelease()
    {
        grabbed = true;

        rb.isKinematic = true;
        rb.useGravity = false;

        manager.MoveDickToGoal(gameObject);
        manager.CaughtDick();

        LeanTween.delayedCall(0.5f, DisableMe);
    }
}
