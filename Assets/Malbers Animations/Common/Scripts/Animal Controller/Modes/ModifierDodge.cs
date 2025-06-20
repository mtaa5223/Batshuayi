﻿using MalbersAnimations.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.Controller
{

    [CreateAssetMenu(menuName = "Malbers Animations/Modifier/Mode/Directional Dodge")]
    public class ModifierDodge : ModeModifier
    {
        [System.Serializable]
        public class DodgeDistance 
        {
            public StateID state;
            public float distance = 1;

        }
        [HelpBox]
        public string Desc ="";

        public enum DirectionDodge { TwoSides, FourSides, EightSides };

        public DirectionDodge direction = DirectionDodge.EightSides;

        [Tooltip("Apply Extra movement to the Dodge")]
        public bool MoveDodge = new BoolReference(true);

        /// <summary>How Much it will mode if Move Dodge is enabled</summary>
        [Tooltip("How Much it will mode if Move Dodge is enabled")]
        public   List<DodgeDistance> dodgeDistance = new();

        private Vector3 DodgeDirection;

        private float Distance = 0;

        public override void OnModeEnter(Mode mode)
        {
            int Ability; ;

            if (mode.Animal.UsingMoveWithDirection)
                Ability = MovewithDirection(mode);
            else
                Ability = MovewithWorld(mode);

            DodgeDirection = DodgeDirection.normalized;


            var StateDistance = dodgeDistance.Find(x => x.state == mode.Animal.ActiveStateID);

            if (StateDistance != null)
                Distance = StateDistance.distance;


            mode.AbilityIndex = Ability; //Sent to the Mode which ability to play
            //Debug.Log("Ability"+Ability);
            //Debug.Log("MovementAxis" + MovementAxis);
        }

      

        private int MovewithDirection(Mode mode)
        {
            var AxisRaw = mode.Animal.Move_Direction;


            float angle = Vector3.Angle(mode.Animal.Forward, AxisRaw);          //Get The angle
            bool left = Vector3.Dot(mode.Animal.Right, AxisRaw) < 0;            //Calculate which directions comes the hit Left or right


            

            angle = !left ? angle : angle * -1;

            switch (direction)
            {
                case DirectionDodge.TwoSides:
                    DodgeDirection = left ? Vector3.left : Vector3.right;
                    return left ? 1 : 2;
                case DirectionDodge.FourSides:
                    if (Mathf.Abs(angle) < 45)
                    {
                        DodgeDirection = Vector3.forward;
                        return  1;        //Use Dodge Front
                    }
                    else if (angle > 45 && angle <= 135)
                    {
                        DodgeDirection = Vector3.right;
                        return 2;   //Use Dodge Right
                    }
                    else if (Mathf.Abs( angle) > 135)
                    {
                        DodgeDirection = Vector3.back;
                        return 3;   //Use Dodge Back
                    }
                    else //if (angle > -135 && angle <= -45)
                    {
                        DodgeDirection = Vector3.left;
                        return 4;   //Use Dodge Left
                    }
                case DirectionDodge.EightSides:
                    if (Mathf.Abs(angle) < 22.5f)
                    {
                        DodgeDirection = Vector3.forward;
                        return 1;        //Use Dodge Front
                    }
                    else if (angle > 22.5f && angle <= 67.5f)
                    {
                        DodgeDirection = (Vector3.forward + Vector3.right).normalized;
                        return 2;   //Use Dodge Right
                    }
                    else if (angle > 67.5f && angle <= 112.5f)
                    {
                        DodgeDirection = Vector3.right;
                        return 3;   //Use Dodge Back
                    }
                    else if (angle > 112.5f && angle <= 157.5f)
                    {
                        DodgeDirection = (Vector3.back + Vector3.right).normalized;
                        return 4;   //Use Dodge Left
                    }
                    else if (Mathf.Abs(angle) > 157.5f)
                    {
                        DodgeDirection = (Vector3.back);
                        return 5;   //Use Dodge Left
                    }
                    else if (angle < -112.5f && angle >= -157.5f)
                    {
                        DodgeDirection = (Vector3.back + Vector3.left).normalized;
                        return 6;   //Use Dodge Left
                    }
                    else if (angle < -67.5f && angle >= -112.5f)
                    {
                        DodgeDirection = Vector3.left;
                        return 7;   //Use Dodge Left
                    }
                    else  
                    {
                       DodgeDirection = (Vector3.forward + Vector3.left).normalized;
                       return 8;   //Use Dodge Left
                    }
                default:
                    return 0;
            }
        }

        private int MovewithWorld(Mode mode)
        {
            int Ability = 0;

            var MovementAxis = mode.Animal.MovementAxisRaw;
            var left = MovementAxis.x < 0;
            var right = MovementAxis.x > 0;
            var front = MovementAxis.z > 0;
            var back = MovementAxis.z < 0;

            switch (direction)
            {
                case DirectionDodge.TwoSides:
                    Ability = left ? 1 : 2;
                    DodgeDirection = left ? Vector3.left : Vector3.right;
                    break;
                case DirectionDodge.FourSides:
                    if (front)
                    {
                        Ability = 1;        //Use Dodge Front
                        DodgeDirection = Vector3.forward;
                    }
                    else if (right)
                    {
                        Ability = 2;   //Use Dodge Right
                        DodgeDirection = Vector3.right;
                    }
                    else if (back)
                    {
                        Ability = 3;   //Use Dodge Back
                        DodgeDirection = Vector3.back;
                    }
                    else if (left)
                    {
                        Ability = 4;   //Use Dodge Left
                        DodgeDirection = Vector3.left;
                    }

                    break;
                case DirectionDodge.EightSides:

                    left = MovementAxis.x < 0 && MovementAxis.z == 0;
                    right = MovementAxis.x > 0 && MovementAxis.z == 0;
                    front = MovementAxis.z > 0 && MovementAxis.x == 0;
                    back = MovementAxis.z < 0 && MovementAxis.x == 0;

                    var FrontRight = MovementAxis.z > 0 && MovementAxis.x > 0;
                    var FrontLeft = MovementAxis.z > 0 && MovementAxis.x < 0;
                    var BackRight = MovementAxis.z < 0 && MovementAxis.x > 0;
                    var BackLeft = MovementAxis.z < 0 && MovementAxis.x < 0;

                    if (front)
                    {
                        Ability = 1;        //Use Dodge Front
                        DodgeDirection = Vector3.forward;
                    }
                    else if (FrontRight)
                    {
                        Ability = 2;   //Use Dodge Right
                        DodgeDirection = (Vector3.forward + Vector3.right).normalized;
                    }
                    else if (right)
                    {
                        Ability = 3;   //Use Dodge Back
                        DodgeDirection = Vector3.right;
                    }
                    else if (BackRight)
                    {
                        Ability = 4;   //Use Dodge Left
                        DodgeDirection = (Vector3.back + Vector3.right).normalized;
                    }
                    else if (back)
                    {
                        Ability = 5;   //Use Dodge Left
                        DodgeDirection = (Vector3.back);
                    }
                    else if (BackLeft)
                    {
                        Ability = 6;   //Use Dodge Left
                        DodgeDirection = (Vector3.back + Vector3.left).normalized;
                    }
                    else if (left)
                    {
                        Ability = 7;   //Use Dodge Left
                        DodgeDirection = Vector3.left;
                    }
                    else if (FrontLeft)
                    {
                        Ability = 8;   //Use Dodge Left
                        DodgeDirection = (Vector3.forward + Vector3.left).normalized;
                    }

                    break;
                default:
                    break;
            }

            return Ability;
        }

        public override void OnModeMove(Mode mode)
        {
            if (MoveDodge)
            {
                var animal = mode.Animal;
                animal.transform.position += animal.DeltaTime * Distance * animal.transform.TransformDirection(DodgeDirection);
            }
        }

        private void OnValidate()
        {
            switch (direction)
            {
                case DirectionDodge.TwoSides:
                    Desc = "The Dodge will be done with Horizontal Sides\n" +
                        "Ability 1: Dodge Left\n" +
                        "Ability 2: Dodge Right";
                    break;
                case DirectionDodge.FourSides:
                    Desc = "The Dodge will be done with Horizontal and Vertical Sides\n" +
                        "Ability 1: Dodge Front\n" +
                        "Ability 2: Dodge Right\n" +
                        "Ability 3: Dodge Back\n" +
                        "Ability 4: Dodge Left\n";
                    break;
                case DirectionDodge.EightSides:
                    Desc = "The Dodge will be done with Vertical, Horizontal and Diagonal Sides\n" +
                        "Ability 1: Dodge Front\n" +
                        "Ability 2: Dodge Front Left\n" +
                        "Ability 3: Dodge Left\n" +
                        "Ability 4: Dodge Back Left\n" +
                        "Ability 5: Dodge Back\n" +
                        "Ability 6: Dodge Back Right\n" +
                        "Ability 7: Dodge Right\n" +
                        "Ability 8: Dodge Front Right";
                    break;
                default:
                    break;
            }
        }
    }
}