using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dildo : MonoBehaviour
{
    RopeGrabber dick;

    public float availablePoints = 10;
    float basePoints;
    bool decreasePoints = false;

    public GameObject moveableDick;
    [HideInInspector] public DildoHandler handler;

    Vector3 startPos;

    private void Awake()
    {
        basePoints = availablePoints;
        dick = GetComponentInChildren<RopeGrabber>();
        dick.dildo = this;

        moveableDick = dick.transform.parent.gameObject;
    }

    private void Start()
    {
        startPos = moveableDick.transform.position;
    }

    private void Update()
    {
        if (decreasePoints)
        {
            availablePoints -= Time.deltaTime;
            if(availablePoints < 1)
            {
                BackToOrigin();
            }
        }
    }

    public void BackToOrigin()
    {
        LeanTween.move(moveableDick, startPos, 0.2f).setOnComplete(GetNewDildo);
        decreasePoints = false; 
    }

    void GetNewDildo()
    {
        availablePoints = basePoints;
        handler.GetNewDildo(false);
    }

    public void IsChosenDildo(DildoHandler dildoHandler)
    {
        handler = dildoHandler;
        decreasePoints = true;
    }

    public void PausePointsDecrease()
    {
        decreasePoints = false;
    }

    public void Pulled()
    {
        handler.OnDildoPulled(availablePoints);
        BackToOrigin();
    }
}
