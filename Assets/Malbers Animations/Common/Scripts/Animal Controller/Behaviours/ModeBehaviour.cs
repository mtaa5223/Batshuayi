﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MalbersAnimations.Controller
{

    [HelpURL("https://malbersanimations.gitbook.io/animal-controller/main-components/manimal-controller/modes#mode-behaviour")]
    [AddComponentMenu("Malbers/Mode Behavior")]
    public class ModeBehaviour : StateMachineBehaviour
    {
        public ModeID ModeID;

        [Tooltip("Calls 'Animation Tag Enter' on the Modes")]
        public bool EnterMode = true;
        [Tooltip("Calls 'Animation Tag Exit' on the Modes")]
        public bool ExitMode = true;

        [Tooltip("Next Ability to do on the Mode.If is set to -1, The Exit On Ability Logic will be ignored.\n" +
            "Used this when you need an ability to finish on another Ability.\n" +
            "E.g. If the wolf is in the Ability SIT, and you activate the HOWL; When HOWL finish you can play again SIT right after")]
        public int ExitAbility = -1;

        private MAnimal animal;
        private Mode ModeOwner;
        private Ability ActiveAbility;

        public void InitializeBehaviour(MAnimal animal)
        {
            this.animal = animal;

            if (ModeID != null)
            {
                ModeOwner = animal.Mode_Get(ModeID);
            }
            else
            {
                Debug.LogWarning("There's a Mode behaviour without an ID. Please check all your Mode Animations states.");
                Destroy(this);
            }
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animal == null)
            {
                animal = animator.GetComponent<MAnimal>();
                ModeOwner = animal.Mode_Get(ModeID);
                // Debug.LogWarning($"Animal is was null? ModeOwner null? {ModeOwner == null}");
            }


            if (ModeID == null) { Debug.LogError("Mode behaviour needs an ID"); return; }
            if (ModeOwner == null) { Debug.LogError($"There's no [{ModeID.name}] mode on your character"); return; }

            ActiveAbility = ModeOwner.ActiveAbility;
            if (animal.ModeStatus == Int_ID.Loop) return;            //Means is Looping so Skip!!!

            if (EnterMode) ModeOwner.AnimationTagEnter(stateInfo.fullPathHash);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Means is Looping to itself So Skip the Exit Mode EXTREMELY IMPORTANT
            if (animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash == stateInfo.fullPathHash) return;

            if (ExitMode) ModeOwner.AnimationTagExit(ActiveAbility, ExitAbility);
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ModeOwner.OnModeStateMove(stateInfo, animator, layerIndex);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ModeBehaviour))]
    public class ModeBehaviourED : Editor
    {
        SerializedProperty EnterMode, ExitMode, ModeID, ExitAbility;
        Color RequiredColor = new Color(1, 0.4f, 0.4f, 1);

        void OnEnable()
        {

            ModeID = serializedObject.FindProperty("ModeID");
            EnterMode = serializedObject.FindProperty("EnterMode");
            ExitMode = serializedObject.FindProperty("ExitMode");
            ExitAbility = serializedObject.FindProperty("ExitAbility");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {


                using (new GUILayout.HorizontalScope())
                {
                    var currentGUIColor = GUI.color;
                    GUI.color = ModeID.objectReferenceValue == null ? RequiredColor : currentGUIColor;
                    EditorGUIUtility.labelWidth = 70;
                    EditorGUILayout.PropertyField(ModeID);

                    var width = 42;
                    GUI.color = EnterMode.boolValue ? Color.green : currentGUIColor;

                    EnterMode.boolValue = GUILayout.Toggle(EnterMode.boolValue,
                                       new GUIContent("Enter"), EditorStyles.miniButton, GUILayout.Width(width));


                    GUI.color = ExitMode.boolValue ? Color.green : currentGUIColor;

                    ExitMode.boolValue = GUILayout.Toggle(ExitMode.boolValue,
                                       new GUIContent("Exit"), EditorStyles.miniButton, GUILayout.Width(width));

                    GUI.color = currentGUIColor;

                }
                EditorGUIUtility.labelWidth = 0;

                if (ExitMode.boolValue) EditorGUILayout.PropertyField(ExitAbility);

            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}