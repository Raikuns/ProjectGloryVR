using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlessureMeter : MonoBehaviour
{
    [Header("Speed variables")]
    public float speed;
    public float desiredSpeed = 5;

    [Header("Meter")]
    public float meter = 0;
    public float min, max;
    public Slider meterSlider;

    [Range(0, 10)]
    public float plessureMultiplier = 10;
    float maxPlessure = 100;

    public bool isGrabbed = false;

    Penis penis;


    private void Start()
    {
        penis = GetComponentInParent<Penis>();

        meterSlider.minValue = min;
        meterSlider.maxValue = max;
    }

    private void Update()
    {
        if (isGrabbed)
        {
            float diff = Mathf.Abs(speed - desiredSpeed);
            plessureMultiplier = maxPlessure / diff;

            if (plessureMultiplier > maxPlessure)
            {
                plessureMultiplier = maxPlessure;
            }
            else if (plessureMultiplier < 0)
            {
                plessureMultiplier = 0;
            }

            if (speed > 1)
            {
                meter += plessureMultiplier * Time.deltaTime;
            }
        }
        else
        {
            meter -= Time.deltaTime / 2;
        }

        if (meter < min)
        {
            meter = min;
        }
        else if (meter > max)
        {
            Ejaculate();
            meter = max;
        }

        meterSlider.value = meter;
    }

    void Ejaculate()
    {
        meter = 0;
        penis.onCum.Invoke();
    }
}
