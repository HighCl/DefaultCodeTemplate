using UnityEngine;

namespace DefaultSetting.Utility
{
    public static class Physics2DUtility
    {
        static readonly int DEFAULT_MAX_DISTANCE = 10000;
        static readonly Color DONT_HIT_COLOR = Color.green;
        static readonly Color HIT_COLOR = Color.red;
        static readonly float RHOMBUS_SIZE = 0.5f;

        /// <summary> WithDraw와의 빠른 전환을 위한 함수 </summary>
        public static RaycastHit2D Raycast(Vector3 startPos, Vector3 dir, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(startPos, dir, maxDistance, layerMask);
            return hitInfo;
        }

        public static RaycastHit2D RaycastWithDraw(Vector3 startPos, Vector3 dir, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, bool isDraw = true)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(startPos, dir, maxDistance, layerMask);

#if UNITY_EDITOR
            if (isDraw)
            {
                maxDistance = (hitInfo.collider == null && maxDistance == Mathf.Infinity) ? DEFAULT_MAX_DISTANCE : maxDistance;
                Vector3 endPos = hitInfo.collider != null ? hitInfo.point : startPos + dir.normalized * maxDistance;
                Color color = hitInfo.collider != null ? HIT_COLOR : DONT_HIT_COLOR;
                Debug.DrawLine(startPos, endPos, color, Time.deltaTime);
            }
#endif

            return hitInfo;
        }

        public static RaycastHit2D[] RaycastAllWithDraw(Vector3 startPos, Vector3 dir, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, bool isDraw = true)
        {
            RaycastHit2D[] hitInfoArr = Physics2D.RaycastAll(startPos, dir, maxDistance, layerMask);

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
