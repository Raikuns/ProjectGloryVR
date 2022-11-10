using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DildoHandler : MonoBehaviour
{
    [SerializeField] List<Dildo> dildos = new List<Dildo>();
    [SerializeField] Transform desiredPos;
    Dildo currentDildo;

    [HideInInspector] public Manager2 manager;

    private void Awake()
    {
        var dildosArray = GetComponentsInChildren<Dildo>();

        for (int i = 0; i < dildosArray.Length; i++)
        {
            dildos.Add(dildosArray[i]);
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetNewDildo();
        }
    }

    public void GetNewDildo()
    {
        Dildo newDildo = GetRandomDildo();
        LeanTween.move(newDildo.moveableDick, new Vector3(newDildo.moveableDick.transform.position.x, newDildo.moveableDick.transform.position.y, desiredPos.position.z), 0.2f);

        newDildo.IsChosenDildo();

        dildos.Remove(newDildo);
        dildos.Insert(0, newDildo);
    }

    Dildo GetRandomDildo()
    {
        int randomDildo = Random.Range(1, dildos.Count);
        currentDildo = dildos[randomDildo];
        print(randomDildo);

        return dildos[randomDildo];
    }
}
