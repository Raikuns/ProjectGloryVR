using FIMSpace.FTail;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager1 : MonoBehaviour
{
    public static Manager1 instance;

    [Header("Start sequence")]
    public FMODAudioSource zipperAudio;
    public FMODAudioSource pantsDropAudio;

    [SerializeField] HandGrabInteractor[] grabbers;

    [Header("Dicks")]
    [SerializeField] Penis firstDick;
    [SerializeField] Penis secondDick;
    [SerializeField] Penis thirdDick;
    [SerializeField] Transform firstDickPos, secondDickPos, thirdDickPos;

    [SerializeField] float moveTime = 1;
    [SerializeField] LeanTweenType moveCurve;

    [HideInInspector] public CollisionDetection colDetection;
    [HideInInspector] public PlessureMeter plesMeter;
    [HideInInspector] public HandPositionCalculator handPosCalc;

    LevelManager levelManager;

    private void Awake()
    {
        instance = this;

        levelManager = LevelManager.instance;

        colDetection = GetComponentInChildren<CollisionDetection>();
        plesMeter = GetComponentInChildren<PlessureMeter>();
        handPosCalc = GetComponentInChildren<HandPositionCalculator>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        zipperAudio.Play();
        yield return new WaitForSeconds(.5f);
        pantsDropAudio.Play();

        yield return new WaitForSeconds(.5f);
        LeanTween.move(firstDick.gameObject, firstDickPos.position, moveTime).setEase(moveCurve).setOnComplete(TurnOnFirstDickInteraction);
    }

    public void DickEjaculated(int dick)
    {
        DisableGrabbers();

        if (dick == 0)
        {
            TurnOnSecondDick();
        }
        else if (dick == 1)
        {
            TurnOnThirdDick();
        }
        else if (dick == 2)
        {
            //LEVEL COMPLETED
            levelManager.LevelCompleted(Level.WhacADick); 
        }

        EnableGrabbers();
    }

    void TurnOnFirstDickInteraction()
    {
        firstDick.TurnOn();
    }

    void TurnOnSecondDickInteraction()
    {
        secondDick.TurnOn();
    }

    void TurnOnThirdDickInteraction()
    {
        thirdDick.TurnOn();
    }

    void TurnOnSecondDick()
    {
        secondDick.gameObject.SetActive(true);
        LeanTween.move(secondDick.gameObject, secondDickPos.position, moveTime).setEase(moveCurve).setOnComplete(TurnOnSecondDickInteraction);
    }

    void TurnOnThirdDick()
    {
        thirdDick.gameObject.SetActive(true);
        LeanTween.move(thirdDick.gameObject, thirdDickPos.position, moveTime).setEase(moveCurve).setOnComplete(TurnOnThirdDickInteraction);
    }

    void DisableGrabbers()
    {
        for (int i = 0; i < grabbers.Length; i++)
        {
            grabbers[i].Disable();
        }
    }

    void EnableGrabbers()
    {
        for (int i = 0; i < grabbers.Length; i++)
        {
            grabbers[i].Disable();
        }
    }
}
