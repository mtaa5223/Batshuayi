﻿using System.Linq;
using UnityEngine;

namespace MalbersAnimations.Controller.AI
{
    using Scriptables;
    using System.Collections.Generic;

    [CreateAssetMenu(menuName = "Malbers Animations/Pluggable AI/Decision/Check String Listener", order = 5)]
    public class CheckVarStringListener : MAIDecision
    {
        public override string DisplayName => "Variables/Check String Listener";

        public enum Affect
        {
            Self,
            CurrentTarget,
            Tag, 
            TransformHook,
            GameObjectHook, 
            RuntimeGameObjectSet
        }

        public enum ComponentPlace
        {
            SameHierarchy,
            Parent,
            Children
        }

        [Space]
        [Tooltip("Check the Variable Listener ID Value, when this value is Zero, the ID is ignored")]
        public IntReference ListenerID = 0;
        /// <summary>Range for Looking forward and Finding something</summary>
        [Space, Tooltip("Find the VarListener component on:\n\n" +
            "-Self: \nCheck on the Animal Gameobject using the Brain\n" +
            "-Target: \ncurrent AI Target\n" +
            "-Tag: \nAll the gameobjects using a Malbers Tag\n" +
            "-Transform Hook: \na in a Transform Hook\n" +
            "-GameObject Hook: \na in a GameObject Hook\n" +
            "-Runtime GameObject Set: \na in all the GameObject in a Runtime Set")]
        public Affect checkOn = Affect.Self;
        
        [Tooltip("Check if the Var Listener component its placed on:\n\n" +
            "-SameHierarchy: \nsame hierarchy level as the gameobject(s) in the [CheckOn] Option\n" +
            "-Parent: \nany of the parents of the gameobject(s) in the [CheckOn] Option\n" +
            "-Children: \nany of the children of the gameobject(s) in the [CheckOn] Option")]
        public ComponentPlace PlacedOn = ComponentPlace.SameHierarchy;
        [Hide("checkOn",  (int)Affect.Tag)] public Tag tag;
        [Hide("checkOn", (int) Affect.TransformHook)] public TransformVar Transform;
        [Hide("checkOn", (int)Affect.GameObjectHook)] public GameObjectVar GameObject;
        [Hide("checkOn", (int)Affect.RuntimeGameObjectSet)] public RuntimeGameObjects GameObjectSet;

        [Tooltip("Values to compare with the Listener. If any of the values match then it will return true"),NonReorderable]
        public StringReference[] value;

        public bool debug = false;

        public override void PrepareDecision(MAnimalBrain brain, int Index)
        {
            MonoBehaviour[] monoValue = null;

            var objectives = GetObjective(brain);

            if (objectives == null)
            {
                if (debug) Debug.LogWarning("Check Var Listener Objectives is Null, Please check your Decisions", this);
                return;
            }

            if (objectives != null && objectives.Length > 0)
            {
                foreach (var target in objectives)
                {
                    if (target == null)
                    {
                        if (debug) Debug.LogWarning($"Check Var Listener Checking on [{checkOn}]. Objective is Null", this);
                        return;
                    }
                    monoValue = GetComponents<StringVarListener>(target.gameObject);
                }
            }

            brain.DecisionsVars[Index].AddComponents(monoValue);
        }

        private Transform[] GetObjective(MAnimalBrain brain)
        {
            switch (checkOn)
            {
                case Affect.Self:
                    return new Transform[1] { brain.Animal.transform };
                case Affect.CurrentTarget:
                    return new Transform[1] { brain.Target };
                case Affect.Tag:
                    var tagH = Tags.TagsHolders.FindAll(X => X.HasTag(tag));
                    if (tagH != null)
                    {
                        var TArray = new List<Transform>();
                        foreach (var t in tagH)TArray.Add(t.transform);
                        return TArray.ToArray(); 
                    }
                        return null;
                case Affect.TransformHook:
                    {
                        if (Transform == null || Transform.Value == null) return null;
                        return new Transform[1] { Transform.Value };
                    }
                case Affect.GameObjectHook:
                    if (!GameObject.Value.IsPrefab())
                        return new Transform[1] { GameObject.Value.transform };
                    else
                    {
                        Debug.LogWarning("The GameObject Hook is a Prefab. Is not in the scene.", GameObject.Value);

                        return null;
                    }
                case Affect.RuntimeGameObjectSet:
                    var TGOS = new List<Transform>();
                    foreach (var t in GameObjectSet.Items) TGOS.Add(t.transform);
                    return TGOS.ToArray();
                default:
                    return null;
            }
        }

        private TVarListener[] GetComponents<TVarListener>(GameObject gameObject)
            where TVarListener : VarListener
        {
            TVarListener[] list;

            switch (PlacedOn)
            {
                case ComponentPlace.Children:
                    list = gameObject.GetComponentsInChildren<TVarListener>();
                    break;
                case ComponentPlace.Parent:
                    list = gameObject.GetComponentsInParent<TVarListener>();
                    break;
                case ComponentPlace.SameHierarchy:
                    list = gameObject.GetComponents<TVarListener>();
                    break;
                default:
                    list = gameObject.GetComponents<TVarListener>();
                    break;
            }

            list = list.ToList().FindAll(x => ListenerID.Value == 0 || x.ID == ListenerID.Value).ToArray();

            return list;
        }

        public override bool Decide(MAnimalBrain brain, int Index)
        {
            var listeners = brain.DecisionsVars[Index].Components;

            if (listeners == null || listeners.Length == 0) return false;

            bool result = false;

            foreach (var varListener in listeners)
            {
                if (varListener is VarListener)
                {
                    var LB = (varListener as StringVarListener);

                    for (int i = 0; i < value.Length; i++)
                    {
                        result = LB.Value == value[i];
                        if (result) break;
                    }
                }
            }

           // if (debug)
            //    Debug.Log($"{brain.Animal.name}: <B>[{name}]</B> ListenerString<{LB.transform.name}> ID<{LB.ID.Value}> Value<{LB.Value}>  <B>Result[{result}]</B>");
            return result;
        }
    }
}