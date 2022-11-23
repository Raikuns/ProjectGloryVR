using FIMSpace.FTail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Penis : MonoBehaviour
{
    public HandPositionCalculator handPositionCalculator;
    public SpeedTracker speedTracker;
    public TailAnimator2 tailAnim;
    public PlessureMeter meter;

    public UnityEvent onCum;

    public bool mirrored = false;

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

    public void TurnOn()
    {
        //LeanTween.value(gameObject, 0, 0.1f, 1).setOnUpdate(UpdateWaveSpeed);
        //LeanTween.value(0, 0.4f, 1).setOnUpdate(UpdateWaveRange);

        speedTracker.gameObject.SetActive(true);
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
