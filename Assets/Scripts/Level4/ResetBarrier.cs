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
            other.gameObject.transform.localScale = Vector3.zero;
            other.gameObject.SetActive(false);
            Manager.ResetThrowingDick();
            Manager.RemovePoints(Random.Range(5, 10));
        }
    }
}
