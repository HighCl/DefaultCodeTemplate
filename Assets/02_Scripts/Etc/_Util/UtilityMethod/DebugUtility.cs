using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultSetting.Utility
{
    public static class DebugUtility
    {
        public static readonly string Null_TEXT = "Data is Null";
        public static StringBuilder staticSB = new StringBuilder(); //공용 StringBuilder

        private static StringBuilder printStaticSB = new StringBuilder(); //print 전용 StringBuilder

        #region DrawUtility

        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void DrawRhombus(Vector2 point, float size, Color color, float duration) => DrawRhombus((Vector3)point, size, color, duration);
        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void DrawRhombus(Vector3 point, float size, Color color, float duration)
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            DebugUtility.Assert(sceneView != null && sceneView.camera != null, "SceneView camera is not available.");
            Camera sceneCamera = sceneView.camera;

            Vector3 cameraForward = sceneCamera.transform.forward;
            Vector3 cameraUp = sceneCamera.transform.up;

            Vector3 right = Vector3.Cross(cameraUp, cameraForward).normalized * size / 2;
            Vector3 up = Vector3.Cross(cameraForward, right).normalized * size / 2;

            Vector3 leftPoint = point - right;
            Vector3 rightPoint = point + right;
            Vector3 upPoint = point + up;
            Vector3 downPoint = point - up;

            // Draw rhombus
            Debug.DrawLine(leftPoint, upPoint, color, duration);
            Debug.DrawLine(upPoint, rightPoint, color, duration);
            Debug.DrawLine(rightPoint, downPoint, color, duration);
            Debug.DrawLine(downPoint, leftPoint, color, duration);

            // Draw cross lines
            Debug.DrawLine(upPoint, downPoint, color, duration);
            Debug.DrawLine(leftPoint, rightPoint, color, duration);
        }

        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void DrawSquare(Vector2 center, float size, Color color, float duration) => DrawSquare((Vector3)center, size, color, duration);
        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void DrawSquare(Vector3 center, float size, Color color, float duration)
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            DebugUtility.Assert(sceneView != null && sceneView.camera != null, "SceneView camera is not available.");
            Camera sceneCamera = sceneView.camera;

            Vector3 cameraForward = sceneCamera.transform.forward;
            Vector3 cameraUp = sceneCamera.transform.up;

            Vector3 right = Vector3.Cross(cameraUp, cameraForward).normalized * size / 2;
            Vector3 up = Vector3.Cross(cameraForward, right).normalized * size / 2;

            Vector3 topLeft = center - right + up;
            Vector3 topRight = center + right + up;
            Vector3 bottomLeft = center - right - up;
            Vector3 bottomRight = center + right - up;

            Debug.DrawLine(topLeft, topRight, color, duration);
            Debug.DrawLine(topRight, bottomRight, color, duration);
            Debug.DrawLine(bottomRight, bottomLeft, color, duration);
            Debug.DrawLine(bottomLeft, topLeft, color, duration);
        }

        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void DrawCircle(Vector2 center, float radius, Color color, float duration) => DrawCircle((Vector3)center, radius, color, duration);
        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void DrawCircle(Vector3 center, float radius, Color color, float duration)
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            DebugUtility.Assert(sceneView != null && sceneView.camera != null, "SceneView camera is not available.");
            Camera sceneCamera = sceneView.camera;

            int segments = 36;
            float angleStep = 360f / segments;

            Vector3 cameraForward = sceneCamera.transform.forward;
            Vector3 cameraUp = sceneCamera.transform.up;

            Vector3 right = Vector3.Cross(cameraUp, cameraForward).normalized * radius;
            Vector3 up = Vector3.Cross(cameraForward, right).normalized * radius;

            Vector3 prevPoint = center + right;
            for (int i = 1; i <= segments; i++)
            {
                float angle = angleStep * i * Mathf.Deg2Rad;
                Vector3 newPoint = center +
                    (Mathf.Cos(angle) * right) +
                    (Mathf.Sin(angle) * up);
                Debug.DrawLine(prevPoint, newPoint, color, duration);
                prevPoint = newPoint;
            }
        }

        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void DrawCircleHandle(Vector2 center, float radius)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(center, Vector3.forward, radius);
        }

        #endregion
        #region DebugWrapUtility

        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void Log(object message) => Debug.Log(FormatMessage(message));

        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void LogWarning(object message) => Debug.LogWarning(FormatMessage(message));

        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void LogError(object message) => Debug.LogError(FormatMessage(message));

        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void Assert(bool condition) => Debug.Assert(condition, FormatMessage("Assertion failed"));

        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void Assert(bool condition, object message) => Debug.Assert(condition, FormatMessage(message));

        private static string FormatMessage(object message)
        {
            float timeInSeconds = Time.time;
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);
            int fractionalSeconds = Mathf.FloorToInt((timeInSeconds % 1) * 1_000_000); // 소수점 부분을 정수화 (0~999999 범위)
            string fractionalString = fractionalSeconds.ToString().PadLeft(6, '0');

            return $"{minutes}m {seconds}s {fractionalString}\n{message}";
        }

        #endregion
        #region PrintUtility

        /// <summary>
        /// 일반 클래스의 경우 ToString을 오버라이드하면 원하는 형태로 출력할 수 있음
        /// </summary>
        [System.Diagnostics.Conditional(Define.DEBUG_CONDITIONAL)]
        public static void Print<T>(this T data, string Label = null)
        {
            printStaticSB.Clear();
            AppendSB(data, Label, printStaticSB);
            Log(printStaticSB);
        }

        public static void AppendSB<T>(this T data, string Label = null, StringBuilder sb = null)
        {
            if (sb == null)
                sb = staticSB;

            if (Label != null)
                sb.AppendLine($"[{Label}]");

            if (data == null)
            {
                MakeStringAndAppendEntity(data, sb);
                return;
            }

            //Unity Class
            if (data is Array array)
            {
                AppendArray(array, sb);
            }
            else if (data is List<T> list)
            {
                AppendList(list, sb);
            }
            else if (data.GetType().IsGenericType && data.GetType().GetGenericTypeDefinition() == typeof(List<>))
            {
                var type = data.GetType();
                var listType = typeof(List<>).MakeGenericType(type.GetGenericArguments());
                var method = typeof(DebugUtility).GetMethod(nameof(AppendList), BindingFlags.NonPublic | BindingFlags.Static);
                var genericMethod = method.MakeGenericMethod(type.GetGenericArguments());
                genericMethod.Invoke(null, new object[] { data, sb });
            }
            else if (data.GetType().IsGenericType && data.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var type = data.GetType();
                var dictType = typeof(Dictionary<,>).MakeGenericType(type.GetGenericArguments());
                var method = typeof(DebugUtility).GetMethod(nameof(AppendDictionary), BindingFlags.NonPublic | BindingFlags.Static);
                var genericMethod = method.MakeGenericMethod(type.GetGenericArguments());
                genericMethod.Invoke(null, new object[] { data, sb });
            }
            else if (data is IEnumerable enumerable && !(data is string))
            {
                Type dataType = data.GetType();
                Type elementType = dataType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))?.GetGenericArguments()[0];

                if (elementType != null)
                {
                    var method = typeof(DebugUtility).GetMethod(nameof(AppendIEnumerable), BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(elementType);
                    method.Invoke(null, new object[] { enumerable, sb });
                }
                else
                {
                    sb.AppendLine("Unable to determine the element type of the IEnumerable.");
                }
            }
            else
            {
                MakeStringAndAppendEntity(data, sb);
            }
            sb.AppendLine();
        }

        //DebugExtension.ClearSB();
        public static void ClearSB()
        {
            staticSB.Clear();
        }

        // DebugExtension.PrintSB();
        public static void PrintSB()
        {
            Debug.Log($"{staticSB}");
        }

        #region Append
        private static void AppendArray(System.Array array, StringBuilder sb)
        {
            switch (array.Rank)
            {
                case 1:
                    foreach (var item in array)
                    {
                        MakeStringAndAppendEntity(item, sb);
                    }
                    break;
                case 2:
                    for (int i = 0; i < array.GetLength(0); i++)
                    {
                        for (int j = 0; j < array.GetLength(1); j++)
                        {
                            MakeStringAndAppendEntity(array.GetValue(i, j), sb);
                        }
                        sb.AppendLine();
                    }
                    break;
                default:
                    sb.Append("3차원 배열 이상: ");
                    foreach (var item in array)
                    {
                        MakeStringAndAppendEntity(item, sb);
                    }
                    break;
            }
            sb.AppendLine();
        }

        //private static void AppendList<T>(T list, StringBuilder sb) where T : List;
        private static void AppendList<T>(List<T> list, StringBuilder sb)
        {
            foreach (var item in list)
            {
                MakeStringAndAppendEntity(item, sb);
            }
            sb.AppendLine();
        }

        private static void AppendDictionary<T1, T2>(Dictionary<T1, T2> dict, StringBuilder sb)
        {
            foreach (KeyValuePair<T1, T2> keyValuePair in dict)
            {
                MakeStringAndAppendEntity($"key: {keyValuePair.Key}, value: {keyValuePair.Value}", sb);
            }
            sb.AppendLine();
        }

        private static void AppendIEnumerable<T>(IEnumerable<T> iEnum, StringBuilder sb)
        {
            foreach (var item in iEnum)
            {
                MakeStringAndAppendEntity(item, sb);
            }
            sb.AppendLine();
        }

        //TODO: 위 함수에도 데이터 담는걸 이걸로 통합해야 함.
        private static void MakeStringAndAppendEntity<T>(T data, StringBuilder sb)
        {
            if (data == null)
            {
                AppendString($"{Null_TEXT}\n", sb);
                return;
            }

            //담을 요소가 리스트인 경우
            if (data.GetType().IsGenericType && data.GetType().GetGenericTypeDefinition() == typeof(List<>))
            {
                var type = data.GetType();
                var listType = typeof(List<>).MakeGenericType(type.GetGenericArguments());
                var method = typeof(DebugUtility).GetMethod(nameof(AppendList), BindingFlags.NonPublic | BindingFlags.Static);
                var genericMethod = method.MakeGenericMethod(type.GetGenericArguments());
                genericMethod.Invoke(null, new object[] { data, sb });
                return;
            }
            else if (data is IEnumerable enumerable && !(data is string))
            {
                Type dataType = data.GetType();
                Type elementType = dataType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))?.GetGenericArguments()[0];

                if (elementType != null)
                {
                    var method = typeof(DebugUtility).GetMethod(nameof(AppendIEnumerable), BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(elementType);
                    method.Invoke(null, new object[] { enumerable, sb });
                }
                else
                {
                    sb.AppendLine("Unable to determine the element type of the IEnumerable.");
                }
            }

            //Unity Class
            else if (data is GameObject obj)
            {
                AppendString($"{obj.name}, {obj.transform.position} ", sb);
                return;
            }
            else if (data is Transform tr)
            {
                AppendString($"{tr.name} ", sb);
                return;
            }
            //Unity Struct
            else if (data is RaycastResult raycastResult)
            {
                AppendString($"RaycastResult: {raycastResult.gameObject.name} ", sb);
                return;
            }
            else if (data is RaycastHit raycastHit)
            {
                AppendString($"RaycastHit: {raycastHit.transform?.name} ", sb);
                return;
            }
            else if (data is RaycastHit2D raycastHit2D)
            {
                AppendString($"raycastHit2D: {raycastHit2D.transform?.name} ", sb);
                return;
            }
            //Override ToString
            else if (data is Vector2 vec)
            {
                AppendString($"{{{vec.x}, {vec.y}}} ", sb);
                return;
            }
            //Default ToString
            else
            {
                AppendString($"{data} ", sb);
                return;
            }
        }

        private static void AppendString(string str, StringBuilder sb)
        {
            //길면 띄어쓰기
            if (str.Length > 15)
                sb.AppendLine(str);
            //그렇지 않으면 그대로 배치
            else
                sb.Append(str);
        }
        #endregion
        #endregion
    }
}