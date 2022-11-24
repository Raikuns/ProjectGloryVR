using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [HideInInspector] public CloudManager manager;

    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    public void StartBounce()
    {
        LeanTween.moveLocalY(gameObject, transform.position.y + manager.moveDist, Random.Range(manager.minTime, manager.maxTime))
            .setEase(manager.moveType)
            .setOnComplete(MoveBackToStart);
    }

    void MoveBackToStart()
    {
        LeanTween.moveLocalY(gameObject, startPos.y, Random.Range(manager.minTime, manager.maxTime))
            .setEase(manager.moveType)
            .setOnComplete(StartBounce);
    }
}
