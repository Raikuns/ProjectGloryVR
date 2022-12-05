using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextExplosion : MonoBehaviour
{
    public float radius = 150.0F;
    public float power = 3000.0F;

    void SimpleExplosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
            }

    }
}
