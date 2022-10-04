using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using FMODUnity;
using Unity.VisualScripting;

[CustomEditor(typeof(FMODAudioSource))]
[CanEditMultipleObjects]
public class FMODAudioSourceEditor : Editor
{
    private ParameterValueView parameterValueView;
    private FMODAudioSource source;
    
    public void OnEnable()
    {
        parameterValueView = new ParameterValueView(serializedObject);
    }
    /// <summary>
    /// Min Max Attenuation handles.
    /// First checks if override attenuation is true.
    /// If true, attenuation is controlled by the values of the FMODAudioSource.
    /// Values can be changed with the Handles in the scene view, or with the parameters.
    /// </summary>
    public void OnSceneGUI()
    {

        var emitter = target as FMODAudioSource;
        if (emitter.m_UnityAudio)
            return;

        EditorEventRef editorEvent = EventManager.EventFromGUID(emitter.fmodEvent.Guid);
        if (editorEvent != null && editorEvent.Is3D)
        {
            EditorGUI.BeginChangeCheck();
            float minDistance = emitter.overrideAttenuation ? emitter.minAtten : editorEvent.MinDistance;
            float maxDistance = emitter.overrideAttenuation ? emitter.maxAtten : editorEvent.MaxDistance;
            Handles.color = Color.cyan;
            minDistance = Handles.RadiusHandle(Quaternion.identity, emitter.transform.position, minDistance);
            maxDistance = Handles.RadiusHandle(Quaternion.identity, emitter.transform.position, maxDistance);
            if (EditorGUI.EndChangeCheck() && emitter.overrideAttenuation)
            {
                Undo.RecordObject(emitter, "Change Emitter Bounds");
                emitter.minAtten = Mathf.Clamp(minDistance, 0, emitter.maxAtten);
                emitter.maxAtten = Mathf.Max(emitter.minAtten, maxDistance);
            }
        }
    }
    public override void OnInspectorGUI()
    {
        
        var legacyAudio = serializedObject.FindProperty("m_UnityAudio");
        var audioComp = target as FMODAudioSource;
        source = audioComp;
        var useSpeedOfSound = serializedObject.FindProperty("useSpeedOfSound");
        var speedOfSound = serializedObject.FindProperty("SPEEDOFSOUND");

        var begin = serializedObject.FindProperty("PlayEvent");
        var end = serializedObject.FindProperty("StopEvent");
        var tag = serializedObject.FindProperty("CollisionTag");
        var logEv = serializedObject.FindProperty("logEvents");
        EditorGUILayout.PropertyField(begin, new GUIContent("Play Event"));
        EditorGUILayout.PropertyField(end, new GUIContent("Stop Event"));

        if ((begin.enumValueIndex >= (int)EmitterGameEvent.TriggerEnter && begin.enumValueIndex <= (int)EmitterGameEvent.TriggerExit2D) ||
        (end.enumValueIndex >= (int)EmitterGameEvent.TriggerEnter && end.enumValueIndex <= (int)EmitterGameEvent.TriggerExit2D))
        {
            tag.stringValue = EditorGUILayout.TagField("Collision Tag", tag.stringValue);
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(legacyAudio, new GUIContent("Legacy Audio"));
        EditorGUILayout.PropertyField(logEv, new GUIContent("Log Events"));
        EditorGUI.EndChangeCheck();

        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(useSpeedOfSound);
        EditorGUI.BeginDisabledGroup(!useSpeedOfSound.boolValue);
        EditorGUIUtility.labelWidth = 100;
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(speedOfSound, new GUIContent("M/s"));
        if (EditorGUI.EndChangeCheck())
        {
            speedOfSound.floatValue = Mathf.Clamp(speedOfSound.floatValue, 0, Mathf.Infinity);
        }
        EditorGUIUtility.labelWidth = 0;
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        if (legacyAudio.boolValue)
        {
            if (!audioComp.GetComponent<AudioSource>())
                AddLegacySource(audioComp);
            LegacyEditor();
            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            var get = audioComp.GetComponent<AudioSource>();
            if (get)
                RemoveLegacySource(get, audioComp);
            FmodEditor(audioComp);
            serializedObject.ApplyModifiedProperties();
        }
        DebugButtons();
    }
    private void RemoveLegacySource(AudioSource get, FMODAudioSource from)
    {
        var hasSet = serializedObject.FindProperty("hasSetAudioSource");
        hasSet.boolValue = true;
        serializedObject.ApplyModifiedProperties();
        var settings = from.audioSourceSettings;

        settings.clip                   = from.m_AudioClip;
        settings.output                 = get.outputAudioMixerGroup;
        settings.mute                   = get.mute;
        settings.bypassEffects          = get.bypassEffects;
        settings.bypassListenerEffects  = get.bypassListenerEffects;
        settings.bypassReverbZones      = get.bypassReverbZones;
        settings.playOnAwake            = get.playOnAwake;
        settings.loop                   = get.loop;
        settings.priority               = get.priority;
        settings.volume                 = get.volume;
        settings.pitch                  = get.pitch;
        settings.stereoPan              = get.panStereo;
        settings.spatialBlend           = get.spatialBlend;
        settings.reverbZoneMix          = get.reverbZoneMix;
        settings.dopplerLevel           = get.dopplerLevel;
        settings.spread                 = get.spread;
        settings.minDistance            = get.minDistance;
        settings.maxDistance            = get.maxDistance;
        settings.isSet                  = true;
        from.audioSourceSettings = settings;
        DestroyImmediate(get);
    }
    private void AddLegacySource(FMODAudioSource from)
    {
        var get = from.AddComponent<AudioSource>();
        if (!from.hasSetAudioSource)
        {
            return;
        }
            
        var settings = from.audioSourceSettings;
        get.clip                   = from.m_AudioClip;
        get.outputAudioMixerGroup  = settings.output;
        get.mute                   = settings.mute;
        get.bypassEffects          = settings.bypassEffects;
        get.bypassListenerEffects  = settings.bypassListenerEffects;
        get.bypassReverbZones      = settings.bypassReverbZones;
        get.playOnAwake            = settings.playOnAwake;
        get.loop                   = settings.loop;
        get.priority               = settings.priority;
        get.volume                 = settings.volume;
        get.pitch                  = settings.pitch;
        get.panStereo              = settings.stereoPan;
        get.spatialBlend           = settings.spatialBlend;
        get.reverbZoneMix          = settings.reverbZoneMix;
        get.dopplerLevel           = settings.dopplerLevel;
        get.spread                 = settings.spread;
        get.minDistance            = settings.minDistance;
        get.maxDistance            = settings.maxDistance;


    }
    private void FmodEditor(FMODAudioSource src)
    {
        
        var eventReference = serializedObject.FindProperty("fmodEvent");
        var eventPath = eventReference.FindPropertyRelative("Path");
        var overrideAtt = serializedObject.FindProperty("overrideAttenuation");
        var minDistance = serializedObject.FindProperty("minAtten");
        var maxDistance = serializedObject.FindProperty("maxAtten");
        var useOcclusion = serializedObject.FindProperty("occlusionEnabled");
        var occlusionParameterName = serializedObject.FindProperty("occlusionParameterName");
        var occlusionIntensity = serializedObject.FindProperty("occlusionIntensity");

        EditorGUI.BeginChangeCheck();

        const string EventReferenceLabel = "Event";

        EditorUtils.DrawLegacyEvent(serializedObject.FindProperty("Event"), EventReferenceLabel);

        EditorGUILayout.PropertyField(eventReference, new GUIContent(EventReferenceLabel));

        EditorEventRef editorEvent = EventManager.EventFromPath(eventPath.stringValue);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtils.UpdateParamsOnEmitter(serializedObject, eventPath.stringValue);
        }

        // Attenuation
        if (editorEvent != null)
        {
            
            {
                EditorGUI.BeginDisabledGroup(editorEvent == null || !editorEvent.Is3D);
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(overrideAtt);
                if (EditorGUI.EndChangeCheck() ||
                    (minDistance.floatValue == -1 && maxDistance.floatValue == -1) // never been initialiased
                    )
                {
                    minDistance.floatValue = editorEvent.MinDistance;
                    maxDistance.floatValue = editorEvent.MaxDistance;
                }
                EditorGUI.BeginDisabledGroup(!overrideAtt.boolValue);
                EditorGUIUtility.labelWidth = 30;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(minDistance, new GUIContent("Min"));
                if (EditorGUI.EndChangeCheck())
                {
                    minDistance.floatValue = Mathf.Clamp(minDistance.floatValue, 0, maxDistance.floatValue);
                }
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(maxDistance, new GUIContent("Max"));
                if (EditorGUI.EndChangeCheck())
                {
                    maxDistance.floatValue = Mathf.Max(minDistance.floatValue, maxDistance.floatValue);
                }
                EditorGUIUtility.labelWidth = 0;
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
                //OCCLUSION
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(useOcclusion);
                EditorGUI.EndChangeCheck();
                EditorGUI.BeginDisabledGroup(!useOcclusion.boolValue);
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(occlusionIntensity);                
                EditorGUI.EndChangeCheck();
                List<string> paramNames = new List<string>();
                paramNames.Add("NONE");
                foreach (var i in editorEvent.LocalParameters)
                {
                    paramNames.Add(i.Name);
                }
                //ENDOCCL
                EditorGUI.BeginChangeCheck();
                src.paramIndex = EditorGUILayout.Popup(src.paramIndex, paramNames.ToArray());
                if (src.prevIndex != src.paramIndex)
                {
                    src.prevIndex = src.paramIndex;
                    src.occlusionParameterName = paramNames[src.paramIndex];
                    EditorUtility.SetDirty(target);
                }

                EditorGUI.EndChangeCheck();
                EditorGUI.EndDisabledGroup();
               
                EditorGUI.EndDisabledGroup();
                
            }

            parameterValueView.OnGUI(editorEvent, !eventReference.hasMultipleDifferentValues);
            /*
            fadeout.isExpanded = EditorGUILayout.Foldout(fadeout.isExpanded, "Advanced Controls");
            if (fadeout.isExpanded)
            {
                EditorGUILayout.PropertyField(preload, new GUIContent("Preload Sample Data"));
                EditorGUILayout.PropertyField(fadeout, new GUIContent("Allow Fadeout When Stopping"));
                EditorGUILayout.PropertyField(once, new GUIContent("Trigger Once"));
            }*/
        }
    }
    private void LegacyEditor()
    {
        var audioClip = serializedObject.FindProperty("m_AudioClip");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(audioClip, new GUIContent("AudioClip"));
        EditorGUI.EndChangeCheck();
    }
    
