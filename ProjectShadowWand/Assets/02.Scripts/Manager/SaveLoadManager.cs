
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public enum eDataType
{
    PLAYER, CHILD, SETTINGS, STAGE,
    //etc...
}
/// <summary>
/// 데이터를 세이브하고 로드할 때 쓰이는 매니저...
/// </summary>
public class SaveLoadManager : Manager<SaveLoadManager>
{
    public int currentDataSlot;
    public string currentFilePath;
    public FileManager fileManager;

    public Data_Player currentData_Player;
    public Data_Child currentData_Child;

    public Data_Settings currentData_Settings;
    public Data_Stage currentData_Stage;

    [SerializeField] public Data_ChildList currentData_ChildList;

    public Dictionary<eChildType, Data_Child> currentData_ChildDict;

    [HideInInspector] public bool isLoad = false;

    [HideInInspector]
    public string stageFileName = "Data_Stage.dat";

    [HideInInspector]
    public string playerFileName = "Data_Player.dat";

    [HideInInspector]
    public string childFileName = "Data_Childs.dat";

    [HideInInspector]
    public string settingsFileName = "Data_Settings.dat";


    protected override void Awake()
    {
        base.Awake();
    }

    private IEnumerator Start()
    {
        //Debug.Log("DataPath : " + Application.dataPath);
        //string path = Application.dataPath;
        //string[] splitPath = path.Split(new string[] { "Assets" }, System.StringSplitOptions.RemoveEmptyEntries);
        //path = splitPath[0];
        //Debug.Log("Assets를 없앤 데이터 패스 : " + path);

        yield return StartCoroutine(LoadData_Settings());


        yield return StartCoroutine(LoadData_Stage());


    }

