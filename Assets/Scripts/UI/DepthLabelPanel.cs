using GJFramework;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class DepthLabelPanelData : UIPanelData
	{
	}
	public partial class DepthLabelPanel : UIPanel, IController
	{
		// Model
		private IDepthLabelModel mModel;

		
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as DepthLabelPanelData ?? new DepthLabelPanelData();
			// please add init code here
			mModel = this.GetModel<IDepthLabelModel>();
			
			// 打开数据目录
			DirBtn.onClick.AddListener(() =>
			{
				this.SendCommand<OpenDirCommand>();
				UpdateView();
			});
			
			// 打开标注文件
			AnnoBtn.onClick.AddListener(() =>
			{
				this.SendCommand<OpenAnnotationCommand>();
				UpdateView();
			});
			
			// 选择保存目录
			SaveDirBtn.onClick.AddListener(() =>
			{
				this.SendCommand<SelectSaveDirCommand>();
				UpdateView();
			});
			
			// 类型选择
			if (!mModel.AnnoPath.Value.IsNullOrEmpty())
			{
				
			}
			
			UpdateView();
		}

		private void UpdateView()
		{
			DirText.text = mModel.DataDir.Value;
			AnnoText.text = mModel.AnnoPath.Value;
			SaveDirText.text = mModel.SaveDir.Value;
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