    private void DebugButtons()
    {
       if( GUILayout.Button("DebugPlay"))
        {
            source.Play();
        }
    }
    private class ParameterValueView
    {
        // The "Params" property from the SerializedObject we're editing in the inspector,
        // so we can expand/collapse it or revert to prefab.
        private SerializedProperty paramsProperty;

        // This holds one SerializedObject for each object in the current selection.
        private List<SerializedObject> serializedTargets = new List<SerializedObject>();

        // Mappings from EditorParamRef to initial parameter value property for all properties
        // found in the current selection.
        private List<PropertyRecord> propertyRecords = new List<PropertyRecord>();

        // Any parameters that are in the current event but are missing from some objects in
        // the current selection, so we can put them in the "Add" menu.
        private List<EditorParamRef> missingParameters = new List<EditorParamRef>();

        // A mapping from EditorParamRef to the initial parameter value properties in the
        // current selection that have the same name.
        // We need this because some objects may be missing some properties, and properties with
        // the same name may be at different array indices in different objects.
        private class PropertyRecord
        {
            public string name { get { return paramRef.Name; } }
            public EditorParamRef paramRef;
            public List<SerializedProperty> valueProperties;
        }

        public ParameterValueView(SerializedObject serializedObject)
        {
            paramsProperty = serializedObject.FindProperty("Params");

            foreach (UnityEngine.Object target in serializedObject.targetObjects)
            {
                serializedTargets.Add(new SerializedObject(target));
            }
        }

