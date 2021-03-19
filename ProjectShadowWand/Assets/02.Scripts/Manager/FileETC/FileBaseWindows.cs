using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/// <summary>
/// Windows OS에서의 FileBase입니다.
/// </summary>
public class FileBaseWindows : FileBase
{

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    public override IEnumerator WriteText(string _dataName, string _data, string _path)
    {

        //string path = GetDataLocation_Persistent() + dataName;
        string path = _path + _dataName;

        File.WriteAllText(path, _data);
        yield break;
    }
    /// <summary>
    /// 텍스트 관련 파일을 읽어옵니다. 결과는 readText_Result에 저장됩니다.
    /// </summary>
    /// <param name="_dataName">파일의 이름. 예시 : testFile.png</param>
    /// <returns></returns>
    public override IEnumerator ReadText(string _dataName, string _path)
    {
        readText_Result = string.Empty;
        //string path = GetDataLocation_Persistent() + dataName;
        string path = _path + _dataName;
        readText_Result = File.ReadAllText(path);
        yield break;
    }


    /// <summary>
    /// Application.persistentDataPath : 사용자디렉토리/AppData/LocalLow/회사이름/프로덕트이름/
    /// </summary>
    /// <returns></returns>
    public override string GetDataLocation_Persistent()
    {
        return Application.persistentDataPath + "/";
        //윈도우의 경우에는 파일 경로 뒤에 / 붙여줘야 ㄹㅇ 경로처럼 되어버리기 때문에...ㅇㅋ? 
    }

    /// <summary>
    /// Application.dataPath : 실행파일/실행파일_Data/
    /// </summary>
    /// <returns></returns>
    public override string GetDataLocation_DataPath()
    {
        return Application.dataPath + "/";
        //윈도우의 경우에는 파일 경로 뒤에 / 붙여줘야 ㄹㅇ 경로처럼 되어버리기 때문에...ㅇㅋ? 
    }


    /// <summary>
    /// 파일이 존재하는지 확인힙니다. 결과는 isExit_Result에 저장됩니다.
    /// </summary>
    /// <param name="dataName">파일의 이름. 예시 : testFile.png</param>
    /// <returns></returns>
    public override IEnumerator IsExist(string dataName, string dataPath)
    {
        string path = dataPath + dataName;
        isExist_Result = File.Exists(path);
        yield break;
    }
#endif
}
