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

    [SerializeField] Transform penisLook;

    Vector3 startPos;

    [HideInInspector] public Vector3 grabbedPos;

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
            if (availablePoints < 1)
            {
                BackToOrigin();
            }
        }

        MoveDickAccordingly();
    }

    void MoveDickAccordingly()
    {
        Vector3 diff = dick.transform.position - grabbedPos;
        print(diff);

        if (diff.x < 0)
        {
            penisLook.position = new Vector3(penisLook.position.x - diff.x, penisLook.position.y, penisLook.position.z);
        }
        else if (diff.y > 0)
        {
            penisLook.position = new Vector3(penisLook.position.x + diff.x, penisLook.position.y, penisLook.position.z);
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
