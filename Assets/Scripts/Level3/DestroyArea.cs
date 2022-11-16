using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyArea : MonoBehaviour
{
    public Manager3 manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FallingDick>())
        {
            Destroy(other.gameObject);
        }
    }
}
