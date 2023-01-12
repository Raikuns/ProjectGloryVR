using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Header : MonoBehaviour
{
    [HideInInspector] public PauseManager pauseManager;

    public LeanTweenType appearType;
    public float appearSpeed = 0.5f;

    private void Start()
    {
        Dissapear();
    }

    public void Appear(GameObject[] headers, int activateHeader)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            if(i == activateHeader)
            {
                headers[i].SetActive(true);
            }
            else
            {
                headers[i].SetActive(false); 
            }
        }

        EnableMe();
        LeanTween.scale(gameObject, Vector3.one, appearSpeed).setEase(appearType);
    }

    public void Dissapear()
    {
        LeanTween.scale(gameObject, Vector3.zero, appearSpeed).setEase(appearType).setOnComplete(DisableMe);
    }

    void EnableMe()
    {
        gameObject.SetActive(true);
    }

    void DisableMe()
    {
        pauseManager.OnHeaderDisabled();
        gameObject.SetActive(false);
    }
}
