using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingDick : MonoBehaviour
{
    Vector3 startPos;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        startPos = transform.position;  
    }

    public void ResetPosition()
    {
        transform.position = startPos;
        transform.rotation = Quaternion.identity;
        rb.isKinematic = true;
    }
}
