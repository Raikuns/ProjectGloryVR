using UnityEditor;
using UnityEditorInternal;
namespace FMODUnity
{
    [CustomEditor(typeof(StudioListener))]
    [CanEditMultipleObjects]
    public class StudioListenerEditor : Editor
    {
        public SerializedProperty attenuationObject;

        private void OnEnable()
        {
            attenuationObject = serializedObject.FindProperty("attenuationObject");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(true);
            int index = ((StudioListener)serializedObject.targetObject).ListenerNumber;
            EditorGUILayout.IntSlider("Listener Index", index, 0, FMOD.CONSTANTS.MAX_LISTENERS - 1);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginChangeCheck();
            var mask = serializedObject.FindProperty("occlusionMask");
            int temp = EditorGUILayout.MaskField("Occlusion Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(mask.intValue), InternalEditorUtility.layers);
            mask.intValue = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(temp);
            EditorGUILayout.PropertyField(attenuationObject);
            serializedObject.ApplyModifiedProperties();
        }
    }
}