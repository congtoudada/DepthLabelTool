using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GJFramework
{
    public class DrawZone : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public Action<PointerEventData> DragAction;
        public Action<PointerEventData> DownAction;
        public Action<PointerEventData> UpAction;

        public void OnDrag(PointerEventData eventData)
        {
            DragAction?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        { 
            DownAction?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            UpAction?.Invoke(eventData);
        }
    }
}
