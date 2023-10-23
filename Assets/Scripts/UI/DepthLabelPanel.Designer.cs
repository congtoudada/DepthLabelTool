using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace GJFramework
{
	// Generate Id:b4e5eb90-3366-4fe1-8221-d090664dcb07
	public partial class DepthLabelPanel
	{
		public const string Name = "DepthLabelPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image MainImg;
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
		
		private DepthLabelPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			MainImg = null;
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
