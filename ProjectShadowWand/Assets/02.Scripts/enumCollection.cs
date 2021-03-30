using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

/// <summary>
/// // 날씨의 종류입니다. 중첩이 고려될 수도 있기에, 확장성을 위해 BitMask를 사용합니다.
/// </summary>
public enum eWeatherType
{
    NONE = 0,
    SUNNY = 1 << 0,
    RAINY = 1 << 1,
    WINDY = 1 << 2,
    Opt4 = 1 << 3,
    Opt5 = 1 << 4,
    Opt6 = 1 << 5,
    Opt7 = 1 << 6
}

public enum eErrorType
{
    MANAGER_INIT_ERROR = -1
}

/// <summary>
/// //Enum들을 모아두려고 노력한 공간입니다.
/// </summary>
public class enumCollection : MonoBehaviour
{

}
