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
    [SerializeField] GameObject halo;
    [SerializeField] GameObject wings;
    [Range(0, 1)] public int _;

    [Header("Lose")]
    [SerializeField] Color loseFilter;
    bool loseColor = false;
    [SerializeField] GameObject horns;
    [SerializeField] GameObject pitchfork;
    [Range(0, 1)] public int __;

    [Header("Main")]
    [SerializeField] Color defaultColor;
    [SerializeField] Volume postProcessing;
    [SerializeField] float speed = 1.0f;
    float startTime;
    [SerializeField] float scaleTime;
    [SerializeField] LeanTweenType scaleType;

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

        ScaleHaloAndWings();
    }

    void ScaleHaloAndWings()
    {
        LeanTween.scale(halo, Vector3.one, scaleTime).setEase(scaleType);
        LeanTween.scale(wings, Vector3.one, scaleTime).setEase(scaleType);
    }
    #endregion

    #region Lose
    public void Lose()
    {
        startTime = Time.time;
        loseColor = true;  

        LeanTween.value(0, .3f, 2f).setOnUpdate(UpdateVolumeWeight).setOnComplete(ScaleHornsAndPitchfork);
    }

    void ScaleHornsAndPitchfork()
    {
        LeanTween.scale(horns, Vector3.one, scaleTime).setEase(scaleType);
        LeanTween.scale(pitchfork, Vector3.one, scaleTime).setEase(scaleType);
    }
    #endregion
}
