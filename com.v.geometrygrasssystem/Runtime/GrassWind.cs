using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GG
{
    [ExecuteInEditMode]
    public class GrassWind : MonoBehaviour
    {
        [Range(1, 50)]
        [SerializeField]
        private float windSpeed = 20;
        public float WindSpeed
        {
            get { return windSpeed; }
            set { windSpeed = value; Shader.SetGlobalFloat("_WindSpeed", windSpeed); }
        }

        [Range(1, 10)]
        [SerializeField]
        private float windWaveLength = 4;
        public float WindWaveLength
        {
            get { return windWaveLength; }
            set { windWaveLength = value; Shader.SetGlobalFloat("_WindWaveLength", (float)1 / (windWaveLength * 10)); }
        }

        [Range(0.01f, 0.2f)]
        [SerializeField]
        private float windWaveFrequency = 0.2f;
        public float WindWaveFrequency
        {
            get { return windWaveLength; }
            set { windWaveFrequency = value; Shader.SetGlobalFloat("_WindWaveFrequency", windWaveFrequency); }
        }

        Vector4 direction = Vector4.zero;

        [Header("Global Rand Wind Simulation")]
        [Space(50)]
        [Range(0.1f, 1)]
        [SerializeField]
        private float oscillation = 0.5f;
        public float Oscillation
        {
            get { return oscillation; }
            set { oscillation = value; Shader.SetGlobalFloat("_Oscillation", oscillation); }
        }

        [Range(0.01f, 5)]
        [SerializeField]
        private float oscFrequency_ = 2.4f;
        public float OscFrequency
        {
            get { return oscFrequency_; }
            set { oscFrequency_ = value; Shader.SetGlobalFloat("_OscFrequency", oscFrequency_); }
        }

        private void Start()
        {
            direction.x = transform.forward.x;
            direction.y = transform.forward.z;
            direction.Normalize();
            Shader.SetGlobalVector("_WindDirection", direction);
            Shader.SetGlobalFloat("_WindSpeed", windSpeed);
            Shader.SetGlobalFloat("_WindWaveLength", (float)1 / (windWaveLength * 10));
            Shader.SetGlobalFloat("_WindWaveFrequency", windWaveFrequency);
            Shader.SetGlobalFloat("_Oscillation", oscillation);
            Shader.SetGlobalFloat("_OscFrequency", oscFrequency_);
        }

        private void Update()
        {
            if (transform.hasChanged)
            {
                direction.x = transform.forward.x;
                direction.y = transform.forward.z;
                direction.Normalize();
                Shader.SetGlobalVector("_WindDirection", direction);
            }

        }

        private void OnValidate()
        {
            Shader.SetGlobalFloat("_WindSpeed", windSpeed);
            Shader.SetGlobalFloat("_WindWaveLength", (float)1 / (windWaveLength * 10));
            Shader.SetGlobalFloat("_WindWaveFrequency", windWaveFrequency);
            Shader.SetGlobalFloat("_Oscillation", oscillation);
            Shader.SetGlobalFloat("_OscFrequency", oscFrequency_);
        }

        private void OnDrawGizmos()
        {
            DrawArrow.ForGizmo(transform.position, transform.forward * 4);
        }

        public static class DrawArrow
        {
            public static void ForGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 1.0f, float arrowHeadAngle = 20.0f)
            {
                Gizmos.DrawRay(pos, direction);
                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 2);
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 2);
                Vector3 up = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + arrowHeadAngle, 0, 0) * new Vector3(0, 0, 2);
                Vector3 down = Quaternion.LookRotation(direction) * Quaternion.Euler(180 - arrowHeadAngle, 0, 0) * new Vector3(0, 0, 2);

                Vector3 from = pos + direction;
                Gizmos.DrawRay(from, right * arrowHeadLength);
                Gizmos.DrawRay(from, left * arrowHeadLength);
                Gizmos.DrawRay(from, up * arrowHeadLength);
                Gizmos.DrawRay(from, down * arrowHeadLength);

                Vector3 rightPos = from + (right * arrowHeadLength);
                Vector3 leftPos = from + (left * arrowHeadLength);
                Vector3 upPos = from + (up * arrowHeadLength);
                Vector3 downPos = from + (down * arrowHeadLength);

                Gizmos.DrawLine(rightPos, upPos);
                Gizmos.DrawLine(upPos, leftPos);
                Gizmos.DrawLine(leftPos, downPos);
                Gizmos.DrawLine(downPos, rightPos);
                Gizmos.DrawLine(upPos, downPos);
                Gizmos.DrawLine(leftPos, rightPos);
            }

            public static void ForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
            {
                Gizmos.color = color;
                Gizmos.DrawRay(pos, direction);

                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
                Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
            }

            public static void ForDebug(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
            {
                Debug.DrawRay(pos, direction);

                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Debug.DrawRay(pos + direction, right * arrowHeadLength);
                Debug.DrawRay(pos + direction, left * arrowHeadLength);
            }
            public static void ForDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
            {
                Debug.DrawRay(pos, direction, color);

                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Debug.DrawRay(pos + direction, right * arrowHeadLength, color);
                Debug.DrawRay(pos + direction, left * arrowHeadLength, color);
            }
        }
    }

}
