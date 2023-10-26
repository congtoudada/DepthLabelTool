/****************************************************************************
 * 2023.10 LAPTOP-CONG
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace GJFramework
{
	public partial class InputPanel
	{
		[SerializeField] public TMPro.TextMeshProUGUI Title;
		[SerializeField] public TMPro.TMP_InputField cls_id;
		[SerializeField] public TMPro.TMP_InputField length;
		[SerializeField] public TMPro.TMP_InputField height;
		[SerializeField] public TMPro.TMP_InputField width;
		[SerializeField] public TMPro.TMP_InputField x_center;
		[SerializeField] public TMPro.TMP_InputField y_bottom;
		[SerializeField] public TMPro.TMP_InputField ry_ind;
		[SerializeField] public TMPro.TMP_InputField z_center;
		[SerializeField] public UnityEngine.UI.Button ClearBtn;
		[SerializeField] public UnityEngine.UI.Button VisBtn;
		[SerializeField] public UnityEngine.UI.Button SaveBtn;
		[SerializeField] public UnityEngine.UI.Button VerifyBtn;
		[SerializeField] public UnityEngine.UI.Button TemplateBtn;

		public void Clear()
		{
			Title = null;
			cls_id = null;
			length = null;
			height = null;
			width = null;
			x_center = null;
			y_bottom = null;
			ry_ind = null;
			z_center = null;
			ClearBtn = null;
			VisBtn = null;
			SaveBtn = null;
			VerifyBtn = null;
			TemplateBtn = null;
		}

		public override string ComponentName
		{
			get { return "InputPanel";}
		}
	}
}
