using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum eResolutionData
{
    HD, FHD, QHD
}
public enum eBlockType
{
    NONE,
    GROUND,
    MOVING_GROUND,
    WALL

}
public enum eMonsterType
{
    A, B, C
}
public enum eState // 상태를 정의해놓습니다.
{
    PLAYER_DEFAULT,
    PLAYER_JUMP,
    PLAYER_AIR,
    PLAYER_DIE,

    /// <summary>
    /// 몬스터가 그림자 안에 있고, 추격 상태가 아닐 때를 의미합니다.
    /// </summary>
    MONSTER_DEFAULT,
    /// <summary>
    /// 몬스터가 그림자 안에 있고, 추격 상태일때를 의미합니다.
    /// </summary>
    MONSTER_CHASE,

    /// <summary>
    /// 몬스터가 그림자 밖에 나왔을 때를 의미합니다.
    /// </summary>
    MONSTER_OUTSHADOW,

    /// <summary>
    /// 몬스터가 뒈짖
    /// </summary>
    MONSTER_DIE
}


public enum eChildOption
{
    TAIN,
    FRIEND
}
public enum eUItype
{
    NONE, DIARY, SAVE, LOAD
}

public enum eChildType
{
    KORA, TEME,
}

/// <summary> 메인 날씨의 종류입니다. </summary>
public enum eMainWeatherType
{
    SUNNY = 0,
    RAINY = 1
}

/// <summary> 서브 날씨의 종류입니다. 메인 날씨와 중첩되어 사용됩니다. </summary>
public enum eSubWeatherType
{
    NONE = 0,
    WINDY = 1
}

/// <summary> 에러 관리를 위한 enum입니다. 특정 함수에서 상황에 맞게 반환됩니다. </summary>
public enum eErrorType
{
    NONE = 0,
    MANAGER_INIT_ERROR = -1
}

/// <summary> 레이어 관리를 위한 eNum입니다. 레이어 이름이나 순서가 바뀐다면 직접 갱신을 부탁합니다. </summary>
public enum eLayer
{
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    _LAYER3 = 3,
    Water = 4,
    UI = 5,
    _LAYER6 = 6,
    _LAYER7 = 7,
    Monster = 8,
    Ground = 9,
    GroundSoft = 10,
    GroundHard = 11,
    Player = 12,
    LightBoom = 13,
    Child = 14,
    WeatherLight = 15,
    SubLight = 16,
    WeatherFx_Default = 17,
    WeatherFx_withOpaqueTex = 18,
}

/// <summary>
/// //Enum들을 모아두려고 노력한 공간입니다.
/// </summary>
public class enumCollection : MonoBehaviour
{

}
