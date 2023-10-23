using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:9fddb6ed-ec96-4a98-a007-6d08fadff41d
	public partial class DepthLabelPanel
	{
		public const string Name = "DepthLabelPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image MainImg;
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
		public UnityEngine.UI.Image TypeDropdown;
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
