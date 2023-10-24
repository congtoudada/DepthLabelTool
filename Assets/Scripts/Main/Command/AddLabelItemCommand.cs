using QFramework;
using UnityEngine;

namespace GJFramework
{
    public class AddLabelItemCommand : AbstractCommand
    {
        private DepthLabelPanel _controller;
        private string typeInfo;
        private Vector2 lt;
        private Vector2 rb;
        private Color labelColor;
        private int typeIdx;
        public AddLabelItemCommand(DepthLabelPanel controller, int typeIdx, string typeInfo, Vector2 lt, Vector2 rb, Color labelColor)
        {
            this._controller = controller;
            this.typeInfo = typeInfo;
            this.lt = lt;
            this.rb = rb;
            this.labelColor = labelColor;
            this.typeIdx = typeIdx;
        }
        
        protected override void OnExecute()
        {
            var obj = GameObject.Instantiate(_controller.ResLoader.LoadSync<GameObject>("LabelItem"),
                _controller.LabelContent, true);
            var showItem = GameObject.Instantiate(_controller.ResLoader.LoadSync<GameObject>("ShowResultItem"),
                _controller.Results, true);
            obj.GetOrAddComponent<LabelResult>().init(this.typeIdx,this.typeInfo, lt, rb, showItem.GetComponent<ShowResultItem>(), labelColor);

            _controller.GrayBG.enabled = false;
            _controller.SetTip(_controller.DefaultTip);
            _controller.DragFeatureComp.Switch(false);
        }
    }
}