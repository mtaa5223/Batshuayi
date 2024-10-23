using MalbersAnimations.Scriptables;
using System;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MalbersAnimations.IK
{
    [Serializable]
    [AddTypeMenu("Generic/IK Offset")]
    public class IKGeneric : IKProcessor
    {
        public override bool RequireTargets => true;


        public List<GenericIKOffset> Bones = new();


        public int[] KeepRotationTarget;



        public override void LateUpdate(IKSet IKSet, Animator anim, int index, float weight)
        {
            Quaternion TargetRotation = Quaternion.identity;

            foreach (var bn in Bones)
            {
                var FinalWeight = weight * bn.Weight;

                if (FinalWeight <= 0) continue; //Do nothing if the weight is zero

                var Bone = IKSet.Targets[bn.BoneIndex];
                var cache = IKSet.CacheTargets[bn.BoneIndex];


                switch (bn.IK)
                {
                    case IKGenerigType.LookAt:

                        var direction = IKSet.aimer.AimDirection;

                        var angle = Vector3.Angle(anim.transform.forward, direction);

                        if (bn.LookAtLimit.maxValue != 0 && bn.LookAtLimit.minValue != 0) //Check the Limit in case there is a limit
                            FinalWeight *= angle.CalculateRangeWeight(bn.LookAtLimit.minValue, bn.LookAtLimit.maxValue);

                        if (bn.Gizmos) MDebug.DrawRay(Bone.Value.position, direction.normalized, Color.Lerp(Color.black, Color.green, FinalWeight));

                        if (FinalWeight == 0) continue; //Do nothing if the weight is zero


                        TargetRotation = Quaternion.LookRotation(direction, bn.UpVector) * Quaternion.Euler(bn.Offset);
                        break;
                    case IKGenerigType.LookAtUpDown:
                        var DirLook = IKSet.aimer.AimDirection;
                        Vector3 HorizontalRotationAxis = Vector3.Cross(bn.UpVector, DirLook).normalized;
                        Bone.Value.RotateAround(Bone.position, HorizontalRotationAxis, IKSet.aimer.VerticalAngle * -FinalWeight);
                        Bone.Value.rotation *= Quaternion.Euler(bn.Offset * FinalWeight);
                        RestoreChildRotation(bn, IKSet);
                        return;
                    case IKGenerigType.AdditiveOffset:

                        TargetRotation = cache.rotation * Quaternion.Euler(bn.Offset);

                        break;
                    case IKGenerigType.RotationOverride:
                        TargetRotation = anim.transform.rotation * Quaternion.Euler(bn.Offset);
                        break;
                    default:
                        break;
                }


                //Debug.Log("DO IK");

                Bone.Value.rotation = Quaternion.Lerp(cache.rotation, TargetRotation, FinalWeight);

                RestoreChildRotation(bn, IKSet);
            }
        }

        private void RestoreChildRotation(GenericIKOffset bn, IKSet iKSet)
        {
            //Store the bone's Child Rotation
            if (bn.KeepChildRot)
            {
                for (int i = 0; i < bn.childs.Length; i++)
                {
                    var TargetIndex = bn.childs[i];

                    iKSet.Targets[TargetIndex].Value.rotation = iKSet.CacheTargets[TargetIndex].rotation;
                }
            }
        }

        public override void Validate(IKSet set, Animator animator, int index)
        {
            var isValid = true;

            if (set.aimer == null)
            {
                Debug.LogWarning($"There's no Aimer on the IK Set. <B>[IK Processor: {name}]</B> needs an Aimer to get the Aim Direction", animator);
                isValid = false;
            }
            else
            {
                //Check for errors and Null references
                foreach (var bn in Bones)
                {
                    if (set.Targets.Length < bn.BoneIndex || set.Targets[bn.BoneIndex] == null)
                    {
                        Debug.LogWarning($"The IK Set <B>[{set.name}]</B> has no Transform set on the [Targets] array - Index [{bn.BoneIndex}]." +
                            $" <B>[IK Processor: {name}]</B> Needs an a value in Index {bn.BoneIndex}." +
                            $"Please add a reference for that index in the [Targets] array.", animator);
                        // set.active = false;

                        isValid = false;
                    }
                }
            }

            if (isValid)
            {
                Debug.Log($"<B>[IK Processor: {name}][IKGeneric]</B>  <color=yellow>[OK]</color>");
            }
        }

        public override void OnDrawGizmos(IKSet IKSet, Animator anim, float weight)
        {
#if UNITY_EDITOR && MALBERS_DEBUG
            // bool AppIsPlaying = Application.isPlaying;
            if (anim == null) return;

            foreach (var bn in Bones)
            {
                if (IKSet.Targets != null && IKSet.Targets.Length > 0 && IKSet.Targets.Length > bn.BoneIndex)
                {
                    var Bone = IKSet.Targets[bn.BoneIndex];

                    if (Bone == null || !bn.Gizmos) continue;

                    var FinalWeight = weight * bn.Weight;

                    Handles.color = new Color(0, 1, 0, 0.1f);
                    Handles.DrawSolidArc(Bone.position, bn.UpVector,
                        Quaternion.Euler(0, -bn.LookAtLimit.minValue, 0) * anim.transform.forward, bn.LookAtLimit.minValue * 2, 1);



                    Handles.color = Color.green;
                    Handles.DrawWireArc(Bone.position,
                        bn.UpVector, Quaternion.Euler(0, -bn.LookAtLimit.minValue, 0) * anim.transform.forward, bn.LookAtLimit.minValue * 2, 1);


                    Handles.color = new Color(0, 0.3f, 0, 0.2f);
                    var Maxlimit = (bn.LookAtLimit.minValue - bn.LookAtLimit.maxValue);

                    Handles.DrawSolidArc(Bone.position,
                        bn.UpVector, Quaternion.Euler(0, -(bn.LookAtLimit.minValue), 0) * anim.transform.forward, (Maxlimit), 1);

                    Handles.DrawSolidArc(Bone.position,
                        bn.UpVector, Quaternion.Euler(0, (bn.LookAtLimit.minValue), 0) * anim.transform.forward, -(Maxlimit), 1);


                    Handles.color = Color.black;

                    Handles.DrawWireArc(Bone.position,
                        bn.UpVector, Quaternion.Euler(0, -(bn.LookAtLimit.minValue), 0) * anim.transform.forward, (Maxlimit), 1);

                    Handles.DrawWireArc(Bone.position,
                        bn.UpVector, Quaternion.Euler(0, (bn.LookAtLimit.minValue), 0) * anim.transform.forward, -(Maxlimit), 1);

                }
            }
#endif
        }
    }

    [Serializable]
    public struct GenericIKOffset
    {
        [Range(0, 1)]
        public float Weight;
        [Tooltip("Bone Reference from the Targets Array the IK Offset")]
        public int BoneIndex;
        public IKGenerigType IK;
        public Vector3 Offset;

        //[Tooltip("Use the Aimer Direction to calculate the LookAt Direction")]
        //[Hide("IK", (int)IKGenerigType.LookAt)]
        //public bool UseAimDirection;


        [Hide(nameof(IK), (int)IKGenerigType.LookAt)]
        [Tooltip("Limits the Look At from the Min to Max Value")]
        public RangedFloat LookAtLimit;

        [Hide(nameof(IK), (int)IKGenerigType.LookAt)]
        public UpVectorType upVector;
        [Hide(nameof(upVector), (int)UpVectorType.Local)]
        public Vector3 LocalUp;
        [Hide(nameof(upVector), (int)UpVectorType.Global)]
        public Vector3Var WorldUp;


        [Tooltip("Restore the Child bone's rotations after the IK is applied to the bone")]
        public bool KeepChildRot;


        [Hide(nameof(KeepChildRot), (int)UpVectorType.Global)]
        public int[] childs;

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

    public enum IKGenerigType
    {
        [InspectorName("Local At (Aim)")]
        LookAt,
        LookAtUpDown,
        [InspectorName("Local Rotation Additive")]
        AdditiveOffset,
        [InspectorName("Local Rotation Override")]
        RotationOverride
    }
}
