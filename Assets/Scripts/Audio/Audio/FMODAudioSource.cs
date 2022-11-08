using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;
using UnityEngine.Audio;
using UnityEditor;
public class FMODAudioSource : FMODUnity.EventHandler
{
    #region Private
    public float SPEEDOFSOUND = 343;
    protected FMOD.Studio.EventDescription eventDescription;
    public FMOD.Studio.EventInstance instance;
    private float distanceToListener;
    private AudioSource legacyAudioSource;
    #endregion
    public EmitterGameEvent PlayEvent = EmitterGameEvent.None;
    public EmitterGameEvent StopEvent = EmitterGameEvent.None;    
    #region LEGACY
    public bool m_UnityAudio = false;
    public bool hasSetAudioSource;
    [System.Serializable]
    public struct AudioSourceSettings
    {
        public bool isSet;
        public AudioClip clip;
        public AudioMixerGroup output;
        public bool mute, bypassEffects, bypassListenerEffects, bypassReverbZones, playOnAwake, loop;
        public int priority;
        public float volume;
        public float pitch;
        public float stereoPan;
        public float spatialBlend;
        public float reverbZoneMix;
        public float dopplerLevel;
        public float spread;
        public float minDistance, maxDistance;
    }
    public AudioSourceSettings audioSourceSettings;
    #endregion
    #region FMOD
    public EventReference fmodEvent;
    [Obsolete("Use the EventReference field instead")]
    public string Event = "";

    public ParamRef[] Params = new ParamRef[0];

    [Tooltip("Delay the sound based on the distance of the listener.")]
    public bool useSpeedOfSound;
    public bool debugPlay;
    public bool overrideAttenuation;
    public float minAtten = 10, maxAtten = 50;
    public List<ParamRef> cachedParams = new List<ParamRef>();

    [SerializeField]
    private bool occlusionEnabled = false;
    [HideInInspector]
    public string occlusionParameterName = null;
    [Range(0.0f, 3.0f)]
    [SerializeField]
    private float occlusionIntensity = 0.1f;

    private float currentOcclusion = 0.0f;
    private float nextOcclusionUpdate = 0.0f;
    [HideInInspector]
    public int paramIndex = 0,prevIndex;
    #endregion
    public bool logEvents;
    #region UNITY_AUDIO
    public AudioClip m_AudioClip;
    #endregion
    
    #region API
    public void Play()
    {
        if (m_UnityAudio && legacyAudioSource)
            PlayAudioLegacy();
        else
            PlayAudioFMOD();
    }
    public void PlayDelayed(float delay)
    {
        StartCoroutine(_PlayDelayed(delay));
    }
    public void Stop()
    {
        if (m_UnityAudio && legacyAudioSource)
            legacyAudioSource.Stop();
        //TODO: FMOD Stop Instance
    }
    #endregion
    private void Update()
    {
        UpdateOcclusion();
        if (debugPlay)
        {
            debugPlay = false;
            PlayAudioFMOD();
        }
    }
    //[MenuItem("GameObject/Audio/Add Uber Source", false, 10)]
    //static void CreateCustomGameObject(MenuCommand menuCommand)
    //{
    //    // Create a custom game object
    //    GameObject go = new GameObject("Uber Audio Source");
    //    // Ensure it gets reparented if this was a context click (otherwise does nothing)
    //    GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
    //    // Register the creation in the undo system
    //    Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
    //    Selection.activeObject = go;

    //    go.AddComponent<FMODAudioSource>();
    //}


    private IEnumerator _PlayDelayed(float delay)
    {
        if (m_UnityAudio && legacyAudioSource)
        {
            yield return new WaitForSeconds(delay);
            PlayAudioLegacy();
        }

        else
        {
            yield return new WaitForSeconds(delay);
            PlayAudioFMOD();
        }
    }
    
