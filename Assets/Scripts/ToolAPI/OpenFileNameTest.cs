using UnityEngine;

public class OpenFileNameTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LocalDialog.GetLocalFileName("存储位置:", "所有格式(*;)\0*;", (string s) =>
            {
                Debug.Log(s);
            });
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            LocalDialog.GetStorageFolderName("存储位置:", (string s) =>
            {
                Debug.Log(s);
            });
        }
    }
}