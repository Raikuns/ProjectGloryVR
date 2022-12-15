using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BackGroundAudioRegulator : MonoBehaviour
{
    FMODAudioSource audioSource;

    [HideInInspector] public Manager4 manager;
    public float param;

    private void Start()
    {
        audioSource = GetComponent<FMODAudioSource>();
    }

    public void AdjustBalance()
    {
        float value = 1 - manager.Remap(manager.points, 0, manager.maxPoints, 0, 1) - 0.2f;

        LeanTween.value(param, value, 1f).setOnUpdate(UpdateParam);
    }

    void UpdateParam(float value)
    {
        audioSource.SetParameter("Dying", value);
        audioSource.instance.getParameterByName("Dying", out param);
    }
}
