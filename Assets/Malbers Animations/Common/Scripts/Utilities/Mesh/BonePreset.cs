﻿using MalbersAnimations.Scriptables;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
    [CreateAssetMenu(menuName = "Malbers Animations/Preset/Bone", order = 200)]
    public class BonePreset : ScriptableCoroutine
    {
        [Header("Smooth BlendShapes")]
        public FloatReference BlendTime = new(1.5f);
        public AnimationCurve BlendCurve = new(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

        [Space, Header("Attributes to modify")]
        public bool positions = false;
        public bool scales = true;

        [Space, Header("Bones Properties")]
        public List<MiniTransform> Bones;



        public virtual void SmoothBlendBones(Transform root)
        {
            StartCoroutine(root, C_SmoothBlendBones(root, BlendTime, BlendCurve));
        }

        internal override void Evaluate(MonoBehaviour mono, Transform target, float time, AnimationCurve curve)
        {
            mono.StartCoroutine(C_SmoothBlendBones(target, time, curve));
        }


        private IEnumerator C_SmoothBlendBones(Transform root, float BlendTime, AnimationCurve BlendCurve)
        {
            var childs = root.GetComponentsInChildren<Transform>().ToList();

            List<MiniTransform> AnimalStartBones = new();
            List<MiniTransform> AnimalEndBones = new();
            List<Transform> AnimalBonesTransforms = new();

            AnimalStartBones.Add(new MiniTransform("Root", Vector3.zero, root.localScale));
            AnimalEndBones.Add(new MiniTransform("Root", Vector3.zero, Bones[0].Scale));
            AnimalBonesTransforms.Add(root);



            foreach (var bone in Bones)
            {
                //Transform Bone_Found = null;
                Transform Bone_Found = childs.Find(item => item.name == bone.name);

                if (Bone_Found)
                {
                    AnimalStartBones.Add(new MiniTransform(Bone_Found.name, Bone_Found.localPosition, Bone_Found.localScale));
                    AnimalEndBones.Add(bone);
                    AnimalBonesTransforms.Add(Bone_Found);
                }
            }

            float elapsedTime = 0;

            while ((BlendTime > 0) && (elapsedTime <= BlendTime))
            {
                float result = BlendCurve.Evaluate(elapsedTime / BlendTime);             //Evaluation of the curve

                //ROOT BONE
                if (scales) root.localScale = Vector3.Lerp(AnimalStartBones[0].Scale, AnimalEndBones[0].Scale, result);

                for (int i = 1; i < AnimalStartBones.Count; i++)
                {
                    var NewPos = Vector3.Lerp(AnimalStartBones[i].Position, AnimalEndBones[i].Position, result);
                    var NewScale = Vector3.Lerp(AnimalStartBones[i].Scale, AnimalEndBones[i].Scale, result);

                    var Bone_Found = AnimalBonesTransforms[i];

                    //                    Debug.Log($" name: {Bone_Found.name}: {Bone_Found.localScale} ..[{i}]");

                    if (scales) Bone_Found.localScale = NewScale;
                    if (positions) Bone_Found.localPosition = NewPos;
                }

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            Load(root);

            yield return null;

            Stop(root);
        }


        public void Load(Transform root)
        {
            var TransfBones = root.GetComponentsInChildren<Transform>().ToList(); ;

            if (Bones[0].name == "Root")
            {
                if (scales) root.localScale = Bones[0].Scale;
            }

            foreach (var bone in Bones)
            {
                var Bone_Found = TransfBones.Find(item => item.name == bone.name);

                if (Bone_Found)
                {
                    if (positions) Bone_Found.localPosition = bone.Position;
                    //if (rotations) Bone_Found.rotation = bone.rotation;
                    if (scales) Bone_Found.localScale = bone.Scale;
                }
            }
        }
    }

    [System.Serializable]
    public class MiniTransform
    {
        public string name = "bone";
        public Vector3 Position;
        //public Vector3 Rotation;
        public Vector3 Scale;

        public MiniTransform(string n, Vector3 p,/* Vector3 r,*/ Vector3 s)
        {
            name = n;
            Position = p;
            //  Rotation = r;
            Scale = s;
        }
    }

}