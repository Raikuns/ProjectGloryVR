using BNG;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Manager3 : MonoBehaviour
{
    public int points;
    public int desiredPoints = 500;

    public Transform boxPos;
    public List<FallingDick> dicks = new List<FallingDick>();

    public float spawnDelay = 1;
    public bool spawnDicks = false;

    public float delayDecreaseTime;

    HandGrabInteractor[] grabbers;

    [Optional]
    public LeanTweenType dickToGoalCurve;
    public Transform dickGoal;

    private IEnumerator Start()
    {
        grabbers = FindObjectsOfType<HandGrabInteractor>();
        var dicksTemp = GetComponentsInChildren<FallingDick>();
        for (int i = 0; i < dicksTemp.Length; i++)
        {
            dicks.Add(dicksTemp[i]);
            dicksTemp[i].Init(this);
        }

        yield return new WaitForSeconds(3f);

        spawnDicks = true;
        StartCoroutine(SpawnDicks());
    }

    public void Update()
    {
        if (!spawnDicks)
            return;

        spawnDelay -= Time.deltaTime * delayDecreaseTime;

        if (spawnDelay <= 0.1f)
        {
            spawnDelay = 0.1f;
        }
    }

    IEnumerator SpawnDicks()
    {
        while (spawnDicks)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnNewDick();
        }
    }

    private Vector3 RandomPointInBox(Vector3 center, Vector3 size)
    {
        return center + new Vector3(
           (Random.value - 0.5f) * size.x,
           (Random.value - 0.5f) * size.y,
           (Random.value - 0.5f) * size.z
        );
    }

    private FallingDick RandomDick()
    {
        FallingDick randomDick;
        int randomNumber = Random.Range(1, dicks.Count);
        randomDick = dicks[randomNumber];

        dicks.RemoveAt(randomNumber);
        dicks.Insert(0, randomDick);
        return randomDick;
    }

    public void SpawnNewDick()
    {
        FallingDick spawnedDick = RandomDick();

        spawnedDick.gameObject.SetActive(true);
        spawnedDick.transform.position = RandomPointInBox(boxPos.position, boxPos.localScale);
        spawnedDick.transform.rotation = Random.rotation;

        spawnedDick.Appear(true);
    }

    public void CaughtDick()
    {
        AddToPoints();
    }

    public void Missed()
    {
        int removedPoints = Random.Range(10, 30);
        points -= removedPoints;

        if (points <= 0)
        {
            points = 0;
        }
    }

    void AddToPoints()
    {
        int newPoints = Random.Range(30, 50);
        points += newPoints;

        if(points >= desiredPoints)
        {
            LevelManager.instance.LevelCompleted(Level.Heaven);
            spawnDicks = false;

            for (int i = 0; i < dicks.Count; i++)
            {
                if (dicks[i].gameObject.activeInHierarchy)
                {
                    dicks[i].Dissapear(true);
                }
            }

            StopCoroutine(SpawnDicks());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(boxPos.position, boxPos.localScale);
    }

    public void DropItems()
    {
        for (int i = 0; i < grabbers.Length; i++)
        {

        }
    }

    public void MoveDickToGoal(GameObject dick)
    {
        LeanTween.move(dick, dickGoal.position, .5f).setEase(dickToGoalCurve);
    }
}
