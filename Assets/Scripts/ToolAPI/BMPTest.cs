/****************************************************
  文件：DepthTool.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using B83.Image.BMP;
using QFramework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BMPTest : MonoBehaviour
{
    private ResLoader mResLoader = null;
    public Image imgae;
    void Start()
    {
        string path = "E:\\Practice\\Unity\\Demo\\DUT\\DepthLabelTool\\Assets\\Inputs\\00001_left_ccd.bmp";
        var texture2D = LoadTexture(path);
        imgae.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);
        


    }

    private void OnDestroy()
    {
	    mResLoader.Recycle2Cache();
	    mResLoader = null;
    }
    
    public static Texture2D LoadTexture(string filePath)
    {
        Texture2D tex = null;

        if (File.Exists(filePath))
        {
            BMPLoader bmpLoader = new BMPLoader();
            //bmpLoader.ForceAlphaReadWhenPossible = true; //Uncomment to read alpha too

            //Load the BMP data
            BMPImage bmpImg = bmpLoader.LoadBMP(filePath);

            //Convert the Color32 array into a Texture2D
            tex = bmpImg.ToTexture2D();
        }
        return tex;
    }
}


