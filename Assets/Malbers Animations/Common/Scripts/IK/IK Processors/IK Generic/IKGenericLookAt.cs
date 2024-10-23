using MalbersAnimations.Scriptables;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MalbersAnimations.IK
{
    [Serializable]
    public struct GenericILookAt
    {
        [Tooltip("Name of the Bone {Display only}")]
        public string name;
        [Range(0, 1)]
        public float Weight;
        [Tooltip("Bone Reference from the Targets Array the IK Offset")]
        public int BoneIndex;
        public Vector3 Offset;



        public UpVectorType upVector;
        [Hide(nameof(upVector), (int)UpVectorType.Local)]
        public Vector3 LocalUp;
        [Hide(nameof(upVector), (int)UpVectorType.Global)]
        public Vector3Var WorldUp;

        [Tooltip("Show Gizmos")]
        public bool Gizmos;


        //  public TransformValues[] CacheChilds { get; set; }

        public Vector3 UpVector
        {
            get
            {
                return upVector switch
                {
                    UpVectorType.Local => LocalUp,
                    UpVectorType.Global => (Vector3)WorldUp,
                    _ => Vector3.up,
                };
            }
        }
    }


    [Serializable]
    [AddTypeMenu("Generic/LookAt")]
    public class IKGenericLookAt : IKProcessor
    {
        public override bool RequireTargets => true;

        public enum UpVectorType { VectorUp, Local, Global }

        public List<GenericILookAt> Bones = new();

        public override void LateUpdate(IKSet IKSet, Animator anim, int index, float weight)
        {
            Quaternion TargetRotation;

            foreach (var bn in Bones)
            {
                var FinalWeight = weight * bn.Weight;
                if (FinalWeight <= 0) continue; //Do nothing if the weight is zero

                Transform Bone = IKSet.Targets[bn.BoneIndex];
                if (Bone == null) continue; //Missing Bone

                var BoneValue = IKSet.CacheTargets[bn.BoneIndex];

                ////Store the  bone's Child Rotation 
                //if (bn.KeepChildRot)
                //{
                //    ChildRotations = new Quaternion[Bone.Value.childCount];
                //    for (int i = 0; i < ChildRotations.Length; i++)
                //        ChildRotations[i] = Bone.Value.GetChild(i).rotation;
                //}

                var direction = IKSet.aimer.AimDirection;
                var angle = Vector3.Angle(anim.transform.forward, direction);

                //if (bn.LookAtLimit.maxValue != 0 && bn.LookAtLimit.minValue != 0) //Check the Limit in case there is a limit
                //    FinalWeight *= angle.CalculateRangeWeight(bn.LookAtLimit.minValue, bn.LookAtLimit.maxValue);

                if (bn.Gizmos) MDebug.DrawRay(Bone.transform.position, direction.normalized, Color.Lerp(Color.black, Color.green, FinalWeight));

                if (FinalWeight == 0) continue; //Do nothing if the weight is zero

                TargetRotation = Quaternion.LookRotation(IKSet.aimer.RawAimDirection, bn.UpVector) * Quaternion.Euler(bn.Offset);

                Bone.rotation = Quaternion.Lerp(BoneValue.rotation, TargetRotation, FinalWeight);
                //Store the bone's Child Rotation
            }
        }

        public override void Validate(IKSet set, Animator animator, int index)
        {
            if (set.Targets.Length == 0)
            {
                Debug.LogWarning($"There's no Targets on the IK Set. Generic IK needs a Target on on Index [{TargetIndex}]");
            }
            if (set.Targets.Length <= TargetIndex)
            {
                Debug.LogWarning($"The Target Index [{TargetIndex}] is out of range on the IK Set. The IK Set has only {set.Targets.Length} targets");
            }
            if (set.Targets[TargetIndex].Value == null)
            {
                Debug.LogWarning($"The Target in Index [{TargetIndex}] is Empty. Make sure you set a proper value. in the Editor, or at Runtime");
            }
            else
            {
                Debug.Log($"<B>[IK Processor: {name}][IK Generic Look At]</B>  <color=yellow>[OK]</color>");
            }
        }

        //        public override void OnDrawGizmos(IKSet IKSet, Animator anim, float weight)
        //        {
        //#if UNITY_EDITOR && MALBERS_DEBUG
        //            // bool AppIsPlaying = Application.isPlaying;
        //            if (anim == null) return;

        //            foreach (var bn in Bones)
        //            {
        //                if (IKSet.Targets != null && IKSet.Targets.Length > 0 && IKSet.Targets.Length > bn.BoneIndex)
        //                {
        //                    var Bone = IKSet.Targets[bn.BoneIndex];

        //                    if (Bone == null || !bn.Gizmos) continue;

        //                    var FinalWeight = weight * bn.Weight * GetProcessorAnimWeight(anim);

        //                    Handles.color = new Color(0, 1, 0, 0.1f);
        //                    Handles.DrawSolidArc(Bone.position, bn.UpVector,
        //                        Quaternion.Euler(0, -bn.LookAtLimit.minValue, 0) * anim.transform.forward, bn.LookAtLimit.minValue * 2, 1);



        //                    Handles.color = Color.green;
        //                    Handles.DrawWireArc(Bone.position,
        //                        bn.UpVector, Quaternion.Euler(0, -bn.LookAtLimit.minValue, 0) * anim.transform.forward, bn.LookAtLimit.minValue * 2, 1);


        //                    Handles.color = new Color(0, 0.3f, 0, 0.2f);
        //                    var Maxlimit = (bn.LookAtLimit.minValue - bn.LookAtLimit.maxValue);

        //                    Handles.DrawSolidArc(Bone.position,
        //                        bn.UpVector, Quaternion.Euler(0, -(bn.LookAtLimit.minValue), 0) * anim.transform.forward, (Maxlimit), 1);

        //                    Handles.DrawSolidArc(Bone.position,
        //                        bn.UpVector, Quaternion.Euler(0, (bn.LookAtLimit.minValue), 0) * anim.transform.forward, -(Maxlimit), 1);


        //                    Handles.color = Color.black;

        //                    Handles.DrawWireArc(Bone.position,
        //                        bn.UpVector, Quaternion.Euler(0, -(bn.LookAtLimit.minValue), 0) * anim.transform.forward, (Maxlimit), 1);

        //                    Handles.DrawWireArc(Bone.position,
        //                        bn.UpVector, Quaternion.Euler(0, (bn.LookAtLimit.minValue), 0) * anim.transform.forward, -(Maxlimit), 1);

        //                }
        //            }
        //#endif
        //        }
    }



    public enum IKGenerigLookAt
    {
        LookAt,
        [InspectorName("Local Rotation Additive")]
        AdditiveOffset,
        [InspectorName("Local Rotation Override")]
        RotationOverride
    }
}
