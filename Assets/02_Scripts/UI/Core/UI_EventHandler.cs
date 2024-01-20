using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultSetting
{
    public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        public Action<PointerEventData> OnClickHandler = null;
        public Action<PointerEventData> OnDragHandler = null;
        public Action<PointerEventData> OnEnterHandler = null;
        public Action<PointerEventData> OnExitHandler = null;
        public Action<PointerEventData> OnUpHandler = null;
        public Action<PointerEventData> OnDownHandler = null;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnClickHandler != null)
                OnClickHandler.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (OnDragHandler != null)
                OnDragHandler.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (OnEnterHandler != null)
                OnEnterHandler.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnExitHandler != null)
                OnExitHandler.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (OnUpHandler != null)
                OnUpHandler.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (OnDownHandler != null)
                OnDownHandler.Invoke(eventData);
        }
    }
}
