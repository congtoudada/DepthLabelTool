/****************************************************
  文件：DepthToolModel.cs
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
    public interface IDepthLabelModel : IModel
    {
        BindableProperty<string> DataDir { get; }
        BindableProperty<string> AnnoPath { get; }
        BindableProperty<string> SaveDir { get; }
    }
    
    public class DepthLabelModel : AbstractModel,IDepthLabelModel
    {
        public BindableProperty<string> DataDir { get; } = new BindableProperty<string>();
        public BindableProperty<string> AnnoPath { get; } = new BindableProperty<string>();
        public BindableProperty<string> SaveDir { get; } = new BindableProperty<string>();
        
        protected override void OnInit()
        {
            var storage = this.GetUtility<IDepthLabelStorage>();

            DataDir.SetValueWithoutEvent(storage.LoadString(nameof(DataDir)));
            DataDir.Register(newStr =>
            {
                storage.SaveString(nameof(DataDir), newStr);
            });
            
            AnnoPath.SetValueWithoutEvent(storage.LoadString(nameof(AnnoPath)));
            AnnoPath.Register(newStr =>
            {
                storage.SaveString(nameof(AnnoPath), newStr);
            });
            
            SaveDir.SetValueWithoutEvent(storage.LoadString(nameof(SaveDir)));
            SaveDir.Register(newStr =>
            {
                storage.SaveString(nameof(SaveDir), newStr);
            });
        }


    }
}


