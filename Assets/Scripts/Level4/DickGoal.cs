using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DickGoal : MonoBehaviour
{
    [HideInInspector] public Manager4 manager;
    public float spawnMoveTime;
    public LeanTweenType moveType;

    private void Update()
    {
        transform.LookAt(Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThrowingDick>() != null)
        {
            //Get points and give visual effects of landing a good throw
            manager.GoalHit();

            Destroy(gameObject);
        }
    }

    public void OnSpawn(Vector3 spawnPos, Manager4 newManager)
    {
        manager = newManager;

        transform.position = new Vector3(spawnPos.x, -7, spawnPos.z);

        LeanTween.move(gameObject, spawnPos, spawnMoveTime).setEase(moveType).setOnComplete(Move);
    }

    void Move()
    {
        LeanTween.move(gameObject, manager.GetRandomGoalPosition(), spawnMoveTime)
            .setEase(moveType)
            .setOnComplete(Move)
            .setDelay(manager.goalMoveDelay);
    }
}
