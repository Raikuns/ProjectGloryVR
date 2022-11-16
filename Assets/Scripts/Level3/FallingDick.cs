using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using BNG;

public class FallingDick : MonoBehaviour
{
    Rigidbody rb;
    public float gravityMultiplier = -5;
    [HideInInspector] public Manager3 manager;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, gravityMultiplier, 0));
    }

    public void OnGrab()
    {
        manager.DropItems();
        rb.isKinematic = true;
        manager.CaughtDick();

        //fade out GFX
        Destroy(gameObject);
    }
}
