using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AnimatorBehaviour.Action))]
public class AnimatorActionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var indent = EditorGUI.indentLevel;
        EditorGUI.BeginProperty(position, label, property);

        var typeRect = new Rect(position.x, position.y, 120, position.height);
        var valueRect = new Rect(position.x + 125, position.y, 40, position.height);
        var keyRect = new Rect(position.x + 170, position.y, position.width - 170, position.height);

        EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("type"), GUIContent.none);
        EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("key"), GUIContent.none);
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);

        EditorGUI.EndProperty();
        EditorGUI.indentLevel = indent;
    }
}