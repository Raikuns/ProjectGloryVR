using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FIMSpace.FTail;

public class Hardness : MonoBehaviour
{

    [Header("Target Settings")]
    [SerializeField] GameObject soft;
    [SerializeField] GameObject hard;
    [SerializeField] GameObject target;

    [Header("Speed Of Hard")]
    [SerializeField] float stiff = 0;
    [SerializeField]TailAnimator2 tail;

    private void Start()
    {
        tail = GetComponent<TailAnimator2>();
        target.transform.position = soft.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Box")
        {
            if((stiff * Time.deltaTime) < 1)
            {
                tail.Slithery = 0;
                tail.Curling = 0;
                target.transform.position = Vector3.Lerp(soft.transform.position, hard.transform.position, (stiff * Time.deltaTime));
                stiff++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Box")
        {
            StartCoroutine(TurnFlaccid());
        }
    }

    IEnumerator TurnFlaccid()
    {
        tail.Slithery = 1.1f;
        tail.Curling = 1;
        while(stiff > 0)
        {
            target.transform.position = Vector3.Lerp(soft.transform.position, hard.transform.position, (stiff * Time.deltaTime));
            stiff--;
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

}
