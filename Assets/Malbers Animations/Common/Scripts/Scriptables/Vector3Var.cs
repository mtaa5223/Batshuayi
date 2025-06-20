﻿using System;
using UnityEngine;

namespace MalbersAnimations.Scriptables
{
    ///<summary> V3 Scriptable Variable. Based on the Talk - Game Architecture with Scriptable Objects by Ryan Hipple  </summary>
    [CreateAssetMenu(menuName = "Malbers Animations/Variables/Vector3", order = 1000)]
    public class Vector3Var : ScriptableVar
    {
        /// <summary>The current value</summary>
        [SerializeField] private Vector3 value = Vector3.zero;

        /// <summary>Invoked when the value changes </summary>
        public Action<Vector3> OnValueChanged = delegate { };


        /// <summary> Value of the Float Scriptable variable</summary>
        public virtual Vector3 Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged(value);
#if UNITY_EDITOR
                if (debug) Debug.Log($"<B>{name} -> [<color=gray> {value} </color>] </B>", this);
#endif
            }
        }
        public float x { get => value.x; set => this.value.x = value; }
        public float y { get => value.y; set => this.value.y = value; }
        public float z { get => value.z; set => this.value.z = value; }

        public void SetValue(Vector3Var var) => Value = var.Value;
        public void SetValue(Vector3 var) => Value = var;

        public void SetValuePosition(Transform var) => Value = var.position;
        public void SetValuePosition(Component var) => Value = var.transform.position;
        public void SetValuePosition(GameObject var) => Value = var.transform.position;

        public void SetValuePositionLocal(Transform var) => Value = var.localPosition;
        public void SetValuePositionLocal(Component var) => Value = var.transform.localPosition;
        public void SetValuePositionLocal(GameObject var) => Value = var.transform.localPosition;

        public void SetValueRotation(Transform var) => Value = var.rotation.eulerAngles;
        public void SetValueRotation(Component var) => Value = var.transform.rotation.eulerAngles;
        public void SetValueRotation(GameObject var) => Value = var.transform.rotation.eulerAngles;

        public void SetValueRotationLocal(Transform var) => Value = var.localRotation.eulerAngles;
        public void SetValueRotationLocal(Component var) => Value = var.transform.localRotation.eulerAngles;
        public void SetValueRotationLocal(GameObject var) => Value = var.transform.localRotation.eulerAngles;

        public void SetValueScale(Transform var) => Value = var.lossyScale;
        public void SetValueScale(Component var) => Value = var.transform.lossyScale;
        public void SetValueScale(GameObject var) => Value = var.transform.lossyScale;

        public void SetValueScaleLocal(Transform var) => Value = var.localScale;
        public void SetValueScaleLocal(Component var) => Value = var.transform.localScale;
        public void SetValueScaleLocal(GameObject var) => Value = var.transform.localScale;


        public void SetPosition(Transform var) => var.position = Value;
        public void SetPositionLocal(Transform var) => var.localPosition = Value;
        public void SetFromTransform_Up(Transform var) => Value = var.transform.up;
        public void SetFromTransform_Down(Transform var) => Value = -var.transform.up;
        public void SetFromTransform_Forward(Transform var) => Value = var.transform.up;
        public void SetFromTransform_Backward(Transform var) => Value = -var.transform.forward;
        public void SetFromTransform_Right(Transform var) => Value = var.transform.right;
        public void SetFromTransform_Left(Transform var) => Value = -var.transform.right;

        public void SetX(float var) => value.x = var;
        public void SetY(float var) => value.y = var;
        public void SetZ(float var) => value.z = var;

        public static implicit operator Vector3(Vector3Var reference) => reference.Value;

        public static implicit operator Vector2(Vector3Var reference) => reference.Value;

    }

    [System.Serializable]
    public class Vector3Reference : ReferenceVar
    {
        public Vector3 ConstantValue = Vector3.zero;
        [RequiredField] public Vector3Var Variable;

        public Vector3Reference()
        {
            UseConstant = true;
            ConstantValue = Vector3.zero;
        }

        public Vector3Reference(bool variable)
        {
            UseConstant = !variable;

            if (!variable)
            {
                ConstantValue = Vector3.zero;
            }
            else
            {
                Variable = ScriptableObject.CreateInstance<Vector3Var>();
                Variable.Value = Vector3.zero;
            }
        }

        public Vector3Reference(Vector3 value) => Value = value;
        public Vector3Reference(float x, float y, float z) => Value = new Vector3(x, y, z);

        public Vector3 Value
        {
            get => (UseConstant || Variable == null) ? ConstantValue : Variable.Value;
            set
            {
                if (UseConstant)
                    ConstantValue = value;
                else
                    Variable.Value = value;
            }
        }

        public float x
        {
            get => UseConstant ? ConstantValue.x : Variable.x;
            set
            {
                if (UseConstant)
                    ConstantValue.x = value;
                else
                    Variable.x = value;
            }
        }

        public float y
        {
            get => UseConstant ? ConstantValue.y : Variable.y;
            set
            {
                if (UseConstant)
                    ConstantValue.y = value;
                else
                    Variable.y = value;
            }
        }

        public float z
        {
            get => UseConstant ? ConstantValue.z : Variable.z;
            set
            {
                if (UseConstant)
                    ConstantValue.z = value;
                else
                    Variable.z = value;
            }
        }

        #region Operators
        public static implicit operator Vector3(Vector3Reference reference) => reference.Value;

        public static implicit operator Vector2(Vector3Reference reference) => reference.Value;
        #endregion
    }


#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects, UnityEditor.CustomEditor(typeof(Vector3Var))]
    public class Vector3VarEditor : VariableEditor
    {
        public override void OnInspectorGUI() => PaintInspectorGUI("Vector3 Variable");
    }
#endif
}