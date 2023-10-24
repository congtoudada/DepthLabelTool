using System;
using System.Collections.Generic;

namespace GJFramework
{
    [Serializable]
    public class LabelResultJson
    {
        public List<LabelResultJsonItem> resultList = new List<LabelResultJsonItem>();
    }

    [Serializable]
    public class LabelResultJsonItem
    {
        public void Init(string bmpFilename, string annotationFilename, int typeIdx, int lt_x, int lt_y,
            int rb_x, int rb_y)
        {
            this.bmpFilename = bmpFilename;
            this.annotationFilename = annotationFilename;
            this.typeIdx = typeIdx;
            this.lt_x = lt_x;
            this.lt_y = lt_y;
            this.rb_x = rb_x;
            this.rb_y = rb_y;
        }
        
        public string bmpFilename;
        public string annotationFilename;
        public int typeIdx;
        public int lt_x;
        public int lt_y;
        public int rb_x;
        public int rb_y;
    }
}