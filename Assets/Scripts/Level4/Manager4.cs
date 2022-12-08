using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Manager4 : MonoBehaviour
{
    public int points = 0;

    [SerializeField] Transform allPoints;
    [SerializeField] List<Transform> goalPoints = new List<Transform>();

    [SerializeField] GameObject dickGoal;
    public float goalMoveDelay = 3;

    public GameObject newGoal;

    ThrowingDick throwingDick;

    public Catapult catapult;

    private void Start()
    {
        throwingDick = FindObjectOfType<ThrowingDick>();

        Transform[] positions = allPoints.GetComponentsInChildren<Transform>();

        for (int i = 0; i < positions.Length; i++)
        {
            if (positions[i].childCount <= 0)
            {
                goalPoints.Add(positions[i]);
            }
        }

        LeanTween.delayedCall(2f, SpawnGoal);
    }

    public void ResetThrowingDick()
    {
        throwingDick.ResetPosition();
    }

    public void GoalHit()
    {
        ResetThrowingDick();
        GetPoints(10);
        SpawnGoal();
    }

    public void GetPoints(int incomingPoints)
    {
        points += incomingPoints;
        if(points >= 100)
        {
            LevelManager.instance.LoadCredits();
        }
    }

    public Transform GetRandomGoalPosition()
    {
        int randomGoal = Random.Range(1, goalPoints.Count);
        Transform newPoint = goalPoints[randomGoal];

        goalPoints.Remove(newPoint);
        goalPoints.Insert(0, newPoint);

        return newPoint;
    }

    void SpawnGoal()
    {
        Transform spawnPos = GetRandomGoalPosition();
        newGoal = Instantiate(dickGoal);

        newGoal.GetComponent<DickGoal>().OnSpawn(spawnPos.position, this);
    }
}
