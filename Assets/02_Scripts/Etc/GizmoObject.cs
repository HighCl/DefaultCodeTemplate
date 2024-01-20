using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DefaultSetting
{
    public class GizmoObject : MonoBehaviour
    {
        public float radius = 1;
        public Color gizmoColor = Color.white;

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = gizmoColor;
            Handles.DrawSolidArc(transform.position, Vector3.back, transform.right, 360, 1);
        }
    #endif
    }
}
