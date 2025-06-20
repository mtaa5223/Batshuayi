﻿using System.Collections.Generic;
using UnityEngine;


namespace MalbersAnimations.Weapons
{
    /// <summary>  Uses a Line Renderer to Predict and Paint a Trajectory  </summary>
    [AddComponentMenu("Malbers/Damage/DisplayTrajectory")]

    public class MDisplayTrajectory : MonoBehaviour
    {
        private IThrower Thrower;
        [RequiredField, Tooltip("Reference for the line renderer")]
        public LineRenderer line;
        [Tooltip("Reference to Place a Transform at the End of the Line Renderer")]
        public GameObject HitPoint;
        [Tooltip("Start width of the line renderer")]
        public float StartWidth = 0.3f;//Width of Line where it starts
        [Tooltip("End width of the line renderer")]
        public float EndWidth = 0.1f;//Width of Line where it ends
        [Tooltip("Start Color of the line renderer")]
        public Color startColor = Color.blue;//Color of Line where it starts
        [Tooltip("End Color of the line renderer")]
        public Color endColor = Color.green;//Color of Line where it ends

        [Tooltip("Line renderer steps")]
        public float Step = 0.1f;
        [Tooltip("Max Steps")]
        public int MaxSteps = 50;

        private List<Vector3> Trajectory = new List<Vector3>();

        public bool ShowTrayectory { get; set; }

        private void Reset()
        {
            line = GetComponent<LineRenderer>();

            if (line == null)
                line = gameObject.AddComponent<LineRenderer>();

            Thrower = GetComponent<IThrower>();

            SetLineRenderer();
        }


        private void OnEnable()
        {
            SetLineRenderer();

            if (HitPoint && HitPoint.IsPrefab())
                HitPoint = Instantiate(HitPoint, transform);

            if (line.sharedMaterial == null)
                line.material = new Material(Shader.Find("Sprites/Default"));

            if (Thrower == null)
                Thrower = GetComponent<IThrower>();

            if (Thrower != null)
                Thrower.Predict += DisplayTraj;
        }

        private void OnDisable() { if (Thrower != null) Thrower.Predict -= DisplayTraj; }


        private void Update()
        {
            if (ShowTrayectory)
            {
                DisplayTrajectory(Thrower.AimOriginPos, Thrower.Velocity);
            }
        }

        void SetLineRenderer()
        {
            line.startWidth = StartWidth;
            line.endWidth = EndWidth;

            var gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new(startColor, 0.0f), new(endColor, 1.0f) },
                new GradientAlphaKey[] { new(startColor.a, 0.0f), new(endColor.a, 1.0f) }
            );
            line.colorGradient = gradient;
            line.useWorldSpace = true;
            line.receiveShadows = false;
            line.enabled = false;
            line.positionCount = 0;
        }



        private void DisplayTraj(bool show)
        {
            ShowTrayectory = show;

            line.enabled = show;
            if (HitPoint) HitPoint.SetActive(show);

            if (!ShowTrayectory)
            {
                line.enabled = false;
                line.positionCount = 0;
            }
        }

        public virtual void DisplayTrajectory(Vector3 Origin, Vector3 ProjectileVelocity)
        {
            if (ProjectileVelocity == Vector3.zero)
            {
                if (HitPoint) HitPoint.SetActive(false);
                line.enabled = false;
                line.positionCount = 0;
                return;
            }
            else
            {
                if (HitPoint) HitPoint.SetActive(true);
                line.enabled = true;
                Trajectory = TrajectoryPoints(Origin, ProjectileVelocity);
                DisplayRenderer();
            }
        }

        private List<Vector3> TrajectoryPoints(Vector3 start, Vector3 velocity)
        {
            var points = new List<Vector3>();
            if (Step <= 0) return points;
            points.Add(start);
            Vector3 prev = start;

            var hit = new RaycastHit() { normal = Vector3.up };

            int NoGravityStep = 0;
            float TraveledDistance = 0;

            for (int i = 1; i < MaxSteps; i++)
            {
                float time = Step * i;
                float gravityTime = Step * (i - NoGravityStep);

                Vector3 pos = (start + velocity * time) + (gravityTime * gravityTime * Thrower.Gravity / 2);

                if (Physics.Linecast(prev, pos, out hit, Thrower.Layer, Thrower.TriggerInteraction))
                {
                    if (!hit.collider.transform.SameHierarchy(Thrower.Owner.transform))
                    {
                        points.Add(hit.point);
                        break;
                    }
                }
                points.Add(pos);

               var Direction = (pos - prev);

                //Check if the gravity can be applied after distance
                if (TraveledDistance < Thrower.AfterDistance)
                {
                    TraveledDistance += Direction.magnitude;
                    NoGravityStep++;
                }

                prev = pos;
            }

            if (HitPoint)
            {
                HitPoint.transform.position = hit.point;
                HitPoint.transform.up = hit.normal;
            }
            return points;
        }


        public void DisplayRenderer()
        {
            for (int i = 1; i < Trajectory.Count; i++)              //Debug Lines for the Prediction
                Debug.DrawLine(Trajectory[i - 1], Trajectory[i], Color.yellow);

            line.positionCount = Trajectory.Count;
            line.SetPositions(Trajectory.ToArray());
        }
    }


