﻿using UnityEngine;
using UnityEngine.Events;

namespace MalbersAnimations.Events
{
    /// <summary>Simple Event Raiser On Disable</summary>
    [AddComponentMenu("Malbers/Events/On [Enable-Disable] Event")]
    public class EnableDisableEvent : MonoBehaviour
    {
        public UnityEvent OnActive;
        public UnityEvent OnDeactive;

        public void OnEnable() => OnActive.Invoke();
        public void OnDisable() => OnDeactive.Invoke();

        public string Description = "";
        [HideInInspector] public bool ShowDescription = false;
        [ContextMenu("Show Description")]
        internal void EditDescription() => ShowDescription ^= true; 
    }



#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(EnableDisableEvent))]
    public class ActiveGameObjectInspector : UnityEditor.Editor
    {
        UnityEditor.SerializedProperty OnActive, OnDeactive, ShowDescription, Description;

        private GUIStyle style;


        private void OnEnable()
        {
            ShowDescription = serializedObject.FindProperty("ShowDescription");
            Description = serializedObject.FindProperty("Description");
            OnDeactive = serializedObject.FindProperty("OnDeactive");
            OnActive = serializedObject.FindProperty("OnActive");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (ShowDescription.boolValue)
            {
                if (style == null)
                {
                    style = new GUIStyle(MTools.StyleBlue)
                    {
                        fontSize = 12,
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.MiddleLeft,
                        stretchWidth = true
                    };

                    style.normal.textColor = UnityEditor.EditorStyles.boldLabel.normal.textColor;
                }

                Description.stringValue = UnityEditor.EditorGUILayout.TextArea(Description.stringValue, style);
            }

            UnityEditor.EditorGUILayout.PropertyField(OnActive, new GUIContent("On Enabled"));
            UnityEditor.EditorGUILayout.PropertyField(OnDeactive, new GUIContent("On Disabled"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}