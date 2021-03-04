using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class FileBaseWindows : FileBase
{

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    public override IEnumerator WriteText(string dataName, string data)
    {
        string path = GetDataLocation() + dataName;
        File.WriteAllText(path, data);
        yield break;
    }


    public override IEnumerator ReadText(string dataName)
    {
        ReadText_Result = string.Empty;
        string path = GetDataLocation() + dataName;
        ReadText_Result = File.ReadAllText(path);
        yield break;
    }

    public override string GetDataLocation()
    {
        return Application.persistentDataPath + "/";
        //윈도우의 경우에는 파일 경로 뒤에 / 붙여줘야 ㄹㅇ 경로처럼 되어버리기 때문에...ㅇㅋ? 
    }


    public override IEnumerator IsExist(string dataName)
    {
        string path = GetDataLocation() + dataName;
        IsExist_Result = File.Exists(path);
        yield break;
    }
#endif
}
