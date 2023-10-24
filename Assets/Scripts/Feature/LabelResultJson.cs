using System;
using System.Collections.Generic;

namespace GJFramework
{
    [Serializable]
    public class LabelResultJson
    {
        public string bmpFilename;
        public string annotationFilename;
        public int img_width = 1280;
        public int img_height = 960;
        public List<string> typeInfoList = new List<string>();
        public List<LabelResultJsonItem> resultList = new List<LabelResultJsonItem>();

        public void Init(string bmpFilename, string annotationFilename, List<string> typeInfoList)
        {
            resultList.Clear();
            this.bmpFilename = bmpFilename;
            this.annotationFilename = annotationFilename;
            this.typeInfoList = typeInfoList;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            LabelResultJson compareObj = obj as LabelResultJson;
            if (compareObj == null) return false;
            if (this.bmpFilename != compareObj.bmpFilename) return false;
            if (this.resultList.Count != compareObj.resultList.Count) return false;
            for (int i = 0; i < this.resultList.Count; i++)
            {
                if (!this.resultList[i].Equals(compareObj.resultList[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }

    [Serializable]
    public class LabelResultJsonItem
    {
        public void Init(int typeIdx, int lt_x, int lt_y, int rb_x, int rb_y)
        {

            this.typeIdx = typeIdx;
            this.lt_x = lt_x;
            this.lt_y = lt_y;
            this.rb_x = rb_x;
            this.rb_y = rb_y;
        }
        public int typeIdx;
        public int lt_x;
        public int lt_y;
        public int rb_x;
        public int rb_y;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            LabelResultJsonItem compareObj = obj as LabelResultJsonItem;
            if (compareObj == null) return false;
            return this.typeIdx == compareObj.typeIdx && this.lt_x == compareObj.lt_x && this.lt_y == compareObj.lt_y &&
                   this.rb_x == compareObj.rb_x && this.rb_y == compareObj.rb_y;
        }
    }
}