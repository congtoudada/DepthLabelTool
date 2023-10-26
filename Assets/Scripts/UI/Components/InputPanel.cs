/****************************************************************************
 * 2023.10 LAPTOP-CONG
 ****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.Networking;
using Debug = System.Diagnostics.Debug;

namespace GJFramework
{
	public enum PY_RUNMODE
	{
		VIS = 0,
		APPEND = 1,
		VERIFY = 2,
		TEMPLATE = 3
	}
	
	public partial class InputPanel : UIComponent
	{
		private DepthLabelPanel _controller;
		private LabelResultJson _jsonObj = new LabelResultJson();
		private string json_path;
		private IDepthLabelModel mModel;
		private const string pattern = @"\d+"; // 匹配一个或多个数字  
		private void Start()
		{
			VisBtn.onClick.AddListener(() =>
			{
				WriteToLocal(json_path);
				CallPythonScript(json_path, PY_RUNMODE.VIS);
				ChangeMainImg();	// 更换主图
			});
			SaveBtn.onClick.AddListener(() =>
			{
				WriteToLocal(json_path);
				CallPythonScript(json_path, PY_RUNMODE.APPEND);
			});
			VerifyBtn.onClick.AddListener(() =>
			{
				WriteToLocal(json_path);
				CallPythonScript(json_path, PY_RUNMODE.VERIFY);
				//从temp/bmpFilename.txt里取标记结果
				string path = _jsonObj.bmpFilename.GetFileNameWithoutExtend() + ".txt";
				path = Path.Combine(mModel.DataDir.Value, "temp", path);
				string count = File.ReadAllLines(path)[0];
				_controller.SetTip("物体个数: " + count, 3.0f); //显示3s
				ChangeMainImg();	// 更换主图
			});
			TemplateBtn.onClick.AddListener(() =>
			{
				WriteToLocal(json_path);
				CallPythonScript(json_path, PY_RUNMODE.TEMPLATE);
			});
			ClearBtn.onClick.AddListener(() =>
			{
				length.text = "";
				height.text = "";
				width.text = "";
				x_center.text = "";
				y_bottom.text = "";
				ry_ind.text = "";
				z_center.text = "";
			});
		}

		public void SetCls(int cls_idx)
		{
			cls_id.text = cls_idx.ToString();
			_jsonObj.cls_id = cls_idx;
		}

		public void Init(DepthLabelPanel controller, string bmpFilename, int cls_idx, int left=0, int top=0, int right=0, int bottom=0)
		{
			this._controller = controller;
			mModel = _controller.GetModel<IDepthLabelModel>();
			Title.text = bmpFilename;
			
			_jsonObj.rootDir = mModel.DataDir.Value;
			_jsonObj.bmpFilename = bmpFilename;
			_jsonObj.cls_id = cls_idx;
			_jsonObj.left = left;
			_jsonObj.top = top;
			_jsonObj.right = right;
			_jsonObj.bottom = bottom;

			length.text = mModel.Length.Value;
			height.text = mModel.Height.Value;
			width.text = mModel.Width.Value;
			x_center.text = mModel.X_center.Value;
			y_bottom.text = mModel.Y_bottom.Value;
			ry_ind.text = mModel.Ry_ind.Value;
			z_center.text = mModel.Z_center.Value;
			
			json_path = Path.Combine(mModel.DataDir.Value, "temp");
			if (!Directory.Exists(json_path))
				Directory.CreateDirectory(json_path);
			json_path = Path.Combine(json_path, bmpFilename.GetFileNameWithoutExtend() + ".json");
		}

		private void ChangeMainImg()
		{
			// 使用正则表达式提取数字部分  
			Match match = Regex.Match(_jsonObj.bmpFilename, pattern);  
  
			if (match.Success)  
			{  
				string path = match.Value;
				path += "_left.png";
				path = Path.Combine(mModel.DataDir.Value, "vis", path);
				LoadPNG(path);
			}
		}

		private void CallPythonScript(string json_path, PY_RUNMODE run_mode)
		{
			UnityEngine.Debug.Log($"[ params ] json_path: {json_path} run_mode:{(int)run_mode}");
			// 创建一个进程
			ProcessStartInfo start = new ProcessStartInfo();
			start.FileName = "python";
			start.Arguments =  $"{Application.streamingAssetsPath}/Python/function.py --json_path {json_path} --run_mode {(int) run_mode}";
			start.UseShellExecute = false;
			start.RedirectStandardOutput = true;

			// 启动进程
			Process process = new Process();
			process.StartInfo = start;
			process.Start();
			process.WaitForExit(); //阻塞
		}

		private void WriteToLocal(string json_path)
		{
			mModel.Length.Value = length.text;
			mModel.Height.Value = height.text;
			mModel.Width.Value = width.text;
			mModel.X_center.Value = x_center.text;
			mModel.Y_bottom.Value = y_bottom.text;
			mModel.Ry_ind.Value = ry_ind.text;
			mModel.Z_center.Value = z_center.text;
			
			_jsonObj.cls_id = cls_id.text.ToInt();
			_jsonObj.length = length.text;
			_jsonObj.height = height.text;
			_jsonObj.width = width.text;
			_jsonObj.x_center = x_center.text;
			_jsonObj.y_bottom = y_bottom.text;
			_jsonObj.ry_ind = ry_ind.text;
			_jsonObj.z_center = z_center.text;

			string json = JsonUtility.ToJson(_jsonObj, true);
			File.WriteAllText(json_path, json);
		}
		
		void LoadPNG(string path)
		{
			StartCoroutine(LoadTexture2D(path));
		}
        
		public IEnumerator LoadTexture2D(string path)
		{
			UnityWebRequest request = UnityWebRequestTexture.GetTexture(path);
			yield return request.SendWebRequest();

			if (request.isHttpError || request.isNetworkError)
			{  }
			else
			{
				// 这个是物体身上的组件Image
				var texture = DownloadHandlerTexture.GetContent(request);
				Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
				_controller.MainImg.sprite = sprite;
			}
		}

		protected override void OnBeforeDestroy()
		{
		}
	}
}