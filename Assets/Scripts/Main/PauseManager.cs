using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    HandVisual visual;

    Transform handRoot;
    GameObject button;

    public LeanTweenType buttonScaleType;
    public float buttonScaleSpeed = 0.8f;

    Vector3 difference;
    public float Threshold = 0.1f;
    Header headerParent;
    public GameObject[] headers;

    Transform mainCam;

    bool inMenu = false;

    [SerializeField] LevelData[] levelData;

    LevelManager levelManager;

    private void Start()
    {
        button = transform.GetChild(0).gameObject;
        handRoot = transform.parent;
        visual = handRoot.GetComponentInParent<HandVisual>();
        headerParent = GetComponentInChildren<Header>();

        headerParent.pauseManager = this;

        mainCam = Camera.main.transform;

        if (LevelManager.instance != null)
        {
            levelManager = LevelManager.instance;
            levelManager.pauseManager = this;
        }
        else
        {
            var managerObj = Instantiate(new GameObject());
            managerObj.name = "--== Level Manager ==--";

            LevelManager manager = managerObj.AddComponent<LevelManager>();
            levelManager = manager;
            levelManager.pauseManager = this;

            levelManager.levels = levelData;
        }
    }

    private void Update()
    {
        difference = (handRoot.eulerAngles - Vector3.up).normalized;

        if (difference.z < Threshold)
        {
            if (!button.activeInHierarchy)
                ToggleButton(true);
        }
        else
        {
            if (button.activeInHierarchy)
                ToggleButton(false);
        }
    }

    public void ToggleButton(bool toggle)
    {
        button.SetActive(toggle);
        if (toggle)
        {
            if (button.transform.localScale != Vector3.zero)
            {
                button.transform.localScale = Vector3.zero;
            }

            LeanTween.scale(button, Vector3.one, buttonScaleSpeed).setEase(buttonScaleType);
        }
        else if (!toggle)
        {
            LeanTween.scale(button, Vector3.zero, buttonScaleSpeed).setEase(buttonScaleType).setOnComplete(DisableButton);
        }
    }

    void DisableButton()
    {
        button.SetActive(false);
    }

    public void OnHeaderDisabled()
    {
        inMenu = false;
        visual._updateRootPose = true;
    }

    public void OnButtonPressed()
    {
        if (!inMenu)
        {
            visual._updateRootPose = false;

            headerParent.Appear(headers, 0);

            headerParent.gameObject.transform.position = new Vector3(button.transform.position.x, button.transform.position.y + .15f, button.transform.position.z);
            headerParent.gameObject.transform.LookAt(mainCam);

            inMenu = true;
        }
        else
        {
            headerParent.Dissapear();

            inMenu = false;
        }
    }

    public void OnLevelComplete()
    {
        headerParent.Appear(headers, 1);

        headerParent.gameObject.transform.position = new Vector3(button.transform.position.x, button.transform.position.y + .3f, button.transform.position.z);
        headerParent.gameObject.transform.LookAt(mainCam);

        inMenu = true;
    }

    public void PlaySelectedLevel()
    {
        levelManager.Play();
    }

    public void BackToMain()
    {
        levelManager.BackToMain();
    }
}
