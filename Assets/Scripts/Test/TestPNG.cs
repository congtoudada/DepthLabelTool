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
using UnityEngine.Networking;
using UnityEngine.UI;

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
            // ConvertToPNG();
            LoadPNG(@"E:\Practice\Unity\资料\DUT\label_toolkit_demo\1020\parking_plot\20231020160928\vis\00001_left.png");
        }

        void LoadPNG(string path)
        {
            StartCoroutine(LoadTexture2D(path));
        }
        
        public IEnumerator LoadTexture2D(string path)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(path);
            yield return request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError)
            {  }
            else
            {
                // 这个是物体身上的组件Image
                Image img = GetComponent<Image>();

                var texture = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                img.sprite = sprite;
            }
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


