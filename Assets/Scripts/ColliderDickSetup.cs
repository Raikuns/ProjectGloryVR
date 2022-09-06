using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FIMSpace.FTail;


public class ColliderDickSetup : MonoBehaviour
{
    TailAnimator2[] tails;
    [SerializeField] private Collider[] cols;

    public void Start()
    {
        Invoke("FindColliders",3f);
    }
    public void FindColliders()
    {
        tails = FindObjectsOfType<TailAnimator2>();

        cols = transform.GetComponentsInChildren<Collider>();

        for (int i = 0; i < tails.Length; i++)
        {
            for (int j = 0; j < cols.Length; j++)
            {
                tails[i].AddCollider(cols[j]);
            }
        }
    } 
}
