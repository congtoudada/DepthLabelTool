/****************************************************
  文件：ShowResultItem.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GJFramework
{
    public class ShowResultItem : MonoBehaviour
    {
        public TMP_Text title;
        public RectTransform rectTransform;
        
        private Image _boundingBox;
        private Color _highlightColor = new Color(209, 76, 76, 186);
        private Color _defaultColor;

        private void Start()
        {
            _boundingBox = GetComponent<Image>();
            _boundingBox.color = _defaultColor;
            // _defaultColor = _boundingBox.color;
        }

        public void Init(Vector2 origin, float widht, float height, string typeInfo, Color labelColor)
        {
            rectTransform.anchoredPosition = new Vector2(origin.x, -origin.y);
            rectTransform.sizeDelta = new Vector2(widht, height);
            title.text = typeInfo;
            _defaultColor = labelColor;
        }

        public void Highlight()
        {
            _boundingBox.DOColor(_highlightColor, 1.0f).onComplete += ()=>
            {
                _boundingBox.DOColor(_defaultColor, 1.0f).SetDelay(2.0f);
            };
        }

        public void SetDefaultColor(Color color)
        {
            _defaultColor = color;
            _boundingBox.color = _defaultColor;
        }
    }
}


