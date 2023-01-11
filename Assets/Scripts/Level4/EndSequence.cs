using FIMSpace;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using PathCreation;
using PathCreation.Examples;

public class EndSequence : MonoBehaviour
{
    [Header("Win")]
    [SerializeField] Color winFilter;
    bool winColor = false;
    [SerializeField] GameObject halo;
    [SerializeField] GameObject wings;
    [SerializeField] PathCreator winPath;
    [SerializeField] Animator gatesAnim;  
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
    [SerializeField] PathFollower pathFollower;
    [SerializeField] GameObject dick;
    [SerializeField] Animator anim;

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

    void ChangePath(PathCreator path)
    {
        pathFollower.pathCreator = path;
        pathFollower.StartFollowing();

        LeanTween.delayedCall(5, LoadCredits);
    }

    void LoadCredits()
    {
        LevelManager.instance.LoadCredits();
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

        LeanTween.delayedCall(2f, FlyToHeaven);
    }

    void FlyToHeaven()
    {
        anim.SetTrigger("Win");
        gatesAnim.SetTrigger("Open");
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

        LeanTween.delayedCall(2f, DropDownToHell);
    }

    void DropDownToHell()
    {
        anim.SetTrigger("Lose");
    }
    #endregion
}
