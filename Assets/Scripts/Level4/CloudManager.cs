using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public LeanTweenType moveType;

    public float moveDist;

    [Header("Travel time(speed)")]
    public float minTime, maxTime;

    public Transform cloudsParent;
    public Cloud[] clouds;

    private void Start()
    {
        clouds = cloudsParent.GetComponentsInChildren<Cloud>();

        BounceCloud();
    }

    void BounceCloud()
    {
        for (int i = 0; i < clouds.Length; i++)
        {
            clouds[i].manager = this;
            clouds[i].StartBounce();
        }
    }
}
