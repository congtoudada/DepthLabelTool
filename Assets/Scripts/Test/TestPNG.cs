/****************************************************
  文件：TestPNG.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GJFramework
{
    public class TestPNG : MonoBehaviour
    {
        public RenderTexture rt;

        private Texture2D texture2D;
        
        // Start is called before the first frame update
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            ConvertToPNG();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void ConvertToPNG()
        {
            RenderTexture.active = rt;
            texture2D = new Texture2D(rt.width, rt.height, TextureFormat.R16, false);
            texture2D.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            var bytes = texture2D.EncodeToPNG();
            var path = Application.dataPath + "/Outputs/test.png";
            File.WriteAllBytes(path, bytes);
        }
    }
}


