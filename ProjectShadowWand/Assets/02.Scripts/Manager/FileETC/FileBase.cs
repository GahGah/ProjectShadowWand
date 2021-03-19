using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// File클래스의 부모입니다.
/// </summary>
public class FileBase
{
    public virtual IEnumerator WriteText(string _dataName, string _data, string _path)
    {

        yield break;
    }

    public string readText_Result;
    public virtual IEnumerator ReadText(string _dataName, string _path)
    {
        yield break;
    }



    public virtual IEnumerator WriteBytes(string dataName, byte[] data)
    {
        yield break;
    }
    //public virtual string GetDataLocation()
    //{
    //    return string.Empty;
    //}
    /// <summary>
    /// Application.PersistentDataPath를 사용해서 경로를 가져옵니다.
    /// </summary>
    public virtual string GetDataLocation_Persistent()
    {
        return string.Empty;
    }
    /// <summary>
    /// Application.dataPath를 사용해서 경로를 가져옵니다.
    /// </summary>
    /// <returns></returns>
    public virtual string GetDataLocation_DataPath()
    {
        return string.Empty;
    }


    public bool isExist_Result;
    public virtual IEnumerator IsExist(string dataName, string dataPath)
    {
        yield break;
    }

}
