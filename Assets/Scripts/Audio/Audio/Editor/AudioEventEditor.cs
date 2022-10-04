using UnityEditor;
using UnityEngine;


using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

//[CustomPropertyDrawer(typeof(AudioEvent))]
public class AudioEventEditor : PropertyDrawer
{
    // Draw the property inside the given rect
    /*public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        base.CreatePropertyGUI(property);
        // Create property container element.
        var container = new VisualElement();
        return container;
        
    }*/

    public VisualElement RenderProperties()
    {
        var container = new VisualElement();
        var items = new List<string>();
        Action<VisualElement, int> bindItem = (e, i) => (e as AudioEventItem).text = items[i];
        Func<VisualElement> makeItem = () => new AudioEventItem();
        // Create a new label and give it a style class.
        var csharpHelpBox = new HelpBox("Fmod Audio Event", HelpBoxMessageType.None);
        csharpHelpBox.AddToClassList("some-styled-help-box");
        var listView = new ListView();
        listView.showAddRemoveFooter = true;


        listView.makeItem = makeItem;
        listView.bindItem = bindItem;
        listView.itemsSource = items;
        listView.selectionType = SelectionType.Multiple;
        container.Add(csharpHelpBox);
        container.Add(listView);
        return container;
    }

    
}

class AudioEventItem : VisualElement
{
    
    public new class UxmlFactory : UxmlFactory<AudioEventItem, UxmlTraits> { }
    
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_String =
            new UxmlStringAttributeDescription { name = "string-attr", defaultValue = "default_value" };
        UxmlIntAttributeDescription m_Int =
            new UxmlIntAttributeDescription { name = "int-attr", defaultValue = 2 };


        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var ate = ve as AudioEventItem;

            ate.stringAttr = m_String.GetValueFromBag(bag, cc);
            ate.intAttr = m_Int.GetValueFromBag(bag, cc);
            ate.text = "hihi";
        }
    }

    public string stringAttr { get; set; }
    public int intAttr { get; set; }
    public string text { get; set; }
}

