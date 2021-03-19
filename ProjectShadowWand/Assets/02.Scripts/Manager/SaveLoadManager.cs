using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif



public enum eDataType
{
    PLAYER, CHILD
    //etc...
}
/// <summary>
/// 데이터를 세이브하고 로드할 때 쓰이는 매니저...
/// </summary>
public class SaveLoadManager : MonoBehaviour
{
    public int currentDataSlot;
    public string currentFilePath;
    public FileManager fileManager;

    public Data_Player currentData_Player;
    public Data_Child currentData_Child;
    public Data_ChildList currentData_ChildList;

    public static SaveLoadManager Instance;


    [HideInInspector]
    public string playerFileName = "Data_Player.dat";

    [HideInInspector]
    public string childFileName = "Data_Childs.dat";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("이미 instance가 존재합니다." + this);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("DataPath : " + Application.dataPath);
        string path = Application.dataPath;
        string[] splitPath = path.Split(new string[] { "Assets" }, System.StringSplitOptions.RemoveEmptyEntries);
        path = splitPath[0];
        Debug.Log("Assets를 없앤 데이터 패스 : " + path);
    }

    /// <summary>
    /// 데이터를 저장할 경로가 있는지 확인하고, 없으면 경로에 맞는 폴더를 생성합니다.
    /// </summary>
    /// <param name="_t">데이터의 타입입니다 플레이어, 차일드 등이 있습니다.</param>
    /// <param name="_dataSlot">데이터 저장 슬롯의 번호입니다.이 번호에 따라 Data_n의 폴더가 만들어집니다.</param>
    /// <returns></returns>
    public IEnumerator CreatePath(eDataType _t, int _dataSlot)
    {
        string path = Application.dataPath + "/DataSlot_" + _dataSlot + "/";
        currentFilePath = path;
        string tempName = string.Empty;
        //dataPath/DataSlot_1/

        switch (_t)
        {
            case eDataType.PLAYER:
                tempName = playerFileName;
                break;

            case eDataType.CHILD:
                tempName = childFileName;
                break;

            default:
                break;
        }

        yield return StartCoroutine(fileManager.IsExist(tempName, path));

        if (fileManager.isExist_Result == false) //파일이 없으면
        {
            DirectoryInfo di = new DirectoryInfo(path);
            di.Create();

            //에디터일 경우 Refresh를 시켜줌.
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }

    #region Data_Player
    /// <summary>
    /// currentData_Player를 세팅합니다.
    /// </summary>
    public void SetCurrentData_Player(Data_Player _d)
    {

        //깊은 복사가 되었으면 좋겠다.
        Data_Player tempData = new Data_Player();

        tempData.currentStage = _d.currentStage;
        tempData.currentPosition = _d.currentPosition;
        tempData.currentHP = _d.currentHP;

        currentData_Player = tempData;
    }

    /// <summary>
    /// currentData_Player를 저장합니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator SaveData_Player()
    {
        yield return StartCoroutine(CreatePath(eDataType.PLAYER, currentDataSlot));
        string path = currentFilePath;
        string data = JsonUtility.ToJson(currentData_Player, true);
        yield return StartCoroutine(fileManager.WriteText(playerFileName, data, path));
    }

    public IEnumerator LoadData_Player()
    {
        yield return StartCoroutine(CreatePath(eDataType.PLAYER, currentDataSlot));
        string path = currentFilePath;
        yield return StartCoroutine(fileManager.ReadText(playerFileName, path));
        if (!string.IsNullOrEmpty(fileManager.readText_Result))
        {
            var loadedData = JsonUtility.FromJson<Data_Player>(fileManager.readText_Result);
            currentData_Player = loadedData;

            //ApplySettings(loadedData);
        }
    }

    #endregion


    #region Data_Child
    public void SetCurrentData_ChildList(Data_ChildList _d)
    {

        //깊은 복사가 되었으면 좋겠다.
        Data_ChildList tempData = new Data_ChildList();


        tempData.childDataList = _d.childDataList;

        currentData_ChildList = tempData;
        //currentData_ChildList = _d.childDataList.ConvertAll<>
    }

    /// <summary>
    /// currentData_Child를 저장합니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator SaveData_ChildList()
    {
        yield return StartCoroutine(CreatePath(eDataType.CHILD, currentDataSlot));

        string path = currentFilePath;
        string data = JsonUtility.ToJson(currentData_ChildList, true);
        yield return StartCoroutine(fileManager.WriteText(childFileName, data, path));
    }

    public IEnumerator LoadData_ChildList()
    {
        yield return StartCoroutine(CreatePath(eDataType.CHILD, currentDataSlot));

        string path = currentFilePath;

        yield return StartCoroutine(fileManager.ReadText(childFileName, path));
        if (!string.IsNullOrEmpty(fileManager.readText_Result))
        {
            var loadedData = JsonUtility.FromJson<Data_ChildList>(fileManager.readText_Result);
            currentData_ChildList = loadedData;
            //ApplySettings(loadedData);
        }
    }

    #endregion


    public void StartTestSave()
    {
        StartCoroutine(TestSave());
    }
    public void StartTestLoad()
    {
        StartCoroutine(TestLoad());
    }

    //테스트용 세이브.
    public IEnumerator TestSave()
    {
        yield return StartCoroutine(CreatePath(eDataType.CHILD, currentDataSlot));

        currentData_ChildList = new Data_ChildList();
        currentData_ChildList.childDataList = new List<Data_Child>();
        currentData_Child = new Data_Child();
        currentData_Child.currentStage = 1;
        currentData_Child.currentPosition = new Vector3(0, 0, 0);
        currentData_Child.isDie = false;
        currentData_Child.isFriend = false;
        currentData_Child.isFriended = false;
        currentData_Child.name = "코라";
        currentData_Child.diaryData = "아앙, 테메와 돈독한 친구사이같다...";

        currentData_ChildList.childDataList.Add(currentData_Child);

        yield return StartCoroutine(SaveData_ChildList());
        currentData_Child = null;

        Debug.Log("Save Finish!");
        yield break;
    }

    public IEnumerator TestLoad()
    {
        yield return StartCoroutine(LoadData_ChildList());
        currentData_Child = new Data_Child();
        currentData_Child.name = currentData_ChildList.childDataList[0].name;
        Debug.Log("currentDataChild.name : " + currentData_Child.name);
        Debug.Log("currentDataChildList0.name : " + currentData_ChildList.childDataList[0].name);

        currentData_ChildList.childDataList[0].name = "복사가 제대로 되었나?";
        Debug.Log("currentDataChild.name (복사 후 ) : " + currentData_Child.name);
        Debug.Log("OK!----------------");
        Data_ChildList tempList = new Data_ChildList();

        //tempList.childDataList = Linq
        tempList.childDataList = currentData_ChildList.childDataList;
        Debug.Log("커런트리스트 변경 전 이름 : " + tempList.childDataList[0].name);
        currentData_ChildList.childDataList[0].name = "test!";
        Debug.Log("커런트리스트 변경 후 이름 : " + tempList.childDataList[0].name);

        yield break;
    }
    //public IEnumerator SaveInGameData()
    //{
    //    string dataString = JsonUtility.ToJson(currentInGameData, true);
    //    yield return StartCoroutine(fileManager.WriteText(""));

    //    yield break;
    //}


    //public IEnumerator SaveSettingsData()
    //{
    //    string dataString = JsonUtility.ToJson(currentSettingsData, true); //true로 하면 제대로...그...띄어쓰기? 가 됨.

    //    yield return StartCoroutine(GameManager.Instance.fileManager.WriteText("Settings.dat", dataString));

    //    yield break;
    //}

    //public IEnumerator LoadSettingsData()
    //{
    //    yield return StartCoroutine(GameManager.Instance.fileManager.ReadText("Settings.dat"));
    //    if (!string.IsNullOrEmpty(GameManager.Instance.fileManager.readText_Result))
    //    {
    //        var loadedSettingsData = JsonUtility.FromJson<SettingsData>(GameManager.Instance.fileManager.readText_Result);
    //        ApplySettings(loadedSettingsData);
    //    }
    //}

}
