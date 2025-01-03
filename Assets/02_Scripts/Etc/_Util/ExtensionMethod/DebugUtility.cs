using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

        public static void DrawRhombus(Vector2 point, float size, Color color, float duration)
        {
            Vector2 left = point + Vector2.left * size / 2;
            Vector2 right = point + Vector2.right * size / 2;
            Vector2 up = point + Vector2.up * size / 2;
            Vector2 down = point + Vector2.down * size / 2;

            // Draw rhombus
            Debug.DrawLine(left, up, color, duration);
            Debug.DrawLine(up, right, color, duration);
            Debug.DrawLine(right, down, color, duration);
            Debug.DrawLine(down, left, color, duration);

            // Draw cross lines
            Debug.DrawLine(up, down, color, duration);
            Debug.DrawLine(left, right, color, duration);
        }

        public static void DrawSquare(Vector2 center, float size, Color color, float duration)
        {
            Vector2 topLeft = center + new Vector2(-size / 2, size / 2);
            Vector2 topRight = center + new Vector2(size / 2, size / 2);
            Vector2 bottomLeft = center + new Vector2(-size / 2, -size / 2);
            Vector2 bottomRight = center + new Vector2(size / 2, -size / 2);

            // Draw square
            Debug.DrawLine(topLeft, topRight, color, duration);
            Debug.DrawLine(topRight, bottomRight, color, duration);
            Debug.DrawLine(bottomRight, bottomLeft, color, duration);
            Debug.DrawLine(bottomLeft, topLeft, color, duration);
        }

        #endregion
        #region DebugWrapUtility

        public static void Log(object message) => Debug.Log(FormatMessage(message));
        public static void LogWarning(object message) => Debug.LogWarning(FormatMessage(message));
        public static void LogError(object message) => Debug.LogError(FormatMessage(message));
        public static void Assert(bool condition) => Debug.Assert(condition, FormatMessage("Assertion failed"));
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
        //[System.Diagnostics.Conditional("A")]
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