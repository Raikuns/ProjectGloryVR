using FIMSpace.FTail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.AI;

public class Penis : MonoBehaviour
{
    public HandPositionCalculator handPositionCalculator;
    public SpeedTracker speedTracker;
    public TailAnimator2 tailAnim;
    public PlessureMeter meter;

    [SerializeField] ParticleSystem cumParticle;
    [SerializeField] Transform dickEnd;
    [SerializeField] LeanTweenType moveBackType;

    public UnityEvent onCum;

    Vector3 startPos;

    public bool mirrored = false;
    bool cum = false;

    private void Awake()
    {
        handPositionCalculator = GetComponentInChildren<HandPositionCalculator>();
        speedTracker = GetComponentInChildren<SpeedTracker>();
        tailAnim = GetComponentInChildren<TailAnimator2>();
        meter = GetComponentInChildren<PlessureMeter>();

        speedTracker.meter = meter;
        handPositionCalculator.penis = this;

        speedTracker.gameObject.SetActive(false);
    }

    private void Start()
    {
        startPos = transform.position;  
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onCum.Invoke();
        }

        if (!cum)
            return;

        var localRot = dickEnd.up;


        cumParticle.transform.position = dickEnd.position;
        cumParticle.transform.rotation = Quaternion.LookRotation(localRot);
    }

    public void TurnOn()
    {
        //LeanTween.value(gameObject, 0, 0.1f, 1).setOnUpdate(UpdateWaveSpeed);
        //LeanTween.value(0, 0.4f, 1).setOnUpdate(UpdateWaveRange);

        speedTracker.gameObject.SetActive(true);
    }

    public void Cum()
    {
        cumParticle.Play();
        cum = true;

        LeanTween.value(gameObject, 1, 2, 0.5f).setOnUpdate(UpdateWaveSpeed);
        LeanTween.value(gameObject, 0.1f, 1, 0.5f).setOnUpdate(UpdateWaveRange);

        LeanTween.delayedCall(cumParticle.main.duration, ResetWave);
    }

    void ResetWave()
    {
        LeanTween.value(gameObject, 2, 1, 0.5f).setOnUpdate(UpdateWaveSpeed);
        LeanTween.value(gameObject, 1, 0.1f, 0.5f).setOnUpdate(UpdateWaveRange);

        LeanTween.move(gameObject, startPos, 1f).setEase(moveBackType);
    }

    void UpdateWaveSpeed(float value)
    {
        tailAnim.WavingSpeed = value;
    }

    void UpdateWaveRange(float value)
    {
        tailAnim.WavingRange = value;
    }
}
