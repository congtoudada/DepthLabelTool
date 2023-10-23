/****************************************************
  文件：DepthToolApp.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace GJFramework
{
    public class DepthLabelApp : Architecture<DepthLabelApp>
    {
        protected override void Init()
        {
            //注册Model
            this.RegisterModel<IDepthLabelModel>(new DepthLabelModel());
            
            //注册存储工具
            this.RegisterUtility<IDepthLabelStorage>(new DepthLabelStorage());
        }
    }
}