#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(MDisplayTrajectory))]
    [UnityEditor.CanEditMultipleObjects]
    public class MDisplayTrajectoryEd : UnityEditor.Editor
    {
        UnityEditor.SerializedProperty line, StartWidth, EndWidth, startColor, endColor, Step, MaxSteps, Hit;
        public static GUIStyle StyleBlue => MTools.Style(new Color(0, 0.5f, 1f, 0.3f));

        private void OnEnable()
        {
            line = serializedObject.FindProperty("line");
            StartWidth = serializedObject.FindProperty("StartWidth");
            EndWidth = serializedObject.FindProperty("EndWidth");
            startColor = serializedObject.FindProperty("startColor");
            endColor = serializedObject.FindProperty("endColor");
            Step = serializedObject.FindProperty("Step");
            MaxSteps = serializedObject.FindProperty("MaxSteps");
            Hit = serializedObject.FindProperty("HitPoint");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            MalbersEditor.DrawDescription("Displays the Trajectory of a projectile, using a list of points");

            UnityEditor.EditorGUILayout.BeginVertical(UnityEditor.EditorStyles.helpBox);
            {
                UnityEditor.EditorGUILayout.PropertyField(Hit);
                UnityEditor.EditorGUILayout.PropertyField(line);
                UnityEditor.EditorGUILayout.BeginHorizontal();
                {
                    UnityEditor.EditorGUILayout.PropertyField(StartWidth, GUILayout.MinWidth(40));
                    UnityEditor.EditorGUIUtility.labelWidth = 50;
                    UnityEditor.EditorGUILayout.PropertyField(EndWidth, new GUIContent("End"), GUILayout.MinWidth(30));
                    UnityEditor.EditorGUIUtility.labelWidth = 0;
                }
                UnityEditor.EditorGUILayout.EndHorizontal();

                UnityEditor.EditorGUILayout.BeginHorizontal();
                {
                    UnityEditor.EditorGUILayout.PropertyField(startColor, GUILayout.MinWidth(40));
                    UnityEditor.EditorGUIUtility.labelWidth = 50;
                    UnityEditor.EditorGUILayout.PropertyField(endColor, new GUIContent("End"), GUILayout.MinWidth(30));
                    UnityEditor.EditorGUIUtility.labelWidth = 0;
                }
                UnityEditor.EditorGUILayout.EndHorizontal();



                UnityEditor.EditorGUILayout.BeginHorizontal();
                {
                    UnityEditor.EditorGUILayout.PropertyField(Step, GUILayout.MinWidth(40));
                    UnityEditor.EditorGUIUtility.labelWidth = 50;
                    UnityEditor.EditorGUILayout.PropertyField(MaxSteps, new GUIContent("Max"), GUILayout.MinWidth(30));
                    UnityEditor.EditorGUIUtility.labelWidth = 0;
                }
                UnityEditor.EditorGUILayout.EndHorizontal();

            }
            UnityEditor.EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}