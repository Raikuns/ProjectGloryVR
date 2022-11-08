using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CustomEditor(typeof(UberAudioParameterTrigger))]
public class UberAudioParameterTriggerEditor : Editor
{
    private FMODAudioSource targetEmitter;
    private SerializedProperty emitters;
    private SerializedProperty trigger;
    private SerializedProperty tag;

    private bool[] expanded;

    private void OnEnable()
    {
        emitters = serializedObject.FindProperty("Emitters");
        trigger = serializedObject.FindProperty("TriggerEvent");
        tag = serializedObject.FindProperty("CollisionTag");
        targetEmitter = null;
        for (int i = 0; i < emitters.arraySize; i++)
        {
            targetEmitter = emitters.GetArrayElementAtIndex(i).FindPropertyRelative("Target").objectReferenceValue as FMODAudioSource;
            if (targetEmitter != null)
            {
                expanded = new bool[targetEmitter.GetComponents<FMODAudioSource>().Length];
                break;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        var newTargetEmitter = EditorGUILayout.ObjectField("Target", targetEmitter, typeof(FMODAudioSource), true) as FMODAudioSource;
        if (newTargetEmitter != targetEmitter)
        {
            emitters.ClearArray();
            targetEmitter = newTargetEmitter;

            if (targetEmitter == null)
            {
                serializedObject.ApplyModifiedProperties();
                Debug.LogWarning($"Target emitter is null. UberParameterTrigger");
                return;
            }

            List<FMODAudioSource> newEmitters = new List<FMODAudioSource>();
            targetEmitter.GetComponents(newEmitters);
            expanded = new bool[newEmitters.Count];
            foreach (var emitter in newEmitters)
            {
                emitters.InsertArrayElementAtIndex(0);
                emitters.GetArrayElementAtIndex(0).FindPropertyRelative("Target").objectReferenceValue = emitter;
            }
        }

        if (targetEmitter == null)
        {
            Debug.LogWarning($"Target emitter is null. UberParameterTrigger");
            return;
        }

        EditorGUILayout.PropertyField(trigger, new GUIContent("Trigger"));

        if (trigger.enumValueIndex >= (int)EmitterGameEvent.TriggerEnter && trigger.enumValueIndex <= (int)EmitterGameEvent.TriggerExit2D)
        {
            tag.stringValue = EditorGUILayout.TagField("Collision Tag", tag.stringValue);
        }

        var localEmitters = new List<FMODAudioSource>();
        targetEmitter.GetComponents(localEmitters);

        int emitterIndex = 0;
        foreach (var emitter in localEmitters)
        {
            SerializedProperty emitterProperty = null;
            for (int i = 0; i < emitters.arraySize; i++)
            {
                if (emitters.GetArrayElementAtIndex(i).FindPropertyRelative("Target").objectReferenceValue == emitter)
                {
                    emitterProperty = emitters.GetArrayElementAtIndex(i);
                    break;
                }
            }

            // New emitter component added to game object since we last looked
            if (emitterProperty == null)
            {
                emitters.InsertArrayElementAtIndex(0);
                emitterProperty = emitters.GetArrayElementAtIndex(0);
                emitterProperty.FindPropertyRelative("Target").objectReferenceValue = emitter;
            }

            if (!emitter.fmodEvent.IsNull)
            {
                expanded[emitterIndex] = EditorGUILayout.Foldout(expanded[emitterIndex], emitter.fmodEvent.Path);
                if (expanded[emitterIndex])
                {
                    var eventRef = EventManager.EventFromGUID(emitter.fmodEvent.Guid);

                    foreach (var paramRef in eventRef.LocalParameters)
                    {
                        bool set = false;
                        int index = -1;
                        for (int i = 0; i < emitterProperty.FindPropertyRelative("Params").arraySize; i++)
                        {
                            if (emitterProperty.FindPropertyRelative("Params").GetArrayElementAtIndex(i).FindPropertyRelative("Name").stringValue == paramRef.Name)
                            {
                                index = i;
                                set = true;
                                break;
                            }
                        }
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel(paramRef.Name);
                        bool newSet = GUILayout.Toggle(set, "");
                        if (!set && newSet)
                        {
                            index = 0;
                            emitterProperty.FindPropertyRelative("Params").InsertArrayElementAtIndex(0);
                            emitterProperty.FindPropertyRelative("Params").GetArrayElementAtIndex(0).FindPropertyRelative("Name").stringValue = paramRef.Name;
                            emitterProperty.FindPropertyRelative("Params").GetArrayElementAtIndex(0).FindPropertyRelative("Value").floatValue = 0;
                        }
                        if (set && !newSet)
                        {
                            emitterProperty.FindPropertyRelative("Params").DeleteArrayElementAtIndex(index);
                        }
                        set = newSet;

                        if (set)
                        {
                            var valueProperty = emitterProperty.FindPropertyRelative("Params")
                                .GetArrayElementAtIndex(index).FindPropertyRelative("Value");
                            valueProperty.floatValue =
                                EditorUtils.DrawParameterValueLayout(valueProperty.floatValue, paramRef);
                        }
                        else
                        {
                            using (new EditorGUI.DisabledScope(true))
                            {
                                EditorUtils.DrawParameterValueLayout(0, paramRef);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            emitterIndex++;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