    /// <summary>
    /// 파일 이름을 정합니다.
    /// </summary>
    public void SetFileName()
    {

        stageFileName = "Data_Stage.dat";

        playerFileName = "Data_Player.dat";

        childFileName = "Data_Childs.dat";

        settingsFileName = "Data_Settings.dat";
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
        SetFileName();
        //dataPath/DataSlot_1/

        switch (_t)
        {
            case eDataType.PLAYER:
                tempName = playerFileName;
                break;

            case eDataType.CHILD:
                tempName = childFileName;
                break;
            case eDataType.SETTINGS:
                tempName = settingsFileName;
                break;

            case eDataType.STAGE:
                tempName = stageFileName;
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
        //tempData.currentHP = _d.currentHP;

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

    #region Data_Stage
    public void SetCurrentData_Stage(Data_Stage _d)
    {
        currentData_Stage = new Data_Stage(_d);
    }

    public IEnumerator SaveData_Stage()
    {
        yield return StartCoroutine(CreatePath(eDataType.STAGE, currentDataSlot));

        string path = currentFilePath; //파일 경로를 가지고...
        string data = JsonUtility.ToJson(currentData_Stage, true); // Json 형식으로 변경

        yield return StartCoroutine(fileManager.WriteText(stageFileName, data, path)); // 글 쓰기
        Debug.Log("스테이지 데이터 저장 완료! 이어하기 시 시작할 스테이지 : " + currentData_Stage.stageName);
    }

    public IEnumerator LoadData_Stage()
    {
        yield return StartCoroutine(CreatePath(eDataType.STAGE, currentDataSlot));

        string path = currentFilePath;

        yield return StartCoroutine(fileManager.ReadText(stageFileName, path));

        if (!string.IsNullOrEmpty(fileManager.readText_Result))
        {
            var loadedData = JsonUtility.FromJson<Data_Stage>(fileManager.readText_Result);
            currentData_Stage = loadedData;
        }
        else // 없을 경우
        {
            Debug.Log("이어할 데이터가 없습니다. 새 데이터를 만듭니다.");
            var defaultData = new Data_Stage();
            currentData_Stage = defaultData;
            yield return StartCoroutine(SaveData_Stage());
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

    }

    #endregion

    #region Data_Settings

    public void SetCurrentData_Settings(Data_Settings _d)
    {
        currentData_Settings = new Data_Settings(_d);
    }
    public IEnumerator SaveData_Settings()
    {
        yield return StartCoroutine(CreatePath(eDataType.SETTINGS, currentDataSlot));

        string path = currentFilePath; //파일 경로를 가지고...
        string data = JsonUtility.ToJson(currentData_Settings, true); // Json 형식으로 변경

        yield return StartCoroutine(fileManager.WriteText(settingsFileName, data, path)); // 글 쓰기
        Debug.Log("세팅 데이터 저장 완료");
    }

    public IEnumerator LoadData_Settings()
    {
        yield return StartCoroutine(CreatePath(eDataType.SETTINGS, currentDataSlot));

        string path = currentFilePath;

        yield return StartCoroutine(fileManager.ReadText(settingsFileName, path));

        if (!string.IsNullOrEmpty(fileManager.readText_Result))
        {
            var loadedData = JsonUtility.FromJson<Data_Settings>(fileManager.readText_Result);
            currentData_Settings = loadedData;
        }
        else // 없을 경우
        {
            Debug.Log("세팅 데이터 파일이 없습니다. 기본 데이터 파일을 생성합니다.");
            var defaultData = new Data_Settings();
            currentData_Settings = defaultData;
            yield return StartCoroutine(SaveData_Settings());

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

    }
    #endregion

    #region Data_Child
    public void SetCurrentData_ChildList(List<Data_Child> _d)
    {

        //깊은 복사가 되었으면 좋겠다.
        //List<Data_Child> tempData = new List<Data_Child>();


        //tempData = _d;

        //currentData_ChildList = tempData;
        ////currentData_ChildDictionary = _d.childDataDictionary.ConvertAll<>
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


    #region Test
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
        isLoad = false;

        //원래라면 딕셔너리로 작업된 녀석들을 DictToList로 변환시켜야합니다.
        currentData_ChildList = new Data_ChildList();
        currentData_ChildList.childDataList = new List<Data_Child>();
        currentData_ChildList.childDataList.Clear();

        currentData_Child = new Data_Child();

        currentData_Child.currentStage = 1;
        currentData_Child.currentPosition = new Vector3(0, 0, 0);
        currentData_Child.isDie = false;
        currentData_Child.isFriend = false;
        currentData_Child.isBye = false;
        currentData_Child.name = "코라";
        currentData_Child.age = "10살";
        currentData_Child.diaryData = "아앙, 테메와 돈독한 친구사이같다...";
        currentData_Child.childType = eChildType.KORA;

        currentData_ChildList.childDataList.Add(currentData_Child);

        Data_Child tempData = new Data_Child(currentData_Child);

        tempData.name = "테메";
        tempData.diaryData = "코라를 좋아하고 있는 듯 하다...";
        tempData.age = "9살";
        tempData.childType = eChildType.TEME;

        currentData_Child = tempData;
        currentData_ChildList.childDataList.Add(currentData_Child);
        currentData_Child = null;

        yield return StartCoroutine(SaveData_ChildList());

        ListToDictionary();

        Debug.Log("Save Finish!");
        yield break;
    }

    public IEnumerator TestLoad()
    {
        yield return StartCoroutine(LoadData_ChildList());

        ListToDictionary();

        isLoad = true;


        foreach (var item in currentData_ChildList.childDataList)
        {
            Debug.Log("애들 이름 : " + item.name);
        }
        Debug.Log("Load Finish!");

        yield break;
    }

    #endregion

    /// <summary>
    /// List에 있는 애들을 Dictionary로 전환해줍니다. 용도? : 데이터 로드 후 인게임에서 사용할...수도 있는 딕셔너리로 전환시킵니다. 
    /// </summary>
    public void ListToDictionary()
    {
        currentData_ChildDict = new Dictionary<eChildType, Data_Child>();
        currentData_ChildDict.Clear();


        foreach (var child in currentData_ChildList.childDataList)
        {
            currentData_ChildDict.Add(child.childType, child);
        }

    }

    /// <summary>
    /// 딕셔너리에 있는 것들을 currentData_ChildList.childDataList로 옮깁니다.
    /// 기존의 childDataList는 사라집니다.
    /// </summary>
    public void DictionaryToList()
    {
        currentData_ChildList.childDataList = new List<Data_Child>();
        currentData_ChildList.childDataList.Clear();

        //딕셔너리의 Values들만 ToList를 이용하여 List로 
        currentData_ChildList.childDataList = currentData_ChildDict.Values.ToList();


        foreach (var item in currentData_ChildList.childDataList)
        {
            Debug.Log("애들 이름 : " + item.name);
        }


    }

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

