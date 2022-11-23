using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Level")]
public class LevelData : ScriptableObject
{
    public string Name;
    public Level level;
}

public enum Level
{
    GloryHole,
    WhacADick,
    Dixie,
    Heaven
}
