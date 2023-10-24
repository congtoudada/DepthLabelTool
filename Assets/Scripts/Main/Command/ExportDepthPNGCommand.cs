using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using QFramework;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GJFramework
{
    public class ExportDepthPNGCommand : AbstractCommand
    {
        private DepthLabelPanel _controller;
        private LabelResultJson _labelResultJson;

        public ExportDepthPNGCommand(DepthLabelPanel contoller, LabelResultJson labelResultJson)
        {
            _controller = contoller;
            _labelResultJson = labelResultJson;
        }
        
        protected override void OnExecute()
        {
            var model = this.GetModel<IDepthLabelModel>();
            // 创建一个进程
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "python";
            string json_path = Path.Combine(model.SaveDir.Value,
                _labelResultJson.bmpFilename.GetFileNameWithoutExtend() + ".json");
            string img_dir = model.DataDir.Value;
            string export_dir = model.SaveDir.Value;
            string label_dir = model.SaveDir.Value;
            start.Arguments =  $"{Application.streamingAssetsPath}/Python/depth_tool.py --json_path {json_path} --img_dir {img_dir} --export_dir {export_dir} --label_dir {label_dir}";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;

            // 启动进程
            Process process = new Process();
            process.StartInfo = start;
            process.Start();
            
            // Texture2D ref2D = _controller.MainImg.sprite.texture;
            // Texture2D export2D = new Texture2D(ref2D.width, ref2D.height, TextureFormat.R16, false);
            // for (int i = 0; i < export2D.width; i++)
            // {
            //     for (int j = 0; j < export2D.height; j++)
            //     {
            //         export2D.SetPixel(i, j, Color.black);                    
            //     }
            // }
            //
            // string exportFilename =  _labelResultJson.bmpFilename.GetFileNameWithoutExtend() + ".png";
            // int width, height;
            // int typeCount = _controller.TypeDropdown.options.Count;
            // List<Color> depthList = new List<Color>(typeCount);
            // for (int i = 0; i < typeCount; i++)
            // {
            //     Match match = Regex.Match(_controller.TypeDropdown.options[i].text, pattern);  
            //
            //     if (match.Success)  
            //     {  
            //         float number = match.Value.ToFloat();  
            //         depthList.Add(new Color(3 * number / 255.0f, 3 * number / 255.0f, 3 * number / 255.0f, 1.0f));
            //     }  
            // }
            // foreach (var result in _labelResultJson.resultList)
            // {
            //     width = result.rb_x - result.lt_x;
            //     height = result.rb_y - result.lt_y;
            //
            //     for (int i = result.lt_x; i <= width; i++)
            //     {
            //         for (int j = result.lt_y; j <= height; j++)
            //         {
            //             export2D.SetPixel(i, j, Color.white);
            //         }
            //     }
            // }
            //
            // var model = this.GetModel<IDepthLabelModel>();
            // var bytes = export2D.EncodeToPNG();
            // var path = Path.Combine(model.SaveDir.Value, exportFilename);
            // File.WriteAllBytes(path, bytes);
        }
    }
}