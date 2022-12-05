using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] GameObject[] creditsList;
    [SerializeField] Vector3 startPos;
    [SerializeField] TextExplosion explos;


    [SerializeField] Vector3 vec3;
    [SerializeField] float _Time;
    [SerializeField] int _index = 0;

    void Start()
    {
        explos = FindObjectOfType<TextExplosion>();
        SpawnText();
    }

    void SpawnText()
    {
        creditsList[_index].transform.position = startPos;
        LeanTween.scale(creditsList[_index], vec3, _Time).setOnComplete(SpawnText).setDelay(2f);
        _index++;
    }

    void Explosion()
    {
        explos.SimpleExplosion();
    }
}
