using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Manager4 : MonoBehaviour
{
    public int points = 0;
    public int maxPoints = 200;
    [SerializeField] Transform IKTarget;
    [SerializeField] float dickRange = 0.4f;

    [SerializeField] Transform allPoints;
    [SerializeField] List<Transform> goalPoints = new List<Transform>();

    [SerializeField] GameObject dickGoal;
    public float goalMoveDelay = 3;
    public GameObject newGoal;
    int goalPoint;

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

        UpdatePointsVisual();
    }

    public void ResetThrowingDick()
    {
        throwingDick.ResetPosition();
    }

    public void GoalHit(CFXR_ParticleText pointParticle)
    {
        ResetThrowingDick();

        ParticleSystem particle = pointParticle.GetComponent<ParticleSystem>();

        if (goalPoint >= 4)
        {
            int goalPoints = Random.Range(15, 30);
            GetPoints(goalPoints);

            pointParticle.UpdateText(goalPoints.ToString());
        }
        else
        {
            int goalPoints = Random.Range(5, 10);
            GetPoints(goalPoints);

            pointParticle.UpdateText(goalPoints.ToString());
        }

        particle.Play();

        SpawnGoal();
    }

    public void GetPoints(int incomingPoints)
    {
        points += incomingPoints;

        UpdatePointsVisual();

        if (points >= maxPoints)
        {
            LevelManager.instance.LoadCredits();
        }
    }

    public void RemovePoints(int removedPoints)
    {
        points -= removedPoints;

        UpdatePointsVisual();
    }

    public Transform GetRandomGoalPosition()
    {
        int randomGoal = Random.Range(1, goalPoints.Count);
        goalPoint = randomGoal;
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

    void UpdatePointsVisual()
    {
        IKTarget.localPosition = new Vector3(IKTarget.localPosition.x, Remap(points, 0, maxPoints, -dickRange, dickRange), IKTarget.localPosition.z);
    }

    public float Remap(float input, float from1, float to1, float from2, float to2)
    {
        return (input - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
