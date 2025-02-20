using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultSetting
{
    public class CoroutineManager : MonoBehaviour
    {
        public void Init()
        {
        }

        public void Clear()
        {
        }

        public void WaitNextFrameAndCallback(Action callback) => StartCoroutine(ExecuteNextFrame(callback));
        public void WaitTimeAndCallback(float waitTime, Action callback) => StartCoroutine(ExecuteAfterDelay(waitTime, callback));

        private IEnumerator ExecuteNextFrame(Action callback)
        {
            yield return null;
            callback?.Invoke();
        }

        private IEnumerator ExecuteAfterDelay(float waitTime, Action callback)
        {
            yield return new WaitForSeconds(waitTime);
            callback?.Invoke();
        }
    }
}
