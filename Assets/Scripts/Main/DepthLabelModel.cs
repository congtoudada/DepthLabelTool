/****************************************************
  文件：DepthToolModel.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/

using System;
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
        BindableProperty<string> Length { get; }
        BindableProperty<string> Height { get; }
        BindableProperty<string> Width { get; }
        BindableProperty<string> X_center { get; }
        BindableProperty<string> Y_bottom { get; }
        BindableProperty<string> Ry_ind { get; }
        BindableProperty<string> Z_center { get; }
    }
    
    public class DepthLabelModel : AbstractModel,IDepthLabelModel
    {
        public BindableProperty<string> DataDir { get; } = new BindableProperty<string>();
        public BindableProperty<string> AnnoPath { get; } = new BindableProperty<string>();
        public BindableProperty<string> SaveDir { get; } = new BindableProperty<string>();
        public BindableProperty<string> Length { get; } = new BindableProperty<string>();
        public BindableProperty<string> Height { get; } = new BindableProperty<string>();
        public BindableProperty<string> Width { get; } = new BindableProperty<string>();
        public BindableProperty<string> X_center { get; } = new BindableProperty<string>();
        public BindableProperty<string> Y_bottom { get; } = new BindableProperty<string>();
        public BindableProperty<string> Ry_ind { get; } = new BindableProperty<string>();
        public BindableProperty<string> Z_center { get; } = new BindableProperty<string>();
        
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
            
            Length.SetValueWithoutEvent(storage.LoadString(nameof(Length)));
            Length.Register(newStr =>
            {
                storage.SaveString(nameof(Length), newStr);
            });
            
            Height.SetValueWithoutEvent(storage.LoadString(nameof(Height)));
            Height.Register(newStr =>
            {
                storage.SaveString(nameof(Height), newStr);
            });
            
            Width.SetValueWithoutEvent(storage.LoadString(nameof(Width)));
            Width.Register(newStr =>
            {
                storage.SaveString(nameof(Width), newStr);
            });
            
            X_center.SetValueWithoutEvent(storage.LoadString(nameof(X_center)));
            X_center.Register(newStr =>
            {
                storage.SaveString(nameof(X_center), newStr);
            });
            
            Y_bottom.SetValueWithoutEvent(storage.LoadString(nameof(Y_bottom)));
            Y_bottom.Register(newStr =>
            {
                storage.SaveString(nameof(Y_bottom), newStr);
            });
            
            Ry_ind.SetValueWithoutEvent(storage.LoadString(nameof(Ry_ind)));
            Ry_ind.Register(newStr =>
            {
                storage.SaveString(nameof(Ry_ind), newStr);
            });
            
            Z_center.SetValueWithoutEvent(storage.LoadString(nameof(Z_center)));
            Z_center.Register(newStr =>
            {
                storage.SaveString(nameof(Z_center), newStr);
            });
        }


    }
}


