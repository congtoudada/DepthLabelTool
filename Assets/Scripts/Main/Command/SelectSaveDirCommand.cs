/****************************************************
  文件：SelectSaveDirCommand.cs
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
    public class SelectSaveDirCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var model = this.GetModel<IDepthLabelModel>();
            var paths = StandaloneFileBrowser.OpenFolderPanel("选择保存目录", model.SaveDir.Value, false);
            if (paths.Length > 0)
            {
                model.SaveDir.Value = paths[0];
            }
        }
    }
}