        // Rebuilds the propertyRecords and missingParameters collections.
        private void RefreshPropertyRecords(EditorEventRef eventRef)
        {
            propertyRecords.Clear();

            foreach (SerializedObject serializedTarget in serializedTargets)
            {
                SerializedProperty paramsProperty = serializedTarget.FindProperty("Params");

                foreach (SerializedProperty parameterProperty in paramsProperty)
                {
                    string name = parameterProperty.FindPropertyRelative("Name").stringValue;
                    SerializedProperty valueProperty = parameterProperty.FindPropertyRelative("Value");

                    PropertyRecord record = propertyRecords.Find(r => r.name == name);

                    if (record != null)
                    {
                        record.valueProperties.Add(valueProperty);
                    }
                    else
                    {
                        EditorParamRef paramRef = eventRef.LocalParameters.Find(p => p.Name == name);

                        if (paramRef != null)
                        {
                            propertyRecords.Add(
                                new PropertyRecord()
                                {
                                    paramRef = paramRef,
                                    valueProperties = new List<SerializedProperty>() { valueProperty },
                                });
                        }
                    }
                }
            }

            // Only sort if there is a multi-selection. If there is only one object selected,
            // the user can revert to prefab, and the behaviour depends on the array order,
            // so it's helpful to show the true order.
            if (serializedTargets.Count > 1)
            {
                propertyRecords.Sort((a, b) => EditorUtility.NaturalCompare(a.name, b.name));
            }

            missingParameters.Clear();
            missingParameters.AddRange(eventRef.LocalParameters.Where(
                p =>
                {
                    PropertyRecord record = propertyRecords.Find(r => r.name == p.Name);
                    return record == null || record.valueProperties.Count < serializedTargets.Count;
                }));
        }

