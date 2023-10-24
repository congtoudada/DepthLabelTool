using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace GJFramework
{
	// Generate Id:89c51fbb-5589-4213-b9c2-71a9fbda9820
	public partial class DepthLabelPanel
	{
		public const string Name = "DepthLabelPanel";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI DirtyTip;
		[SerializeField]
		public UnityEngine.UI.Button DirBtn;
		[SerializeField]
		public TMPro.TextMeshProUGUI DirText;
		[SerializeField]
		public UnityEngine.UI.Button AnnoBtn;
		[SerializeField]
		public TMPro.TextMeshProUGUI AnnoText;
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
		public RectTransform Results;
		[SerializeField]
		public UnityEngine.UI.Image GrayBG;
		[SerializeField]
		public UnityEngine.UI.Image MainImg;
		
		private DepthLabelPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			DirtyTip = null;
			DirBtn = null;
			DirText = null;
			AnnoBtn = null;
			AnnoText = null;
			SaveDirBtn = null;
			SaveDirText = null;
			TypeDropdown = null;
			LabelScrollView = null;
			InfoScrollView = null;
			BackBtn = null;
			NextBtn = null;
			Results = null;
			GrayBG = null;
			MainImg = null;
			
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
