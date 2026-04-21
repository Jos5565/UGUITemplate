using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    [CustomEditor(typeof(ButtonToggle), true)]
    [CanEditMultipleObjects]
    public class ButtonToggleEditor : Editor
    {
        private ButtonToggle buttonToggle;
        private string useToggle;
        private void OnEnable()
        {
            buttonToggle = (ButtonToggle)target;
            buttonToggle.Initialize();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("Button Settings", EditorStyles.boldLabel);
            DrawSelectableProperties();


            EditorGUILayout.Space(10);
            if (buttonToggle.isToggle)
            {
                SerializedProperty spriteField = serializedObject.FindProperty("toggleCheckSprite");
                EditorGUILayout.PropertyField(spriteField, new GUIContent("Toggle Check Image"), true);
                SerializedProperty booleanField = serializedObject.FindProperty("isOn");
                EditorGUILayout.PropertyField(booleanField, new GUIContent("isOn"), true);
                EditorGUILayout.Space(5);
                useToggle = "Activated Button";
                GUI.backgroundColor = Color.yellow;

                SerializedProperty eventPropField = serializedObject.FindProperty("IsOn");
                EditorGUILayout.PropertyField(eventPropField, new GUIContent("IsOn"), true);
            }
            else
            {
                useToggle = "Activated Toggle";
                GUI.backgroundColor = Color.cyan;
                SerializedProperty eventPropField = serializedObject.FindProperty("OnClick");
                EditorGUILayout.PropertyField(eventPropField, new GUIContent("OnClick"), true);
            }
            if (GUILayout.Button(useToggle))
            {
                buttonToggle.isToggle = !buttonToggle.isToggle;
            }
            // 내 커스텀 설정은 항상 노출
            GUI.backgroundColor = Color.white;

            serializedObject.ApplyModifiedProperties();

        }

        private void DrawSelectableProperties()
        {
            SerializedProperty interactable = serializedObject.FindProperty("m_Interactable");
            EditorGUILayout.PropertyField(interactable);
            SerializedProperty targetGraphic = serializedObject.FindProperty("m_TargetGraphic");
            EditorGUILayout.PropertyField(targetGraphic);
            SerializedProperty transitionProp = serializedObject.FindProperty("m_Transition");
            EditorGUILayout.PropertyField(transitionProp);
            EditorGUI.indentLevel++;
            Selectable.Transition transition = (Selectable.Transition)transitionProp.enumValueIndex;

            if (transition == Selectable.Transition.ColorTint)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Colors"));
            }
            else if (transition == Selectable.Transition.SpriteSwap)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_SpriteState"));
            }
            else if (transition == Selectable.Transition.Animation)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_AnimationTriggers"));
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Navigation"));
        }
    }


}