        public void OnGUI(EditorEventRef eventRef, bool matchingEvents)
        {
            foreach (SerializedObject serializedTarget in serializedTargets)
            {
                serializedTarget.Update();
            }

            if (Event.current.type == EventType.Layout)
            {
                RefreshPropertyRecords(eventRef);
            }

            DrawHeader(matchingEvents);

            if (paramsProperty.isExpanded)
            {
                if (matchingEvents)
                {
                    DrawValues();
                }
                else
                {
                    GUILayout.Box("Cannot change parameters when different events are selected", GUILayout.ExpandWidth(true));
                }
            }

            foreach (SerializedObject serializedTarget in serializedTargets)
            {
                serializedTarget.ApplyModifiedProperties();
            }
        }

        private void DrawHeader(bool enableAddButton)
        {
            Rect controlRect = EditorGUILayout.GetControlRect();

            Rect titleRect = controlRect;
            titleRect.width = EditorGUIUtility.labelWidth;

            // Let the user revert the whole Params array to prefab by context-clicking the title.
            EditorGUI.BeginProperty(titleRect, GUIContent.none, paramsProperty);

            paramsProperty.isExpanded = EditorGUI.Foldout(titleRect, paramsProperty.isExpanded,
                "Initial Parameter Values");

            EditorGUI.EndProperty();

            Rect buttonRect = controlRect;
            buttonRect.xMin = titleRect.xMax;

            EditorGUI.BeginDisabledGroup(!enableAddButton);

            DrawAddButton(buttonRect);

            EditorGUI.EndDisabledGroup();
        }

        private void DrawAddButton(Rect position)
        {
            EditorGUI.BeginDisabledGroup(missingParameters.Count == 0);

            if (EditorGUI.DropdownButton(position, new GUIContent("Add"), FocusType.Passive))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("All"), false, () =>
                {
                    foreach (EditorParamRef parameter in missingParameters)
                    {
                        AddParameter(parameter);
                    }
                });

                menu.AddSeparator(string.Empty);

                foreach (EditorParamRef parameter in missingParameters)
                {
                    menu.AddItem(new GUIContent(parameter.Name), false,
                        (userData) =>
                        {
                            AddParameter(userData as EditorParamRef);
                        },
                        parameter);
                }

                menu.DropDown(position);
            }

