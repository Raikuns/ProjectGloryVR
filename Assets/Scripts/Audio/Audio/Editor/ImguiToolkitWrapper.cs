using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ImguiToolkitWrapper : UnityEditor.Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        var prop = serializedObject.GetIterator();
        if (prop.NextVisible(true))
        {
            do
            {
                var field = new PropertyField(prop);

                if (prop.name == "m_Script")
                {
                    field.SetEnabled(false);
                }

                root.Add(field);
            }
            while (prop.NextVisible(false));
        }

        return root;
    }
}
