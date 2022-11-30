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
}
