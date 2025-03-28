using DefaultSetting.Utility;
using System.Collections;
using UnityEngine;

namespace DefaultSetting
{
    public class ResourceManager : MonoBehaviour
    {
        private T LoadOriginal<T>(string path) where T : Object
        {
            T original = Load<T>($"Prefabs/{path}");
            if (original == null)
            {
#if UNITY_EDITOR
                string prevFuncName = new System.Diagnostics.StackFrame(1, true).GetMethod().Name;
                string prevClassName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().ReflectedType.Name;
                DebugUtility.Log($"Failed to load prefab : {(string.IsNullOrEmpty(path) ? "None" : path)}\nprevFunc : {prevFuncName}\nprefClassName : {prevClassName}\n");
#endif
                return null;
            }
            return original;
        }

        public T Load<T>(string path) where T : Object
        {
            if (typeof(T) == typeof(GameObject))
            {
                string name = path;
                int index = name.LastIndexOf('/');
                if (index >= 0)
                    name = name.Substring(index + 1);

                GameObject go = Managers.Pool.GetOriginal(name);
                if (go != null)
                    return go as T;
            }

            //TODO: Instantiate를 거치지 않고 바로 Load하는 경우 못 찾았을 때 체크가 불가능

            return Resources.Load<T>(path);
        }

        #region Path_GameObject
        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject original = LoadOriginal<GameObject>(path);
            return InstantiateInternal(original, parent);
        }

        public GameObject Instantiate(string path, Vector3 position, Quaternion rotation = default)
        {
            GameObject original = LoadOriginal<GameObject>(path);
            return InstantiateInternal(original, null, position, rotation);
        }
        #endregion

        #region Path_Component
        public T Instantiate<T>(string path, Transform parent = null) where T : Component
        {
            GameObject original = LoadOriginal<GameObject>(path);
            GameObject instance = InstantiateInternal(original, parent);
            return instance?.GetComponent<T>();
        }

        public T Instantiate<T>(string path, Vector3 position, Quaternion rotation = default) where T : Component
        {
            GameObject original = LoadOriginal<GameObject>(path);
            GameObject instance = InstantiateInternal(original, null, position, rotation);
            return instance?.GetComponent<T>();
        }
        #endregion

        #region GameObject
        public GameObject Instantiate(GameObject original, Transform parent = null)
        {
            return InstantiateInternal(original, parent);
        }

        public GameObject Instantiate(GameObject original, Vector3 position = default, Quaternion rotation = default)
        {
            return InstantiateInternal(original, null, position, rotation);
        }
        #endregion

        #region Component
        public new T Instantiate<T>(T original, Transform parent = null) where T : Component
        {
            GameObject go = InstantiateInternal(original?.gameObject, parent);
            return go?.GetComponent<T>();
        }

        public new T Instantiate<T>(T original, Vector3 position, Quaternion rotation = default) where T : Component
        {
            GameObject go = InstantiateInternal(original?.gameObject, null, position, rotation);
            return go?.GetComponent<T>();
        }
        #endregion

        private GameObject InstantiateInternal(GameObject original, Transform parent = null, Vector3 position = default, Quaternion rotation = default)
        {
            if (original == null)
                return null;

            GameObject go = (original.GetComponent<Poolable>() != null)
                ? Managers.Pool.Pop(original, parent).gameObject
                : Object.Instantiate(original, parent);

            go.transform.SetPositionAndRotation(position, rotation);
            go.name = original.name;
            return go;
        }

        public void Destroy(GameObject go, float destroyTime = 0)
        {
            StartCoroutine(CoDestroy(go, destroyTime));
        }

        IEnumerator CoDestroy(GameObject go, float destroyTime)
        {
            if (go == null)
                yield break;

            yield return new WaitForSeconds(destroyTime);

            if (go == null)
                yield break;

            Poolable poolable = go.GetComponent<Poolable>();
            if (poolable != null)
            {
                Managers.Pool.Push(poolable);
                yield break;
            }


            Object.Destroy(go);
        }

    }
}
