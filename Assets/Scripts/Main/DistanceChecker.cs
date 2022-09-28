using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    public Transform bone2;
    public Transform bone3;

    public Transform hand;

    private void Update()
    {
        print(Vector3.Distance(bone2.position, hand.position) + " bone 2");
        print(Vector3.Distance(bone3.position, hand.position) + " bone 3");
    }
}
