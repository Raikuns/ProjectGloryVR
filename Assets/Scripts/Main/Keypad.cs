using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    public InteractableColorVisual.ColorState defaultColor;
    public InteractableColorVisual.ColorState selectedColor;
    public InteractableColorVisual.ColorState lockedColor;

    LevelButton[] levelButtons;

    LevelManager manager;
    
    private void Start()
    {
        levelButtons = GetComponentsInChildren<LevelButton>();
        manager = LevelManager.instance;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].keypad = this;

            if (manager.levels[i].unlocked == false)
            {
                levelButtons[i].colorVisual.InjectOptionalNormalColorState(lockedColor);
                levelButtons[i].colorVisual.UpdateVisual();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetColors();
        }
    }

    public void ResetColors()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (manager.levels[i].unlocked)
            {
                levelButtons[i].colorVisual.InjectOptionalNormalColorState(defaultColor);
            }
            else
            {
                levelButtons[i].colorVisual.InjectOptionalNormalColorState(lockedColor);
            }

            levelButtons[i].colorVisual.UpdateVisual();
        }
    }
}
