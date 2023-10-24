using System.Collections;
using System.Diagnostics;
using GJFramework;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CallPython : MonoBehaviour
{
    public TMP_Text TestText;
    private string output;

    public static void ExportDepthPNG(LabelResultJson labelResultJson)
    {
        
    }
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3.0f);
        
        // 创建一个进程
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = "python";
        string args = @"E:\Temporary\项目\深度估计实验数据\标注结果\00001_left_ccd.json";
        start.Arguments = Application.streamingAssetsPath + $"/Python/depth_tool.py --json_path {args}";
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;

        // 启动进程
        Process process = new Process();
        process.StartInfo = start;
        process.Start();

        // 从Python脚本中获取输出结果
        output = process.StandardOutput.ReadToEnd();

        // 输出结果
        Debug.Log(output);
        TestText.text = "result: " + output;
    }
}