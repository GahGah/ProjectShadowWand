using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum resolutionData
{
    HD, FHD, QHD
}
public class SettingsData // 컨테이너이기 때문에 그냥 클래스로.... 
{
    public string sfxVolume;
    public string bgmVolume;
    public string brightness;
    public resolutionData resolution;

    /// <summary>
    /// 디폴트 생성자. 기본값이 들어갑니다~~
    /// </summary>
    public SettingsData() //생성자?
    {
        sfxVolume = "0.5";
        bgmVolume = "0.5";
        brightness = "0.5";
        resolution = resolutionData.HD;
    }

    /// <summary>
    /// 데이터를 넣어서 생성합니다.
    /// </summary>
    /// <param name="data">이 데이터가 넣어집니다.</param>
    public SettingsData(SettingsData data)
    {
        CopyFrom(data);
    }

    /// <summary>
    /// 해당하는 데이터를 자신에게 복사해 넣습니다.
    /// </summary>
    /// <param name="data">복사되어서 넣어질 테이터</param>
    public void CopyFrom(SettingsData data)
    {
        this.bgmVolume = data.bgmVolume;
        this.sfxVolume = data.sfxVolume;
        this.brightness = data.brightness;
        this.resolution = data.resolution;
    }

}
