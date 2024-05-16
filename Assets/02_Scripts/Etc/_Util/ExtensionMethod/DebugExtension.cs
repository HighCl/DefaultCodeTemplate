using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;


namespace DefaultSetting.Utility
{
    public static class DebugExtension
    {
        public static StringBuilder staticSB = new StringBuilder();

        public static void Print<T>(this T data)
        {
            var sb = new StringBuilder();
            AppendSB(data, sb: sb);
            Debug.Log(sb.ToString());
        }

        public static void AppendSB<T>(this T data, string name = null, StringBuilder sb = null)
        {
            if (sb == null)
                sb = staticSB;

            if (name != null)
                sb.AppendLine($"[{name}]");

            if (data is Array array)
            {
                AppendArray(array, sb);
            }
            else if (data.GetType().IsGenericType && data.GetType().GetGenericTypeDefinition() == typeof(List<>))
            {
                var type = data.GetType();
                var listType = typeof(List<>).MakeGenericType(type.GetGenericArguments());
                var method = typeof(DebugExtension).GetMethod(nameof(AppendList), BindingFlags.NonPublic | BindingFlags.Static);
                var genericMethod = method.MakeGenericMethod(type.GetGenericArguments());
                genericMethod.Invoke(null, new object[] { data, sb });
            }
            else if (data.GetType().IsGenericType && data.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var type = data.GetType();
                var dictType = typeof(Dictionary<,>).MakeGenericType(type.GetGenericArguments());
                var method = typeof(DebugExtension).GetMethod(nameof(AppendDictionary), BindingFlags.NonPublic | BindingFlags.Static);
                var genericMethod = method.MakeGenericMethod(type.GetGenericArguments());
                genericMethod.Invoke(null, new object[] { data, sb });
            }

            else
            {
                sb.Append(data);
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
        private static void AppendList<T>(List<T> list, StringBuilder sb)
        {
            foreach (var item in list)
            {
                sb.AppendLine($"{item}");
            }
        }

        private static void AppendDictionary<T1, T2>(Dictionary<T1, T2> dict, StringBuilder sb)
        {
            foreach (KeyValuePair<T1, T2> keyValuePair in dict)
            {
                sb.AppendLine($"key: {keyValuePair.Key}, value: {keyValuePair.Value}");
            }
        }

        private static void AppendArray(System.Array array, StringBuilder sb)
        {
            switch (array.Rank)
            {
                case 1:
                    foreach (var item in array)
                    {
                        sb.Append($"{item} ");
                    }
                    break;
                case 2:
                    for (int i = 0; i < array.GetLength(0); i++)
                    {
                        for (int j = 0; j < array.GetLength(1); j++)
                        {
                            sb.Append($"{array.GetValue(i, j)} ");
                        }
                        sb.AppendLine();
                    }
                    break;
                default:
                    sb.Append("3차원 배열 이상: ");
                    foreach (var item in array)
                    {
                        sb.Append($"{item} ");
                    }
                    break;
            }
        }
        #endregion
    }
}