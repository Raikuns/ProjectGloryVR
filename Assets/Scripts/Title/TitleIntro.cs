using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleIntro : MonoBehaviour
{
    [Header("Big Dick")]
    [SerializeField] GameObject _BigDick;
    [SerializeField] Vector3 _BigDickLoc;
    [SerializeField] Vector3 _BigDickSpawn;
    [SerializeField] LeanTweenType _DickType;
    [SerializeField] float _DickTime;

    [Header("Title")]
    [SerializeField] GameObject _Title;
    [SerializeField] Vector3 _TitleLoc;
    [SerializeField] Vector3 _TitleSpawn;
    [SerializeField] LeanTweenType _TitleType;
    [SerializeField] float _TitleTime;

    // Start is called before the first frame update
    void Start()
    {
        _Title.transform.position = _TitleSpawn;
        _BigDick.transform.position = _BigDickSpawn;
        BigDick();
    }

    public void BigDick()
    {
        LeanTween.move(_BigDick, _BigDickLoc, _DickTime).setEase(_DickType).setOnComplete(TitleScreen);
    }

    void TitleScreen()
    {
        LeanTween.move(_Title, _TitleLoc, _DickTime).setEase(_TitleType);
    }
}
