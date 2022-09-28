using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager1 : MonoBehaviour
{
    public static Manager1 instance;

    [HideInInspector] public CollisionDetection colDetection;
    [HideInInspector] public PlessureMeter plesMeter;
    [HideInInspector] public HandPositionCalculator handPosCalc;

    private void Awake()
    {
        instance = this;

        colDetection = GetComponentInChildren<CollisionDetection>();
        plesMeter = GetComponentInChildren<PlessureMeter>();
        handPosCalc = GetComponentInChildren<HandPositionCalculator>();
    }
}
