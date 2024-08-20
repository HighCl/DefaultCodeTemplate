using UnityEngine;

namespace DefaultSetting
{
    public static class PhysicsExtensions
    {
        static readonly int DEFAULT_MAX_DISTANCE = 10000;
        static readonly Color DONT_HIT_COLOR = Color.green;
        static readonly Color HIT_COLOR = Color.red;

        public static RaycastHit2D Raycast2DWithDraw(Vector3 startPos, Vector3 dir, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(startPos, dir, maxDistance, layerMask);

#if UNITY_EDITOR
            if (hitInfo.collider == null && maxDistance == Mathf.Infinity)
                maxDistance = DEFAULT_MAX_DISTANCE;

            Vector3 endPos = hitInfo.collider != null ? hitInfo.point : startPos + dir * maxDistance;
            Color color = hitInfo.collider != null ? HIT_COLOR : DONT_HIT_COLOR;

            Debug.DrawLine(startPos, endPos, color, Time.deltaTime);
#endif

            return hitInfo;
        }


        public static bool RaycastWithDraw(Vector3 startPos, Vector3 dir, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            bool isHit = Physics.Raycast(startPos, dir, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);

#if UNITY_EDITOR
            if (!isHit && maxDistance == Mathf.Infinity)
                maxDistance = DEFAULT_MAX_DISTANCE;

            Vector3 endPos = isHit ? hitInfo.point : startPos + dir * maxDistance;
            Color color = isHit ? HIT_COLOR : DONT_HIT_COLOR;
            Debug.DrawLine(startPos, endPos, color, Time.deltaTime);
#endif

            return isHit;
        }
    }
}
