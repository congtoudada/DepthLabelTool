/****************************************************
  文件：DepthLabelConvert.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Text;
using QFramework;
using UnityEngine;

namespace GJFramework
{
    public interface IDepthLabelConvert : IUtility
    {
        string ConcatePath(string[] paths);
    }
    
    public class DepthLabelConvert : IDepthLabelConvert
    {
        private StringBuilder _path = new StringBuilder();
        public string ConcatePath(string[] paths)
        {
            if (paths.Length == 0) {
                return "";
            }
            
            // _path.Clear();
            // foreach (var p in paths) {
            //     _path += p + "\n";
            //     _path.Append(p)
            // }

            return _path.ToString();
        }
    }
}