    private void PlayAudioLegacy()
    {
        if (useSpeedOfSound)
        {
            AudioListener listener = GameObject.FindObjectOfType<AudioListener>();
            if (!listener)
                return;

            Vector3 listenerPos = listener.transform.position;
            distanceToListener = Vector3.Distance(this.transform.position, listenerPos);
            float delay = (distanceToListener / SPEEDOFSOUND);
            StartCoroutine(PlayLegacyAudioWithDelay(delay));
        }
    }
    private IEnumerator PlayLegacyAudioWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        legacyAudioSource.Play();
    }
    #region FMOD
    private void PlayAudioFMOD(bool getSurface = false,string physicsMaterialName="")
    {
        if (useSpeedOfSound)
        {
            StudioListener listener = GameObject.FindObjectOfType<StudioListener>();
            if (!listener)
                return;

            Vector3 listenerPos = listener.transform.position;
            distanceToListener = Vector3.Distance(this.transform.position, listenerPos);
            StartCoroutine(PlayFMODAudioWithDelay(fmodEvent));
        }
        else
        {
            instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
            instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
            if (overrideAttenuation) { SetFMODAttenuation(); }
            if (getSurface)
                instance.setParameterByNameWithLabel("Surface", physicsMaterialName);
            instance.start();
        }
    }
    private IEnumerator PlayFMODAudioWithDelay(EventReference eventRef)
    {
        float delay = (distanceToListener / SPEEDOFSOUND);
        print($"Playing... waiting for {delay} seconds. Distance: {distanceToListener}");
        yield return new WaitForSeconds(delay);
        instance = FMODUnity.RuntimeManager.CreateInstance(eventRef);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
        if (overrideAttenuation) { SetFMODAttenuation(); }
        instance.start();
    }

    private void SetFMODAttenuation()
    {
        instance.setProperty(FMOD.Studio.EVENT_PROPERTY.MINIMUM_DISTANCE, minAtten);
        instance.setProperty(FMOD.Studio.EVENT_PROPERTY.MAXIMUM_DISTANCE, maxAtten);
    }

    void UpdateOcclusion()
    {
        if (instance.isValid())
        {
            instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
            instance.setProperty(FMOD.Studio.EVENT_PROPERTY.MINIMUM_DISTANCE, minAtten);
            instance.setProperty(FMOD.Studio.EVENT_PROPERTY.MAXIMUM_DISTANCE, maxAtten);
            if (!occlusionEnabled)
            {
                currentOcclusion = 0.0f;
            }
            else if (Time.time >= nextOcclusionUpdate)
            {
                nextOcclusionUpdate = Time.time + FMODUnityResonance.FmodResonanceAudio.occlusionDetectionInterval;
                currentOcclusion = occlusionIntensity * FMODUnityResonance.FmodResonanceAudio.ComputeOcclusion(transform);
                instance.setParameterByName(occlusionParameterName, currentOcclusion);
            }
        }
    }

    protected override void HandleGameEvent(EmitterGameEvent gameEvent)
    {
        if (gameEvent != PlayEvent)
            return;
                Play();
        if (logEvents)
            print($"Event handled: {gameEvent}");
    }
    protected override void HandleGameEvent(EmitterGameEvent gameEvent, Collider collider)
    {
        if (gameEvent != PlayEvent)
            return;
        if (m_UnityAudio)
            Play();
        else
        {
            PhysicMaterial pM = collider.GetComponent<PhysicMaterial>();
            if (pM)
                PlayAudioFMOD(true, pM.name);
            else
                PlayAudioFMOD(false);
        }
        if (logEvents)
            print($"Event handled: {gameEvent}");
    }
    protected override void HandleGameEvent(EmitterGameEvent gameEvent, Collision collision)
    {
        if (gameEvent != PlayEvent)
            return;
        if (m_UnityAudio)
            Play();
        else
        {
            PhysicMaterial pM = collision.gameObject.GetComponent<PhysicMaterial>();
            if (pM)
                PlayAudioFMOD(true, pM.name);
            else
                PlayAudioFMOD(false);
        }
        if (logEvents)
            print($"Event handled: {gameEvent}");
    }

    public void SetParameter(string param, float value)
    {
        if (instance.isValid())
        {
          instance.setParameterByName(param, value);
        }
    }
    #endregion
}
