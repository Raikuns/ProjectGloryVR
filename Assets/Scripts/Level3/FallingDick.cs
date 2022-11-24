using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using BNG;

public class FallingDick : MonoBehaviour
{
    Rigidbody rb;
    Collider col;
    public float gravityMultiplier = -5;
    [HideInInspector] public Manager3 manager;
    public float fadeinTime = 1f;

    [SerializeField] MeshRenderer mesh;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>(); 

        LeanTween.value(0, 1, fadeinTime).setOnUpdate(UpdateAlpha);
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, gravityMultiplier, 0));
    }

    void UpdateAlpha(float value)
    {
        mesh.material.color = new Color(1, 1, 1, value);
    }

    public void FadeOut(bool turnStatic)
    {
        if (turnStatic)
        {
            rb.isKinematic = turnStatic;
            col.enabled = false;
        }
        LeanTween.value(1, 0, fadeinTime).setOnUpdate(UpdateAlpha).setOnComplete(DestroyMe);
    }

    void DestroyMe()
    {
        Destroy(gameObject);
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
