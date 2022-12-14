using FIMSpace;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EndSequence : MonoBehaviour
{
    [Header("Win")]
    [SerializeField] Color winFilter;
    bool winColor = false;
    [Range(0, 1)] public int _;

    [Header("Lose")]
    [SerializeField] Color loseFilter;
    bool loseColor = false;
    [Range(0, 1)] public int __;

    [Header("Main")]
    [SerializeField] Color defaultColor;
    [SerializeField] Volume postProcessing;
    [SerializeField] float speed = 1.0f;
    float startTime;

    #region Main
    private void Start()
    {
        RenderSettings.skybox = new Material(RenderSettings.skybox);
    }

    private void Update()
    {
        if (loseColor)
        {
            float t = (Time.time - startTime) * speed;
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(defaultColor, loseFilter, t));
        }
        else if (winColor)
        {
            float t = (Time.time - startTime) * speed;
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(defaultColor, winFilter, t));
        }
    }

    void UpdateVolumeWeight(float value)
    {
        postProcessing.weight = value;
    }
    #endregion

    #region Win
    public void Win()
    {
        startTime = Time.time;
    }
    #endregion

    #region Lose
    public void Lose()
    {
        startTime = Time.time;
        loseColor = true;  

        LeanTween.value(0, .3f, 2f).setOnUpdate(UpdateVolumeWeight);
    }
    #endregion
}
