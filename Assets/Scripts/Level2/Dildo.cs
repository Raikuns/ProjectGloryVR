using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dildo : MonoBehaviour
{
    RopeGrabber dick;

    public GameObject moveableDick;
    Vector3 startPos;

    private void Awake()
    {
        dick = GetComponentInChildren<RopeGrabber>();
        dick.dildo = this;

        moveableDick = dick.transform.parent.gameObject;
    }

    private void Start()
    {
        startPos = moveableDick.transform.position;
    }

    public void BackToOrigin()
    {
        LeanTween.move(moveableDick, startPos, 0.2f);
    }

    public void IsChosenDildo()
    {
        StartCoroutine(WaitAndGoBack());
    }

    IEnumerator WaitAndGoBack()
    {
        yield return new WaitForSeconds(3f);
        BackToOrigin();
    }
}
