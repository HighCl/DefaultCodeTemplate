using UnityEngine;

namespace DefaultSetting.Utility
{
    public static class PhysicsUtility
    {
        static readonly int DEFAULT_MAX_DISTANCE = 10000;
        static readonly Color DONT_HIT_COLOR = Color.green;
        static readonly Color HIT_COLOR = Color.red;
        static readonly float RHOMBUS_SIZE = 0.5f;

        /// <summary> WithDraw와의 빠른 전환을 위한 함수 </summary>
        public static bool Raycast(Vector3 startPos, Vector3 dir, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            bool isHit = Physics.Raycast(startPos, dir, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            return isHit;
        }

        public static bool RaycastWithDraw(Vector3 startPos, Vector3 dir, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, bool isDraw = true)
        {
            bool isHit = Physics.Raycast(startPos, dir, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);

#if UNITY_EDITOR
            if (isDraw)
            {
                maxDistance = (!isHit && maxDistance == Mathf.Infinity) ? DEFAULT_MAX_DISTANCE : maxDistance;
                Vector3 endPos = isHit ? hitInfo.point : startPos + dir.normalized * maxDistance;
                Color color = isHit ? HIT_COLOR : DONT_HIT_COLOR;
                Debug.DrawLine(startPos, endPos, color, Time.deltaTime);
            }
#endif

            return isHit;
        }

        public static RaycastHit[] RaycastAllWithDraw(Vector3 startPos, Vector3 dir, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, bool isDraw = true)
        {
            RaycastHit[] hitInfoArr = Physics.RaycastAll(startPos, dir, maxDistance, layerMask, queryTriggerInteraction);

#if UNITY_EDITOR
            if (isDraw)
            {
                maxDistance = (maxDistance == Mathf.Infinity) ? DEFAULT_MAX_DISTANCE : maxDistance;
                Vector3 endPos = startPos + dir.normalized * maxDistance;
                Debug.DrawLine(startPos, endPos, Color.green, Time.deltaTime);

                foreach (var hit in hitInfoArr)
                {
                    DebugUtility.DrawRhombus(hit.point, RHOMBUS_SIZE, Color.red, Time.deltaTime);
                }
            }
#endif

            return hitInfoArr;
        }
    }
}
