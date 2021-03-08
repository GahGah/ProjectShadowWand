using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public GameObject targetObject;
    public Rigidbody2D rb;

    public float speed;

    public int layerMask; //레이어 마스크

    public Bounds bounds; // 몬스터의 사각형 영역

    public RaycastHit2D[] hits; //레이캐스트 히트. LightObject에서 조종한다.
    public bool[] hitsLog; //사각형 영역의 레이캐스트가 hit했는지 인스펙터에 알려줌

    public Color[] colors;

    public Vector2 relOffset;

    public Vector2[] directions; // 원래는 빛을 향하는 방향이었음. 하지만 LightObject의 경우에는 반대가 되야겠지?
    public Vector3[] path; // 바운드의 꼭짓점 4개 위치.

    [Tooltip("몬스터가 그림자에 들어가있는 상태면 true, 아니면 false를 갖습니다.")]
    public bool inShadow;

    protected void StartSetting()
    {
        rb = GetComponent<Rigidbody2D>();
        hits = new RaycastHit2D[4];

        hitsLog = new bool[4];

        layerMask = (1 << LayerMask.NameToLayer("Monster")); //hit가 자기 자신에게는 부딪히지 않기 위해 
        layerMask = ~layerMask;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            bounds = renderer.bounds;

            Debug.Log("렌더러 기준으로 바운딩");

        }
        else
        {

            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                bounds = collider.bounds;

                Debug.Log("콜라이더 기준으로 바운딩");

            }
        }


        ColorSetting();
        if (MonsterManager.Instance.monsterList.Contains(this) == false) //자기 자신이 안들어가있다면
        {
            MonsterManager.Instance.AddMonsterToList(this); //넣는다.
        }
    }

    void Start()
    {
        StartSetting();

    }

    //void Update()
    //{

    //}

    protected void ColorSetting()
    {
        colors = new Color[]
        {
            Color.red,
            Color.yellow,
            Color.green,
            Color.blue
        };
    }


    /// <summary>
    ///hitsLog를 업데이트합니다.
    /// </summary>
    public void UpdateHitsLog() //hitsLog를 hits랑 동일하게 합니다.
    {
        hitsLog[0] = hits[0];
        hitsLog[1] = hits[1];
        hitsLog[2] = hits[2];
        hitsLog[3] = hits[3];
    }

    /// <summary>
    /// 모든 hits가 트루일때 트루를 반환함~
    /// </summary>
    public bool isAllHitsTrue()
    {
        if (hits[0] && hits[1] && hits[2] && hits[3])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
