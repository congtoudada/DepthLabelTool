using System.Collections;
using System.Collections.Generic;
using System.IO;
using B83.Image.BMP;
using GJFramework;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using Unity.VisualScripting;

namespace GJFramework
{
	public class DepthLabelPanelData : UIPanelData
	{
	}

	public enum DepthLabelPanelViewEnum
	{
		All,
		OpenDir,
		OpenAnnotation,
		SaveDir
	}

	public partial class DepthLabelPanel : UIPanel, IController
	{
		// Model
		private IDepthLabelModel mModel;
		private int currentTypeIdx = 0;
		private int currentBMPIdx = 0;
		private List<string> _infoList = new List<string>();
		private string _defaultTip = "";
		private IActionController _tipController;
		private ResLoader mResLoader = ResLoader.Allocate();
		private Transform _infoContent;
		private Color infoColor = new Color(234 / 55.0f, 138 / 255.0f, 138 / 255.0f, 255 / 255.0f);
		
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as DepthLabelPanelData ?? new DepthLabelPanelData();
			// please add init code here
			ResKit.Init();
			mModel = this.GetModel<IDepthLabelModel>();
			_defaultTip = DirtyTip.text;
			_infoContent = InfoScrollView.content;
			
			// 打开数据目录
			DirBtn.onClick.AddListener(() =>
			{
				this.SendCommand<OpenDirCommand>();
				UpdateView(DepthLabelPanelViewEnum.OpenDir);
			});
			
			// 打开标注文件
			AnnoBtn.onClick.AddListener(() =>
			{
				this.SendCommand<OpenAnnotationCommand>();
				UpdateView(DepthLabelPanelViewEnum.OpenAnnotation);
			});
			
			// 选择保存目录
			SaveDirBtn.onClick.AddListener(() =>
			{
				this.SendCommand<SelectSaveDirCommand>();
				UpdateView(DepthLabelPanelViewEnum.SaveDir);
			});
			
			// 类型选择
			TypeDropdown.onValueChanged.AddListener(idx =>
			{
				currentTypeIdx = idx;
			});
			
			//上一张
			BackBtn.onClick.AddListener(() =>
			{
				if (currentBMPIdx-1 >= 0)
				{
					SwitchInfoItem(currentBMPIdx-1);
				}
			});
			
			//下一张
			NextBtn.onClick.AddListener(() =>
			{
				if (currentBMPIdx+1 < _infoContent.childCount)
				{
					SwitchInfoItem(currentBMPIdx+1);
				}
			});
			
			UpdateView(DepthLabelPanelViewEnum.All);
		}

		private void UpdateView(DepthLabelPanelViewEnum viewEnum)
		{
			switch (viewEnum)
			{
				case DepthLabelPanelViewEnum.All:
					UpdateView(DepthLabelPanelViewEnum.OpenDir);
					UpdateView(DepthLabelPanelViewEnum.OpenAnnotation);
					UpdateView(DepthLabelPanelViewEnum.SaveDir);
					break;
				case DepthLabelPanelViewEnum.OpenDir:
					DirText.text = mModel.DataDir.Value;
					//更新InfoScrollView
					StartCoroutine(UpdateView_InfoScrollView());
					break;
				case DepthLabelPanelViewEnum.OpenAnnotation:
					AnnoText.text = mModel.AnnoPath.Value;
					//更新TypeDropDown
					UpdateView_TypeDropDown();
					break;
				case DepthLabelPanelViewEnum.SaveDir:
					SaveDirText.text = mModel.SaveDir.Value;
					break;
			}
		}
		
		//更新InfoScrollView
		private IEnumerator UpdateView_InfoScrollView()
		{
			//删除旧节点
			if (_infoContent.childCount > 0)
			{
				Transform[] children = _infoContent.GetComponentsInChildren<Transform>();
				foreach (Transform child in children)  
				{  
					if (child != _infoContent)
						Destroy(child.gameObject);
				}  
			}

			yield return null;
			string dataDir = mModel.DataDir.Value;
			if (dataDir.IsNotNullAndEmpty())
			{
				//添加新节点
				string[] files = Directory.GetFiles(dataDir, "*left_ccd*", SearchOption.TopDirectoryOnly);
				int fileIdx = 0;
				foreach (string file in files)
				{
					// 处理每个文件
					var obj = Instantiate(mResLoader.LoadSync<GameObject>("InfoItemBtn"), _infoContent, true);
					obj.name = fileIdx + "@" + file.GetFileName();
					obj.GetComponentInChildren<TMP_Text>().text = file.GetFileName();
					obj.GetComponent<Button>().onClick.AddListener(() =>
					{
						SwitchInfoItem(obj.name);
					});
					fileIdx++;
				}
				
				if (_infoContent.childCount > 0)
				{
					currentBMPIdx = 0;
					SwitchInfoItem(currentBMPIdx);
				}
			}
		}
		
		//更换Info
		private void SwitchInfoItem(string name)
		{
			//还原旧数据
			_infoContent.GetChild(currentBMPIdx).GetComponent<Image>().color = Color.white;
						
			//设置新数据
			string[] data = name.Split('@');
			Debug.Log("data: " + data[0] + "@" + data[1]);
			currentBMPIdx = data[0].ToInt();
			_infoContent.GetChild(currentBMPIdx).GetComponent<Image>().color = infoColor;
			//显示BMP
			string path = Path.Combine(mModel.DataDir.Value, data[1]);
			Texture2D texture2D = BMPLoader.LoadTexture(path);
			MainImg.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);
		}

		private void SwitchInfoItem(int idx)
		{
			// Debug.Log(_infoContent.GetChild(idx).name);
			SwitchInfoItem(_infoContent.GetChild(idx).name);
		}
		
		//更新TypeDropDown
		private void UpdateView_TypeDropDown()
		{
			// 类型选择
			TypeDropdown.options.Clear();
			_infoList.Clear();
			// TypeDropdown.options.Add(new TMP_Dropdown.OptionData("空类别"));
			if (mModel.AnnoPath.Value.IsNotNullAndEmpty())
			{
				// TypeDropdown.options.Add(new TMP_Dropdown.OptionData(""));
				using StreamReader sr = new StreamReader(mModel.AnnoPath.Value);
				string line =  sr.ReadLine();
				while (line != null)
				{
					TypeDropdown.options.Add(new TMP_Dropdown.OptionData(line));
					_infoList.Add(line);
					line = sr.ReadLine();
				}
			}
		}

		private void SetTip(string info, float duration)
		{
			if (_tipController != null)
			{
				_tipController.Deinit();
				_tipController = null;
			}
			
			_tipController = ActionKit.Sequence()
				.Callback(() => DirtyTip.text = info)
				.Delay(duration)
				.Callback(() => DirtyTip.text = _defaultTip).Start(this);
		}

		private void SetTip(string info)
		{
			DirtyTip.text = info;
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}

		public IArchitecture GetArchitecture()
		{
			return DepthLabelApp.Interface;
		}
	}
}
