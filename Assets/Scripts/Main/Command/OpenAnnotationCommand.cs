/****************************************************
  文件：OpenLabelCommand.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using QFramework;
using SFB;
using UnityEngine;

namespace GJFramework
{
    public class OpenAnnotationCommand : AbstractCommand
    {
        ExtensionFilter[] extensions = new [] {
            new ExtensionFilter("文本文件", "txt" ),
        };
        
        protected override void OnExecute()
        {
            var model = this.GetModel<IDepthLabelModel>();
            string[] paths = StandaloneFileBrowser.OpenFilePanel("打开标注文件", model.AnnoPath.Value, extensions, false);
            if (paths.Length > 0)
            {
                model.AnnoPath.Value = paths[0];
            }
        }
    }
}


