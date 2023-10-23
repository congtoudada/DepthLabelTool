/****************************************************
  文件：BoundingBox.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJFramework
{
    public struct BoundingBox
    {
        public Vector2 origin;
        public int width;
        public int height;

        public BoundingBox(Vector2 origin, int width, int height)
        {
            this.origin = origin;
            this.width = width;
            this.height = height;
        }

        public BoundingBox(Vector2 left_up, Vector2 right_bottom)
        {
            origin = left_up;
            width = (int) (right_bottom.x - left_up.x);
            height = (int)(left_up.y - right_bottom.y);
        }
    }
}


