using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager2 : MonoBehaviour
{
    DildoHandler dildoHandler;
    public float minDelay, maxDelay;

    [Header("Points")]
    public int points = 0;
    public float pointMultiplier = 10;
    public float pointGoal = 500;
    public Slider pointsSlider;
    [SerializeField] LeanTweenType sliderEaseType;

    private void Awake()
    {
        dildoHandler = FindObjectOfType<DildoHandler>();

        if (pointsSlider != null)
            pointsSlider.maxValue = pointGoal;
    }

    private IEnumerator Start()
    {
        dildoHandler.manager = this;
        yield return new WaitForSeconds(2f);
        StartGame();
    }

    void StartGame()
    {
        dildoHandler.GetNewDildo(true);
    }

    public void AddToPoints(float newPoints)
    {
        newPoints *= pointMultiplier;
        float currentPoints = points;

        LeanTween.value(currentPoints, currentPoints + newPoints, 0.7f).setOnUpdate(UpdateSlider).setEase(sliderEaseType);

        currentPoints += newPoints;

        points = Mathf.FloorToInt(currentPoints);
    }

    void UpdateSlider(float value)
    {
        pointsSlider.value = value;
    }
}
