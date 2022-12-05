using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public Level level;

    [HideInInspector]public InteractableColorVisual colorVisual;
    [HideInInspector] public Keypad keypad;

    private void Awake()
    {
        colorVisual = GetComponentInChildren<InteractableColorVisual>();
    }

    public void OnPressed()
    {
        LevelManager.instance.SelectLevel(level);

        keypad.ResetColors();
        colorVisual.InjectOptionalNormalColorState(keypad.selectedColor);
    }
}
