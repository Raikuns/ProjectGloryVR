using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioEvent
{
    [System.Serializable]
    public struct Event
    {
        public FMODAudioSource m_target;
        public string parameterName;
        public float value;
    }
    public List<Event> events;
    

    public void Invoke() 
    {
        foreach(var e in events)
            e.m_target.SetParameter(e.parameterName, e.value);
       
    }
}