            EditorGUI.EndDisabledGroup();
        }

        private void DrawValues()
        {
            // We use this to defer deletion so we don't mess with arrays while using
            // SerializedProperties that refer to array elements, as this can throw exceptions.
            string parameterToDelete = null;

            foreach (PropertyRecord record in propertyRecords)
            {
                if (record.valueProperties.Count == serializedTargets.Count)
                {
                    bool delete;
                    DrawValue(record, out delete);

                    if (delete)
                    {
                        parameterToDelete = record.name;
                    }
                }
            }

            if (parameterToDelete != null)
            {
                DeleteParameter(parameterToDelete);
            }
        }

        private void DrawValue(PropertyRecord record, out bool delete)
        {
            delete = false;

            GUIContent removeLabel = new GUIContent("Remove");

            Rect position = EditorGUILayout.GetControlRect();

            Rect nameLabelRect = position;
            nameLabelRect.width = EditorGUIUtility.labelWidth;

            Rect removeButtonRect = position;
            removeButtonRect.width = EditorStyles.miniButton.CalcSize(removeLabel).x;
            removeButtonRect.x = position.xMax - removeButtonRect.width;

            Rect sliderRect = position;
            sliderRect.xMin = nameLabelRect.xMax;
            sliderRect.xMax = removeButtonRect.xMin - EditorStyles.miniButton.margin.left;

            GUIContent nameLabel = new GUIContent(record.name);

            float value = 0;
            bool mixedValues = false;

            // We use EditorGUI.BeginProperty when there is a single object selected, so
            // the user can revert the value to prefab by context-clicking the name.
            // We handle multi-selections ourselves, so that we can deal with
            // mismatched arrays nicely.
            if (record.valueProperties.Count == 1)
            {
                value = record.valueProperties[0].floatValue;
                EditorGUI.BeginProperty(position, nameLabel, record.valueProperties[0]);
            }
            else
            {
                bool first = true;

                foreach (SerializedProperty property in record.valueProperties)
                {
                    if (first)
                    {
                        value = property.floatValue;
                        first = false;
                    }
                    else if (property.floatValue != value)
                    {
                        mixedValues = true;
                        break;
                    }
                }
            }

            EditorGUI.LabelField(nameLabelRect, nameLabel);

            if (record.paramRef.Type == ParameterType.Labeled)
            {
                EditorGUI.BeginChangeCheck();

                EditorGUI.showMixedValue = mixedValues;

                int newValue = EditorGUI.Popup(sliderRect, (int)value, record.paramRef.Labels);

                EditorGUI.showMixedValue = false;

                if (EditorGUI.EndChangeCheck())
                {
                    foreach (SerializedProperty property in record.valueProperties)
                    {
                        property.floatValue = newValue;
                    }
                }
            }
            else if (record.paramRef.Type == ParameterType.Discrete)
            {
                EditorGUI.BeginChangeCheck();

                EditorGUI.showMixedValue = mixedValues;

                int newValue = EditorGUI.IntSlider(sliderRect, (int)value, (int)record.paramRef.Min, (int)record.paramRef.Max);

                EditorGUI.showMixedValue = false;

                if (EditorGUI.EndChangeCheck())
                {
                    foreach (SerializedProperty property in record.valueProperties)
                    {
                        property.floatValue = newValue;
                    }
                }
            }
            else
            {
                EditorGUI.BeginChangeCheck();

                EditorGUI.showMixedValue = mixedValues;

                float newValue = EditorGUI.Slider(sliderRect, value, record.paramRef.Min, record.paramRef.Max);

                EditorGUI.showMixedValue = false;

                if (EditorGUI.EndChangeCheck())
                {
                    foreach (SerializedProperty property in record.valueProperties)
                    {
                        property.floatValue = newValue;
                    }
                }
            }

            delete = GUI.Button(removeButtonRect, removeLabel, EditorStyles.miniButton);

            if (record.valueProperties.Count == 1)
            {
                EditorGUI.EndProperty();
            }
            else
            {
                // Context menu to set all values from one object in the multi-selection.
                if (mixedValues && Event.current.type == EventType.ContextClick
                    && nameLabelRect.Contains(Event.current.mousePosition))
                {
                    GenericMenu menu = new GenericMenu();

                    foreach (SerializedProperty sourceProperty in record.valueProperties)
                    {
                        UnityEngine.Object targetObject = sourceProperty.serializedObject.targetObject;

                        menu.AddItem(new GUIContent(string.Format("Set to Value of '{0}'", targetObject.name)), false,
                            (userData) => CopyValueToAll(userData as SerializedProperty, record.valueProperties),
                            sourceProperty);
                    }

                    menu.DropDown(position);

                }
            }
        }

        // Copy the value from the source property to all target properties.
        private void CopyValueToAll(SerializedProperty sourceProperty, List<SerializedProperty> targetProperties)
        {
            foreach (SerializedProperty targetProperty in targetProperties)
            {
                if (targetProperty != sourceProperty)
                {
                    targetProperty.floatValue = sourceProperty.floatValue;
                    targetProperty.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        // Add an initial value for the given parameter to all selected objects that don't have one.
        private void AddParameter(EditorParamRef parameter)
        {
            foreach (SerializedObject serializedTarget in serializedTargets)
            {
                FMODAudioSource emitter = serializedTarget.targetObject as FMODAudioSource;

                if (Array.FindIndex(emitter.Params, p => p.Name == parameter.Name) < 0)
                {
                    SerializedProperty paramsProperty = serializedTarget.FindProperty("Params");

                    int index = paramsProperty.arraySize;
                    paramsProperty.InsertArrayElementAtIndex(index);

                    SerializedProperty arrayElement = paramsProperty.GetArrayElementAtIndex(index);

                    arrayElement.FindPropertyRelative("Name").stringValue = parameter.Name;
                    arrayElement.FindPropertyRelative("Value").floatValue = parameter.Default;

                    serializedTarget.ApplyModifiedProperties();
                }
            }
        }

        // Delete initial parameter values for the given name from all selected objects.
        private void DeleteParameter(string name)
        {
            foreach (SerializedObject serializedTarget in serializedTargets)
            {
                SerializedProperty paramsProperty = serializedTarget.FindProperty("Params");

                foreach (SerializedProperty child in paramsProperty)
                {
                    if (child.FindPropertyRelative("Name").stringValue == name)
                    {
                        child.DeleteCommand();
                        break;
                    }
                }
            }
        }
    }
}
