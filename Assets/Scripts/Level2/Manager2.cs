using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager2 : MonoBehaviour
{
    DildoHandler dildoHandler;

    private void Awake()
    {
        dildoHandler = FindObjectOfType<DildoHandler>();
    }

    private IEnumerator Start()
    {
        dildoHandler.manager = this;
        yield return new WaitForSeconds(2f);
        StartGame();
    }

    void StartGame()
    {
        dildoHandler.GetNewDildo();
    }
}
