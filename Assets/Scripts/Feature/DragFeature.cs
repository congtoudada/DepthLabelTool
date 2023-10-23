using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GJFramework
{
    public class DragFeature : MonoBehaviour
    {
        public Image Rect_left_up;
        public Image Rect_right_bottom;
        public Image Rect_left_bottom;
        public Image Rect_right_up;

        public Image Zoom;

        public Vector2 LeftTopPos;
        public Vector2 RightBottomPos;
        public Vector2 LeftBottomPos;
        public Vector2 RightTopPos;
        public GameObject _Panel;
        DrawZone dz;

        // Start is called before the first frame update
        void Start()
        {

            dz = TryGetComponent<DrawZone>(_Panel);
            //InitRectAngelLine();

            //增加监听方法 
            dz.DownAction = (PointerEventData) => { LeftTopPos = PointerEventData.position; };
            dz.DragAction = (PointerEventData) => { DrawStraightLine(LeftTopPos, PointerEventData.position); };
            dz.UpAction = (PointerEventData) =>
            {
                RightBottomPos = PointerEventData.position;

                // Debug.Log("上左" + LeftTopPos);
                // Debug.Log("上右" + RightTopPos);
                // Debug.Log("下左" + LeftBottomPos);
                // Debug.Log("下右" + RightBottomPos);
            };
        }

        //画线
        void DrawStraightLine(Vector2 a, Vector2 b)
        {
            float height = Mathf.Abs(a.x - b.x);
            float width = Mathf.Abs(a.y - b.y);

            LeftTopPos = a;
            RightTopPos = b;
            LeftBottomPos = new Vector2(a.x, b.y);
            RightBottomPos = new Vector2(b.x, a.y);

            Rect_left_up.rectTransform.position = a;
            Rect_right_bottom.rectTransform.position = b;
            Rect_left_bottom.rectTransform.position = new Vector2(a.x, b.y);
            Rect_right_up.rectTransform.position = new Vector2(b.x, a.y);


            Zoom.rectTransform.position = new Vector2((a.x + b.x) / 2, (a.y + b.y) / 2);
            Zoom.rectTransform.sizeDelta = new Vector2(height, width);


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

