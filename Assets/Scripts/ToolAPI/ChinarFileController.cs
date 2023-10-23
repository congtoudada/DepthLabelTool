using UnityEngine;
using System.Runtime.InteropServices;
using System;


/// <summary>
/// 文件控制脚本
/// </summary>
public class ChinarFileController : MonoBehaviour
{
    /// <summary>
    /// 打开项目
    /// </summary>
    public void OpenProject()
    {
        OpenFileDlg pth  = new OpenFileDlg();
        pth.structSize   = Marshal.SizeOf(pth);
        pth.filter       = "All files (*.*)|*.*";
        pth.file         = new string(new char[256]);
        pth.maxFile      = pth.file.Length;
        pth.fileTitle    = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir   = Application.dataPath.Replace("/", "\\") + "\\Resources"; //默认路径
        pth.title        = "打开项目";
        pth.defExt       = "dat";
        pth.flags        = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (OpenFileDialog.GetOpenFileName(pth))
        {
            string filepath = pth.file; //选择的文件路径;  
            Debug.Log(filepath);
        }
    }


    /// <summary>
    /// 保存文件项目
    /// </summary>
    public void SaveProject()
    {
        SaveFileDlg pth  = new SaveFileDlg();
        pth.structSize   = Marshal.SizeOf(pth);
        pth.filter       = "All files (*.*)|*.*";
        pth.file         = new string(new char[256]);
        pth.maxFile      = pth.file.Length;
        pth.fileTitle    = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir   = Application.dataPath; //默认路径
        pth.title        = "保存项目";
        pth.defExt       = "dat";
        pth.flags        = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (SaveFileDialog.GetSaveFileName(pth))
        {
            string filepath = pth.file; //选择的文件路径;  
            Debug.Log(filepath);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            OpenProject();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SaveProject();
        }
    }
}