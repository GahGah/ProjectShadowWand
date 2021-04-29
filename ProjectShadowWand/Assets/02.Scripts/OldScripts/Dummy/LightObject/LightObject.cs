//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Experimental.Rendering.Universal; // Light2D를 가져오기 위해...
///// <summary>
///// 빛을 내뿜는다고 가정하는 오브젝트입니다.
///// </summary>
///// 
//public class LightObject : MonoBehaviour
//{
//    [Tooltip("씬 내 몬스터가 몇마리~?")]
//    public int monsterCount;

//    [HideInInspector]
//    [Tooltip("해당 몬스터가 그림자에 들어가있는 상태라면 true를, 아니라면 false를 갖습니다. 인덱스는 몬스터 리스트와 동일합니다. ")]
//    public bool[] shadowJudgment;

//    [HideInInspector]
//    public int layerMask;

//    private Mesh mesh;

//    [SerializeField]
//    protected Light2D light2D;

//    void Start()
//    {
//        StartSetting();
//    }

//    protected virtual void StartSetting()
//    {
//        shadowJudgment = new bool[MonsterManager.Instance.monsterList.Count];
//        if (LightObjectManager.Instance.lightObjectList.Contains(this) == false) //자기 자신이 안들어가있다면
//        {
//            LightObjectManager.Instance.AddLightObjectToList(this); //넣는다.
//        }

//        //if (lightObjectType == eLightObjectType.DISTANCE)
//        //{
//            layerMask = ((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Monster")));
//            layerMask = ~layerMask;
//            //  layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));

//        //}
//        //else
//        //{
//        //    //  특정 2개이상 layer raycast 제외하기
//        //    layerMask = ((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Monster")));
//        //    layerMask = ~layerMask;
//        //    //한개만 제외
//        //    //layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));

//        //}

 
//    }

//    public virtual void UpdateShadowJudgement()
//    {

//    }

//    /// <summary>
//    /// UpdateShadowJudgement 이후에 업데이트를 합니다.
//    /// </summary>
//    public virtual void LateUpdateLightObject()
//    {

//    }

//    public void KillLightObject()
//    {
//        LightObjectManager.Instance.RemoveLightObjectToList(this);
//        Destroy(gameObject);
//    }
//    Mesh SpriteToMesh(Sprite sprite)
//    {
//        Mesh mesh = new Mesh();
//        mesh.vertices = Array.ConvertAll(sprite.vertices, i => (Vector3)i);
//        mesh.uv = sprite.uv;
//        mesh.triangles = Array.ConvertAll(sprite.triangles, i => (int)i);

//        return mesh;
//    }


//}
