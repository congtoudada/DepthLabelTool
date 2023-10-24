using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace GJFramework
{
	// Generate Id:1c5135b0-c600-4eee-af5c-050b80f3a0e1
	public partial class DepthLabelPanel
	{
		public const string Name = "DepthLabelPanel";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI DirtyTip;
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
		public UnityEngine.UI.ScrollRect LabelScrollView;
		[SerializeField]
		public UnityEngine.UI.ScrollRect InfoScrollView;
		[SerializeField]
		public UnityEngine.UI.Button BackBtn;
		[SerializeField]
		public UnityEngine.UI.Button NextBtn;
		[SerializeField]
		public UnityEngine.UI.Button ExitButton;
		[SerializeField]
		public UnityEngine.UI.Image GrayBG;
		[SerializeField]
		public UnityEngine.UI.Image MainImg;
		[SerializeField]
		public RectTransform Results;
		
		private DepthLabelPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			DirtyTip = null;
			LockBtn = null;
			LockText = null;
			AnnoBtn = null;
			AnnoText = null;
			DirBtn = null;
			DirText = null;
			SaveDirBtn = null;
			SaveDirText = null;
			TypeDropdown = null;
			LabelScrollView = null;
			InfoScrollView = null;
			BackBtn = null;
			NextBtn = null;
			ExitButton = null;
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
