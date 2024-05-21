using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace TheGuarden.Utility.Editor
{
    [CustomEditor(typeof(ExposedProperty), true)]
    public class ExposedPropertyInspector : UnityEditor.Editor
    {
        private Label propertyIDLabel;

        private void UpdateLabel()
        {
            ExposedProperty property = serializedObject.targetObject as ExposedProperty;
            property.ResetPropertyID();
            propertyIDLabel.text = $"PropertyID: {property.PropertyID}";
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            PropertyField propertyNameField = root.Q<PropertyField>("PropertyField:propertyName");
            ExposedProperty property = serializedObject.targetObject as ExposedProperty;
            propertyIDLabel = new Label($"PropertyID: {property.PropertyID}");
            propertyIDLabel.TrackPropertyValue(serializedObject.FindProperty("propertyName"), (serializedProperty) => UpdateLabel());
            root.Add(propertyIDLabel);
            return root;
        }
    }
}
