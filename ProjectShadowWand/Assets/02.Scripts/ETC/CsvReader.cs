using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class CsvReader : MonoBehaviour
{

    //���� �̸� ������ �� Ȯ���� ���� ����.
    //���ҽ�.�ε带 ����ϱ� ������, ��θ� ���Ÿ� "/DataFiles/(����ΰž���?)"+file �̷� ������ ����Ѵ�. ok?

    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";

    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static string UnquoteString(string str)
    {
        if (String.IsNullOrEmpty(str))
        {
            return str;
        }

        int length = str.Length;
        if (length > 1 && str[0] == '\"' && str[length - 1] == '\"')
        {
            str = str.Substring(1, length - 2);
        }

        return str;
    }

    /// <summary>
    /// Resources.Load�� �̿��Ͽ� ������ �ҷ��ɴϴ�.
    /// </summary>
    /// <param name="file">��δ� "�����̸�/�����̸�"</param>
    /// <returns></returns>
    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load<TextAsset>(file) as TextAsset;

        if (data == null)
        {
            Debug.Log("������ �����ϴ� : " + file);
            return null;
        }
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j].Replace("\\n", "\n");
                //string value = values[j].Replace("\"\"", "\"");
                value = UnquoteString(value);
                //value = value.Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
