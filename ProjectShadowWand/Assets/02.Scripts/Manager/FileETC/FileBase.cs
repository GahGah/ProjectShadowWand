using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileBase
{
    public virtual IEnumerator WriteText(string dataName, string data)
    {

        yield break;
    }

    public string ReadText_Result;
    public virtual IEnumerator ReadText(string dataName)
    {
        yield break;
    }
    


    public virtual IEnumerator WriteBytes(string dataName, byte[] data)
    {
        yield break;
    }
    public virtual string GetDataLocation()
    {
        return string.Empty;
    }

    public bool IsExist_Result;
    public virtual IEnumerator IsExist(string dataName)
    {
        yield break;
    }
       
}
