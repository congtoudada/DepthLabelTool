using System;
using System.Collections.Generic;

namespace GJFramework
{
    [Serializable]
    public class LabelResultJson
    {
        public string rootDir;
        public string bmpFilename;
        public int cls_id;
        public string length;
        public string height;
        public string width;
        public string x_center;
        public string y_bottom;
        public string ry_ind;
        public string z_center;
        
        public int left;
        public int top;
        public int right;
        public int bottom;
        // public int img_width = 1280;
        // public int img_height = 960;
        // public List<LabelResultJsonItem> resultList = new List<LabelResultJsonItem>();

        // public void Init(string bmpFilename, int cls_id, string length, string height, string width, string x_center,
        //     string y_bottom, string ry_ind, string z_center,
        //     int left, int top, int right, int bottom)
        // {
        //     this.bmpFilename = bmpFilename;
        //     
        //     this.cls_id = cls_id;
        //     this.length = length;
        //     this.height = height;
        //     this.width = width;
        //     this.x_center = x_center;
        //     this.y_bottom = y_bottom;
        //     this.ry_ind = ry_ind;
        //     this.z_center = z_center;
        //     
        //     this.left = left;
        //     this.top = top;
        //     this.right = right;
        //     this.bottom = bottom;
        // }

        // public override bool Equals(object obj)
        // {
        //     if (obj == null) return false;
        //     LabelResultJson compareObj = obj as LabelResultJson;
        //     if (compareObj == null) return false;
        //     if (this.bmpFilename != compareObj.bmpFilename) return false;
        //     if (this.resultList.Count != compareObj.resultList.Count) return false;
        //     for (int i = 0; i < this.resultList.Count; i++)
        //     {
        //         if (!this.resultList[i].Equals(compareObj.resultList[i]))
        //         {
        //             return false;
        //         }
        //     }
        //     return true;
        // }
    }

    [Serializable]
    public class LabelResultJsonItem
    {
        public void Init(int cls_id, string length, string height, string width, string x_center,
            string y_bottom, string ry_ind, string z_center,
            int left, int top, int right, int bottom)
        {

            this.cls_id = cls_id;
            this.length = length;
            this.height = height;
            this.width = width;
            this.x_center = x_center;
            this.y_bottom = y_bottom;
            this.ry_ind = ry_ind;
            this.z_center = z_center;
            
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
        public int cls_id;
        public string length;
        public string height;
        public string width;
        public string x_center;
        public string y_bottom;
        public string ry_ind;
        public string z_center;
        
        public int left;
        public int top;
        public int right;
        public int bottom;

        // public override bool Equals(object obj)
        // {
        //     if (obj == null) return false;
        //     LabelResultJsonItem compareObj = obj as LabelResultJsonItem;
        //     if (compareObj == null) return false;
        //     return this.cls_id == compareObj.cls_id && this.lt_x == compareObj.lt_x && this.lt_y == compareObj.lt_y &&
        //            this.rb_x == compareObj.rb_x && this.rb_y == compareObj.rb_y;
        // }
    }
}