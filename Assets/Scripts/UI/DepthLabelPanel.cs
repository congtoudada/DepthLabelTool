using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using B83.Image.BMP;
using DG.Tweening;
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
		SaveDir,
		Start,
		Clear
	}

	public partial class DepthLabelPanel : UIPanel, IController
	{
		public DragFeature DragFeatureComp;
		// Model
		private IDepthLabelModel mModel;
		// Other
		public int CurrentTypeIdx => currentTypeIdx;
		private int currentTypeIdx = 0;
		private int currentBMPIdx = 0;
		private List<string> _typeInfoList = new List<string>();
		private List<string> _typeFloatInfoList = new List<string>();
		private const string pattern = @"\d+(\.\d+)?";  
		public string DefaultTip => _defaultTip;
		private string _defaultTip = "";
		private IActionController _tipController;
		public ResLoader ResLoader => mResLoader;
		private ResLoader mResLoader = ResLoader.Allocate();
		private Transform _infoContent;
		public Transform LabelContent => _labelContent;
		private Transform _labelContent;
		private Color infoColor = new Color(234 / 55.0f, 138 / 255.0f, 138 / 255.0f, 255 / 255.0f);
		private Dictionary<int, Color> _labelColorDict = new Dictionary<int, Color>();
		private LabelResultJson _labelResultJson = new LabelResultJson();
		private SimpleObjectPool<LabelResultJsonItem> _jsonPool;
		private bool isLock = true; //初始化会设为false

		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as DepthLabelPanelData ?? new DepthLabelPanelData();
			// please add init code here
			ResKit.Init();
			Random.InitState(201500401);
			_jsonPool = new SimpleObjectPool<LabelResultJsonItem>(() => new LabelResultJsonItem(), null, 5);
			mModel = this.GetModel<IDepthLabelModel>();
			_defaultTip = DirtyTip.text;
			_infoContent = InfoScrollView.content;
			_labelContent = LabelScrollView.content;
			DragFeatureComp = UIKit.Root.PopUI.GetComponent<DragFeature>();
			
			#region 事件监听
			LockBtn.onClick.AddListener(SwitchLock);
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
					SwitchInfoItem(currentBMPIdx, currentBMPIdx-1);
				}
			});
			
			//下一张
			NextBtn.onClick.AddListener(() =>
			{
				if (currentBMPIdx+1 < _infoContent.childCount)
				{
					SwitchInfoItem(currentBMPIdx, currentBMPIdx+1);
				}
			});
			
			
			//标注开关
			DragFeatureComp.OnDragOverEvent += (lt, rb) =>
			{
				string typeInfo = TypeDropdown.options[currentTypeIdx].text; //当前类型信息
				Color labelColor = Color.black;
				_labelColorDict.TryGetValue(currentTypeIdx, out labelColor);
				this.SendCommand(new AddLabelItemCommand(this, currentTypeIdx, typeInfo, lt, rb, labelColor));
			};
			ActionKit.OnUpdate.Register(() =>
			{
				if (isLock && Input.GetKeyDown(KeyCode.W))
				{
					this.SendCommand(new LabelActionCommand(this));
				}
			});
			
			//退出
			ExitButton.onClick.AddListener(Application.Quit);
			#endregion
			
			UpdateView(DepthLabelPanelViewEnum.Start);
		}
		

		private void UpdateView(DepthLabelPanelViewEnum viewEnum)
		{
			switch (viewEnum)
			{
				case DepthLabelPanelViewEnum.All:
					UpdateView(DepthLabelPanelViewEnum.SaveDir);
					UpdateView(DepthLabelPanelViewEnum.OpenAnnotation);
					//更新TypeDropDown
					UpdateView_TypeDropDown();
					UpdateView(DepthLabelPanelViewEnum.OpenDir);
					//更新InfoScrollView
					StartCoroutine(UpdateView_InfoScrollView());
					break;
				case DepthLabelPanelViewEnum.OpenDir:
					DirText.text = mModel.DataDir.Value;
					break;
				case DepthLabelPanelViewEnum.OpenAnnotation:
					AnnoText.text = mModel.AnnoPath.Value;
					break;
				case DepthLabelPanelViewEnum.SaveDir:
					SaveDirText.text = mModel.SaveDir.Value;
					break;
				case DepthLabelPanelViewEnum.Start:
					UpdateView(DepthLabelPanelViewEnum.SaveDir);
					UpdateView(DepthLabelPanelViewEnum.OpenAnnotation);
					UpdateView(DepthLabelPanelViewEnum.OpenDir);
					SwitchLock();
					// 1920x1080就全屏显示
					if (Screen.width != 1920 || Screen.height != 1080)
					{
						Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
					}
					break;
				case DepthLabelPanelViewEnum.Clear:
					ClearView();
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
				string[] files = Directory.GetFiles(dataDir, "*left_ccd*.bmp", SearchOption.TopDirectoryOnly);
				int fileIdx = 0;
				foreach (string file in files)
				{
					// 处理每个文件
					var obj = Instantiate(mResLoader.LoadSync<GameObject>("InfoItemBtn"), _infoContent, true);
					obj.name = fileIdx + "@" + file.GetFileName();
					obj.GetComponentInChildren<TMP_Text>().text = file.GetFileName();
					obj.GetComponent<Button>().onClick.AddListener(() =>
					{
						SwitchInfoItem(_infoContent.GetChild(currentBMPIdx).name, obj.name);
					});
					fileIdx++;
				}
				
				// 文件数大于0
				if (_infoContent.childCount > 0)
				{
					currentBMPIdx = 0;
					SwitchInfoItem(currentBMPIdx, currentBMPIdx);
				}
			}
		}
		
		//更换Info
		private void SwitchInfoItem(string old_name, string name)
		{
			//还原旧数据
			_infoContent.GetChild(currentBMPIdx).GetComponent<Image>().color = Color.white;
						
			//设置新数据
			string[] data = name.Split('@');
			//0是序号 1是文件名
			// Debug.Log("data: " + data[0] + "@" + data[1]);
			currentBMPIdx = data[0].ToInt();
			_infoContent.GetChild(currentBMPIdx).GetComponent<Image>().color = infoColor;
			//显示BMP
			string path = Path.Combine(mModel.DataDir.Value, data[1]);
			Texture2D texture2D = BMPLoader.LoadTexture(path);
			MainImg.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);
			
			//更新LabelResult
			StartCoroutine(UpdateLabelResult(old_name.Split('@')[1], data[1]));
		}

		private void SwitchInfoItem(int oldIdx, int idx)
		{
			// Debug.Log(_infoContent.GetChild(idx).name);
			SwitchInfoItem(_infoContent.GetChild(oldIdx).name ,_infoContent.GetChild(idx).name);
		}
		
		// 更新结果列表
		//  old_filename: 切换前图片文件名
		//	filename: 图片文件名
		private IEnumerator UpdateLabelResult(string old_filename, string filename)
		{
			if (_labelContent.childCount > 0)
			{
				//存储
				string save_jsonName = old_filename.GetFileNameWithoutExtend() + ".json";
				string save_jsonPath = Path.Combine(mModel.SaveDir.Value, save_jsonName);

				_labelResultJson.Init(old_filename, mModel.AnnoPath.Value.GetFileName(), this._typeFloatInfoList);
				LabelResult[] labelResults = _labelContent.GetComponentsInChildren<LabelResult>();
				foreach (LabelResult result in labelResults)
				{
					LabelResultJsonItem jsonItem = _jsonPool.Allocate();
					jsonItem.Init(result.typeIndex, (int) result.left_up[0], (int) result.left_up[1], 
						(int) result.right_bottom[0], (int) result.right_bottom[1]);
					_labelResultJson.resultList.Add(jsonItem);
				}
				
				//根据是否变化判断写入本地
				LabelResultJson localJsonObj = null;
				if (File.Exists(save_jsonPath))
				{
					localJsonObj = JsonUtility.FromJson<LabelResultJson>(File.ReadAllText(save_jsonPath));
				}

				if (localJsonObj == null || !localJsonObj.Equals(_labelResultJson))
				{
					string jsonString = JsonUtility.ToJson(_labelResultJson, true);
					using (StreamWriter sw = new StreamWriter(save_jsonPath))  
					{  
						sw.Write(jsonString);
					}
					//导出png
					this.SendCommand(new ExportDepthPNGCommand(this, _labelResultJson));
				}

				//回收
				foreach (var jsonItem in _labelResultJson.resultList)
				{
					_jsonPool.Recycle(jsonItem);
				}
				_labelResultJson.resultList.Clear();

				//删除旧节点
				LabelResult[] children = _labelContent.GetComponentsInChildren<LabelResult>();
				foreach (LabelResult child in children)  
				{
					child.OnRelease();
					Destroy(child.gameObject);
				}
				yield return null;
			}
			//尝试载入本地Annotation
			string loadJsonName = filename.GetFileNameWithoutExtend() + ".json";
			string loadJsonPath = Path.Combine(mModel.SaveDir.Value, loadJsonName);
			if (File.Exists(loadJsonPath))
			{
				_labelResultJson = JsonUtility.FromJson<LabelResultJson>(File.ReadAllText(loadJsonPath));
				foreach (var item in _labelResultJson.resultList)
				{
					Color labelColor = Color.black;
					_labelColorDict.TryGetValue(item.typeIdx, out labelColor);
					this.SendCommand(new AddLabelItemCommand(this, item.typeIdx, TypeDropdown.options[item.typeIdx].text,
						new Vector2(item.lt_x, item.lt_y), new Vector2(item.rb_x, item.rb_y),
						labelColor));
				}
			}
		}

		//更新TypeDropDown
		private void UpdateView_TypeDropDown()
		{
			// 类型选择
			TypeDropdown.options.Clear();
			_typeInfoList.Clear();
			_typeFloatInfoList.Clear();
			// TypeDropdown.options.Add(new TMP_Dropdown.OptionData("空类别"));
			if (mModel.AnnoPath.Value.IsNotNullAndEmpty())
			{
				// TypeDropdown.options.Add(new TMP_Dropdown.OptionData(""));
				using StreamReader sr = new StreamReader(mModel.AnnoPath.Value);
				string line =  sr.ReadLine();
				while (line != null)
				{
					TypeDropdown.options.Add(new TMP_Dropdown.OptionData(line));
					_typeInfoList.Add(line);
					line = sr.ReadLine();
				}
				for (int i = 0; i < _typeInfoList.Count; i++)
				{
					Match match = Regex.Match(_typeInfoList[i], pattern);  
          
					if (match.Success)  
					{  
						_typeFloatInfoList.Add(match.Value);
					}  
				}

				if (TypeDropdown.options.Count > 0)
				{
					_labelColorDict.Clear();
					TypeDropdown.value = 0;
					for (int i = 0; i < TypeDropdown.options.Count; i++)
					{
						_labelColorDict.Add(i, new Color(Random.value, Random.value, Random.value, 186.0f / 255.0f));
					}
					
					currentTypeIdx = 0;
					TypeDropdown.value = 0;
					TypeDropdown.RefreshShownValue();
				}
			}
		}

		public void SwitchLock()
		{
			if (isLock)
			{
				// 锁定 -> 解锁
				isLock = false;
				LockText.text = "锁定路径";
				DirBtn.interactable = true;
				AnnoBtn.interactable = true;
				SaveDirBtn.interactable = true;
				BackBtn.interactable = false;
				NextBtn.interactable = false;
				//清屏
				UpdateView(DepthLabelPanelViewEnum.Clear);
			}
			else
			{
				// 解锁 -> 锁定
				isLock = true;
				LockText.text = "解锁路径";
				DirBtn.interactable = false;
				AnnoBtn.interactable = false;
				SaveDirBtn.interactable = false;
				BackBtn.interactable = true;
				NextBtn.interactable = true;
				//更新屏幕信息
				UpdateView(DepthLabelPanelViewEnum.All);
			}
		}

		private void ClearView()
		{
			TypeDropdown.options.Clear();
			TypeDropdown.RefreshShownValue();
			if (_infoContent.childCount > 0)
			{
				Transform[] children = _infoContent.GetComponentsInChildren<Transform>();
				foreach (Transform child in children)  
				{  
					if (child != _infoContent)
						Destroy(child.gameObject);
				}  
			}

			if (_labelContent.childCount > 0)
			{
				LabelResult[] children = _labelContent.GetComponentsInChildren<LabelResult>();
				foreach (LabelResult child in children)  
				{
					child.OnRelease();
					Destroy(child.gameObject);
				}
			}
		}

		public void SetTip(string info, float duration)
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

		public void SetTip(string info)
		{
			DirtyTip.DOText(info, 1.0f);
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

		private new void OnDestroy()
		{
			base.OnDestroy();
			mResLoader.Recycle2Cache();
			mResLoader = null;
		}

		public IArchitecture GetArchitecture()
		{
			return DepthLabelApp.Interface;
		}
	}
}
