using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif
namespace MalbersAnimations.Utilities
{
    /// <summary> Simple script to reparent a bone on enable </summary>
    [AddComponentMenu("Malbers/Utilities/Tools/Parent")]
    public class ReParent : MonoBehaviour
    {
        [Tooltip("Reparent this gameObject to a new Transform. Use this to have more organized GameObjects on the hierarchy")]
        public Transform newParent;

        public bool ResetLocal = false;
        private void OnEnable()
        {
            if (newParent == null)
                transform.parent = null;
            else

                transform.SetParent(newParent, true);
        }
        private void Reset()
        {
            newParent = transform.parent;
            if (ResetLocal) transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(ReParent)), CanEditMultipleObjects]
    public class ReParentEditor : Editor
    {
        SerializedProperty newParent, ResetLocal;
        private void OnEnable()
        {
            newParent = serializedObject.FindProperty("newParent");
            ResetLocal = serializedObject.FindProperty("ResetLocal");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(newParent);
                ResetLocal.boolValue = GUILayout.Toggle(ResetLocal.boolValue, new GUIContent("R", "Reset Local Position and Rotation after parenting"),
                    EditorStyles.miniButton, GUILayout.Width(23));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}