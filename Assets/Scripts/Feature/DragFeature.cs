using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GJFramework
{
    public class DragFeature : MonoBehaviour
    {
        public GameObject DragFeaturePanel;
        public Image Rect_left_up;
        public Image Rect_right_bottom;
        public Image Rect_left_bottom;
        public Image Rect_right_up;
        public Image Outline;
        public GameObject DragDetectZone;
        
        [SerializeField] private int MinTolerance = 5;
        [SerializeField] private Vector2 LeftTopPos;
        [SerializeField] private Vector2 RightBottomPos;
        [SerializeField] private Vector2 LeftBottomPos;
        [SerializeField] private Vector2 RightTopPos;
        
        private float _ratio = 0;
        private DrawZone _dz;
        private Vector2 _dragVec;
        private Vector2 _originalLTPos;
        private Vector2 _originalRBPos;
        private Vector2 result_LT;
        private Vector2 result_RT;


        // Start is called before the first frame update
        void Start()
        {

            _dz = TryGetComponent<DrawZone>(DragDetectZone);
            //InitRectAngelLine();
            _ratio = 1920.0f / Screen.width;
            _originalLTPos = Rect_left_up.rectTransform.position;
            _originalRBPos = Rect_right_bottom.rectTransform.position;

            //增加监听方法 
            _dz.DownAction = (pointerEventData) =>
            {
                LeftTopPos = pointerEventData.position;
                if (LeftTopPos.x - _originalLTPos.x < 0) LeftTopPos.x = _originalLTPos.x;
                if (LeftTopPos.y - _originalLTPos.y > 0) LeftTopPos.y = _originalLTPos.y;
            };
            _dz.DragAction = (pointerEventData) =>
            {
                _dragVec = pointerEventData.position;
                if (_dragVec.x - LeftTopPos.x < MinTolerance) _dragVec.x = LeftTopPos.x + MinTolerance;
                if (_dragVec.y - LeftTopPos.y > -MinTolerance) _dragVec.y = LeftTopPos.y - MinTolerance;
                if (_dragVec.x > _originalRBPos.x) _dragVec.x = _originalRBPos.x;
                if (_dragVec.y < _originalRBPos.y) _dragVec.y = _originalRBPos.y;
                DrawStraightLine(LeftTopPos, _dragVec);
            };
            _dz.UpAction = (pointerEventData) =>
            {
                RightBottomPos = pointerEventData.position;
                // Debug.Log("上左" + LeftTopPos);
                // Debug.Log("上右" + RightTopPos);
                // Debug.Log("下左" + LeftBottomPos);
                // Debug.Log("下右" + RightBottomPos);
                result_LT = Rect_left_up.rectTransform.anchoredPosition;
                result_LT.y = -result_LT.y;
                result_RT = Rect_right_bottom.rectTransform.anchoredPosition;
                result_RT.y = -result_RT.y;
                OnDragOverEvent?.Invoke(result_LT, result_RT);
                
                LeftTopPos = _originalLTPos;
                RightTopPos = _originalRBPos;
                DrawStraightLine(LeftTopPos, RightTopPos);
            };

            ActionKit.Repeat(-1)
                .Delay(1)
                .Callback(() =>
                {
                    _ratio = 1920.0f / Screen.width;
                }).Start(this);
        }

        //画线
        void DrawStraightLine(Vector2 a, Vector2 b)
        {
            float width = Mathf.Abs(a.x - b.x);
            width *= _ratio;
            float height = Mathf.Abs(a.y - b.y);
            height *= _ratio;
            

            LeftTopPos = a;
            RightTopPos = b;
            LeftBottomPos = new Vector2(a.x, b.y);
            RightBottomPos = new Vector2(b.x, a.y);
            
            Rect_left_up.rectTransform.position = a;
            Rect_right_bottom.rectTransform.position = b;
            Rect_left_bottom.rectTransform.position = new Vector2(a.x, b.y);
            Rect_right_up.rectTransform.position = new Vector2(b.x, a.y);

            //Pivot在Center
            // Zoom.rectTransform.position = new Vector2((a.x + b.x) / 2, (a.y + b.y) / 2);  //Pivot在Center
            // Zoom.rectTransform.sizeDelta = new Vector2(height, width);
            
            //Pivot在左上
            Outline.rectTransform.position = new Vector2(a.x, a.y);
            Outline.rectTransform.sizeDelta = new Vector2(width, height);
        }
        
        //注册检测成功回调（返回anchoredPosition）
        public event Action<Vector2, Vector2> OnDragOverEvent;
        
        //检测控制
        public void Switch(bool isOn)
        {
            DragFeaturePanel.SetActive(isOn);
        }

        public T TryGetComponent<T>(GameObject go) where T : Component
        {
            T t = go.GetComponent<T>();
            if (t == null)
            {
                t = go.AddComponent<T>();
            }

            return t;
        }
        
        
    }
}

