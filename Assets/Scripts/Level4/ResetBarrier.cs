using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBarrier : MonoBehaviour
{
    [SerializeField] Manager4 Manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThrowingDick>())
        {
            Manager.ResetThrowingDick();
        }
    }
}
