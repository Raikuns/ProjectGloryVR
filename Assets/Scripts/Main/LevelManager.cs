using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LevelData[] levels;
    public LevelData selectedLevel;

    public static LevelManager instance;

    private void Awake()
    {
        if (FindObjectOfType<LevelManager>() != this)
        {
            Destroy(gameObject);
        }

        instance = this;
    }

    public void SelectLevel(Level level)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].level == level)
            {
                selectedLevel = levels[i];
            }
        }
    }

    public void Play()
    {
        if (selectedLevel == null)
            return;

        SceneManager.LoadScene(selectedLevel.Name);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void LevelCompleted(Level nextLevel)
    {
        UnlockLevel(nextLevel);
        BackToMain();
    }

    public void UnlockLevel(Level level)
    {
        if (level == Level.WhacADick)
        {
            levels[1].unlocked = true;
        }
        else if (level == Level.Dixie)
        {
            levels[2].unlocked = true;
        }
        else if (level == Level.Heaven)
        {
            levels[3].unlocked = true;
        }
    }
}
