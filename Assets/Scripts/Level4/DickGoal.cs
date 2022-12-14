using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DickGoal : MonoBehaviour
{
    [HideInInspector] public Manager4 manager;
    public float spawnMoveTime;
    public LeanTweenType moveType;
    [SerializeField] Transform dickPos;

    ParticleSystem particles;

    GameObject throwingDick;

    Collider col;

    CFXR_ParticleText pointParticle;

    private void Start()
    {
        pointParticle = GetComponentInChildren<CFXR_ParticleText>();
        col = GetComponent<Collider>();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        transform.LookAt(Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThrowingDick>() != null)
        {
            //Get points and give visual effects of landing a good throw

            throwingDick = other.GetComponent<ThrowingDick>().gameObject;

            LeanTween.cancel(gameObject);

            LeanTween.move(throwingDick, dickPos.position, 0.5f).setEase(moveType).setOnComplete(MoveDown);

            col.enabled = false;
        }
    }

    public void OnSpawn(Vector3 spawnPos, Manager4 newManager)
    {
        manager = newManager;

        transform.position = new Vector3(spawnPos.x, -7, spawnPos.z);

        LeanTween.move(gameObject, spawnPos, spawnMoveTime).setEase(moveType).setOnComplete(Move);
    }

    void MoveDown()
    {
        throwingDick.transform.localScale = Vector3.zero;
        throwingDick.SetActive(false);

        manager.GoalHit(pointParticle);

        LeanTween.moveY(gameObject, -7, spawnMoveTime).setEase(moveType).setOnComplete(DeleteToilet);
    }

    void DeleteToilet()
    {
        Destroy(gameObject);
    }

    void Move()
    {
        LeanTween.move(gameObject, manager.GetRandomGoalPosition(), spawnMoveTime)
            .setEase(moveType)
            .setOnComplete(Move)
            .setDelay(manager.goalMoveDelay);
    }
}
