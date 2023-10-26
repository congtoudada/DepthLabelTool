using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace GJFramework
{
	// Generate Id:f4332692-897c-4870-9d0d-fbae841822e4
	public partial class DepthLabelPanel
	{
		public const string Name = "DepthLabelPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button ExitButton;
		[SerializeField]
		public TMPro.TextMeshProUGUI DirtyTip;
		[SerializeField]
		public TMPro.TMP_Dropdown PageDropdown;
		[SerializeField]
		public UnityEngine.UI.Button LockBtn;
		[SerializeField]
		public TMPro.TextMeshProUGUI LockText;
		[SerializeField]
		public UnityEngine.UI.Button AnnoBtn;
		[SerializeField]
		public TMPro.TextMeshProUGUI AnnoText;
		[SerializeField]
		public UnityEngine.UI.Button DirBtn;
		[SerializeField]
		public TMPro.TextMeshProUGUI DirText;
		[SerializeField]
		public UnityEngine.UI.Button SaveDirBtn;
		[SerializeField]
		public TMPro.TextMeshProUGUI SaveDirText;
		[SerializeField]
		public TMPro.TMP_Dropdown TypeDropdown;
		[SerializeField]
		public RectTransform RightVerticalLayout;
		[SerializeField]
		public UnityEngine.UI.ScrollRect LabelScrollView;
		[SerializeField]
		public UnityEngine.UI.ScrollRect InfoScrollView;
		[SerializeField]
		public UnityEngine.UI.Button BackBtn;
		[SerializeField]
		public UnityEngine.UI.Button NextBtn;
		[SerializeField]
		public InputPanel InputPanel;
		[SerializeField]
		public UnityEngine.UI.Image GrayBG;
		[SerializeField]
		public UnityEngine.UI.Image MainImg;
		[SerializeField]
		public RectTransform Results;
		
		private DepthLabelPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ExitButton = null;
			DirtyTip = null;
			PageDropdown = null;
			LockBtn = null;
			LockText = null;
			AnnoBtn = null;
			AnnoText = null;
			DirBtn = null;
			DirText = null;
			SaveDirBtn = null;
			SaveDirText = null;
			TypeDropdown = null;
			RightVerticalLayout = null;
			LabelScrollView = null;
			InfoScrollView = null;
			BackBtn = null;
			NextBtn = null;
			InputPanel = null;
			GrayBG = null;
			MainImg = null;
			Results = null;
			
			mData = null;
		}
		
		public DepthLabelPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		DepthLabelPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new DepthLabelPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
