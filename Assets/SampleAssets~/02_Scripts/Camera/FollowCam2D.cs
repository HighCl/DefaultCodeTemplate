using System.Collections;
using UnityEngine;

namespace DefaultSetting
{
    public class FollowCam2D : MonoBehaviour
    {
        private enum ExecutableFunction
        {
            LateUpdate,
            FixedUpdate,
        }
        [SerializeField] private ExecutableFunction executableFunction = ExecutableFunction.LateUpdate;

        public Transform targetTr;
        private Transform camTr;

        [Range(2.0f, 20.0f)]
        public float distance = 10.0f;

        [Range(0.0f, 10.0f)]
        public float height = 2.0f;

        [Range(0.0f, 50.0f)]
        public float camSize = 24f;
        private float _lastCamSize = 0;

        void Awake()
        {
            camTr = GetComponent<Transform>();

            if (targetTr == null)
            {
                Debug.Log("target이 없습니다.");

                //targetTr = Managers.Game.Player.transform;
            }
            if (executableFunction == ExecutableFunction.FixedUpdate)
                StartCoroutine(CoFixedUpdate());
        }

        void LateUpdate()
        {
            if (executableFunction == ExecutableFunction.LateUpdate)
                followCamLogic();
        }


        private void followCamLogic()
        {
            Vector3 pos = targetTr.position
                             + (-targetTr.forward * distance)
                             + (Vector3.up * height);

            camTr.position = pos;       // 목표 위치까지 도달할 시간

            if (_lastCamSize != camSize)
            {
                _lastCamSize = camSize;
                camTr.GetComponent<Camera>().orthographicSize = camSize;
            }
        }

        IEnumerator CoFixedUpdate()
        {
            while (true)
            {
                followCamLogic();
                yield return new WaitForFixedUpdate();
            }
        }
    }
}