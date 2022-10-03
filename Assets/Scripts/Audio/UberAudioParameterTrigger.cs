using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[System.Serializable]
public class EmitterReference
{
    public FMODAudioSource Target;
    public ParamRef[] Params;
}
public class UberAudioParameterTrigger : EventHandler
{
    public EmitterReference[] Emitters;
    public EmitterGameEvent TriggerEvent;
    #region GAME_EVENTS
    protected override void HandleGameEvent(EmitterGameEvent gameEvent)
    {
        if (TriggerEvent == gameEvent)
        {
            TriggerParameters();
        }
    }
    protected override void HandleGameEvent(EmitterGameEvent gameEvent, Collision collision)
    {
        if (TriggerEvent == gameEvent)
        {
            TriggerParameters();
        }
    }
    protected override void HandleGameEvent(EmitterGameEvent gameEvent, Collider collider)
    {
        if (TriggerEvent == gameEvent)
        {
            TriggerParameters();
        }
    }
    #endregion
    public void TriggerParameters()
    {
        for (int i = 0; i < Emitters.Length; i++)
        {
            var emitterRef = Emitters[i];
            if (emitterRef.Target != null && emitterRef.Target.instance.isValid())
            {
                for (int j = 0; j < Emitters[i].Params.Length; j++)
                {
                    emitterRef.Target.instance.setParameterByID(Emitters[i].Params[j].ID, Emitters[i].Params[j].Value);
                }
            }
        }
    }

    public void TriggerParameter(float value)
    {
        for (int i = 0; i < Emitters.Length; i++)
        {
            var emitterRef = Emitters[i];
            if (emitterRef.Target != null && emitterRef.Target.instance.isValid())
            {
                for (int j = 0; j < Emitters[i].Params.Length; j++)
                {
                    emitterRef.Target.instance.setParameterByID(Emitters[i].Params[j].ID, value);
                }
            }
        }
    }
}
