using BNG;
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
    public GameObject dick;

    public float spawnDelay = 1;
    public bool spawnDicks = false;

    public float delayDecreaseTime;

    HandGrabInteractor[] grabbers;

    private IEnumerator Start()
    {
        grabbers = FindObjectsOfType<HandGrabInteractor>();
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

    public void SpawnNewDick()
    {
        GameObject spawnedDick = Instantiate(dick, RandomPointInBox(boxPos.position, boxPos.localScale), Quaternion.identity);
        spawnedDick.GetComponent<FallingDick>().manager = this;
    }

    public void CaughtDick()
    {
        AddToPoints();
    }

    void AddToPoints()
    {
        int newPoints = Random.Range(30, 50);
        points += newPoints;
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
}
