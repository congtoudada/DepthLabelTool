using QFramework;

namespace GJFramework
{
    public class LabelActionCommand : AbstractCommand
    {
        private DepthLabelPanel _controller
            ;
        public LabelActionCommand(DepthLabelPanel controller)
        {
            _controller = controller;
        }
        
        protected override void OnExecute()
        {
            _controller.SetTip("标注中......");
            _controller.GrayBG.enabled = true;
            _controller.DragFeatureComp.Switch(true);
        }
    }
}