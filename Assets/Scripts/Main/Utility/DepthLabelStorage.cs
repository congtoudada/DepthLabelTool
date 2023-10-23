/****************************************************
  文件：DepthToolStorage.cs
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
    public interface IDepthLabelStorage : IUtility
    {
        void SaveString(string key, string value);
        string LoadString(string key, string defaultValue = "");
    }
    
    public class DepthLabelStorage : IDepthLabelStorage
    {
        public void SaveString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public string LoadString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }
    }
}


