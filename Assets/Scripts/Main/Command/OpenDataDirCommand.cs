/****************************************************
  文件：OpenDirCommand.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using QFramework;
using SFB;
using UnityEngine;

namespace GJFramework
{
    public class OpenDirCommand :  AbstractCommand
    {
        protected override void OnExecute()
        {
            var model = this.GetModel<IDepthLabelModel>();
            var paths = StandaloneFileBrowser.OpenFolderPanel("选择图片目录", model.DataDir.Value, false);
            if (paths.Length > 0)
            {
                model.DataDir.Value = paths[0];
            }
        }
    }
}


