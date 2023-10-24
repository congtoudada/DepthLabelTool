/****************************************************
  文件：BoundingBox.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GJFramework
{
    public class LabelResult : MonoBehaviour
    {
        public Button DelBtn;
        public Button HighlightBtn;
        public TMP_Text highlightText;

        public int typeIndex;
        public string typeInfo;
        public Vector2 left_up;
        public Vector2 right_bottom;
        
        private ShowResultItem showItem;

        private void Start()
        {
            DelBtn.onClick.AddListener(() =>
            {
                Destroy(showItem.gameObject);
                Destroy(this.gameObject);
            });
            
            HighlightBtn.onClick.AddListener(() =>
            {
                showItem.Highlight();
            });

            highlightText = HighlightBtn.GetComponentInChildren<TMP_Text>();
        }

        // public LabelResult init(string typeInfo, Vector2 origin, int width, int height)
        // {
        //     this.typeInfo = typeInfo;
        //     ltrb[0] = origin;
        //     ltrb[1] = origin + new Vector2(width, height);
        //     return this;
        // }

        public LabelResult init(int typeIndex, string typeInfo, Vector2 left_up, Vector2 right_bottom, ShowResultItem showItem, Color labelColor)
        {
            this.typeIndex = typeIndex;
            this.typeInfo = typeInfo;
            this.left_up = left_up;
            this.right_bottom = right_bottom;
            highlightText.text = $"{typeInfo}\n左上:({(int) left_up.x},{(int) left_up.y}) 右下:({(int)right_bottom.x},{(int)right_bottom.y})";
            this.showItem = showItem;
            this.showItem.Init(left_up, right_bottom.x - left_up.x, right_bottom.y - left_up.y, typeInfo, labelColor);
            return this;
        }

        public void OnRelease()
        {
            Destroy(showItem.gameObject);
        }

    }
}


