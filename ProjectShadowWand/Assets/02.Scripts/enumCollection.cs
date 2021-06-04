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
    WALL,
    LADDER,
}

public enum eLiftState
{
    STOP,
    FIRST,
    SECOND,
}

public enum eLiftMoveType
{
    LEFTNRIGHT,
    UPNDOWN
}

public enum eDirection
{
    UP,
    RIGHT,
    DOWN,
    LEFT,
}

public enum eState // 상태를 정의해놓습니다.
{
    PLAYER_DEFAULT,
    PLAYER_JUMP,
    PLAYER_CLIMB_LADDER,
    PLAYER_FALL,
    PLAYER_GLIDE,
    PLAYER_SKILL_WATER,
    PLAYER_SKILL_LIGHTNING,
    PLAYER_DIE,
    PLAYER_INTERACT,
    // PLAYER_PUSH,
    //PLAYER_CATCH,
    NONE,

}

public enum eSkill
{
    RESTORE,
    WINDGILDE,
    WATERWAVE,
    LIGHTNINGSHOCK,
}

public enum eFireDirection
{
    twoDirection, fourDirection, oneDirection,
}


public enum eCutsceneType
{
    INTRO,
    UNLOCK_WIND,
    UNLOCK_WATER,
    UNLOCK_LIGHTNING,
}
public enum eUItype
{
    NONE, DIARY, SAVE, LOAD, PAUSE,
    SETTINGS, BLACKSCREEN,
    CUTSCENE,
    SOULMEMORY_01, SOULMEMORY_02, SOULMEMORY_03,
}

public enum eElectricableType
{
    DESTROY,
    WORK,
}

public enum eBurnableType
{


    /// <summary>
    /// 불이 붙으면 지정한 초 후에 타서 사라집니다.
    /// </summary>
    BURN,

    /// <summary>
    /// 불이 붙지만 연기만 나오고, 타서 사라지지 않습니다.
    /// </summary>
    SMOKE,
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
    OnlyPlayerContact = 3,
    Water = 4,
    UI = 5,
    _LAYER6 = 6,
    _LAYER7 = 7,
    FlamingObject = 8,
    Ground = 9,
    Ground_Soft = 10,
    Ground_Hard = 11,
    Player = 12,
    LightBoom = 13,
    Child = 14,
    WeatherLight = 15,
    SubLight = 16,
    WeatherFx_Default = 17,
    WeatherFx_withOpaqueTex = 18,
    Tornado_Collider = 19,
    Tornado_Trigger = 20,
    AreaEffector = 21,
    Totem = 22,
    TornadoBreakLayer = 23,
    Ladder = 24,
    Fire = 25,
}

public enum eSortingLayer
{
    Default,
    Far,
    Middle,
    Near_Back,
    VFX_Back,
    Player,
    VFX_Front,
    Near_Front,
    SuperNear,
}
public enum eQuestCode
{
    MOM_AND_BABY_01,
    MOM_AND_BABY_02,
    MOM_AND_BABY_03,

}

/// <summary>
/// //Enum들을 모아두려고 노력한 공간입니다.
/// </summary>
public class enumCollection : MonoBehaviour
{

}
