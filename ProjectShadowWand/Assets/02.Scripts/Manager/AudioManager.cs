using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 모든 소리를 책임지는 오디오 매니저
/// </summary>
public class AudioManager : Manager<AudioManager>
{

    [Header("오디오 믹서")]
    public AudioMixer audioMixer;

    [Header("오디오 소스")]
    public AudioSource audioSource_bgm;
    public AudioSource audioSource_evm;
    public AudioSource audioSource_sfx;

    [Header("배경음악 클립")]

    public AudioClip bgm_stage_main;
    public AudioClip bgm_stage_00;
    public AudioClip bgm_stage_01;
    public AudioClip bgm_stage_02;

    [Header("환경음 클립")]
    public AudioClip evm_stage_00;
    public AudioClip evm_stage_01;
    public AudioClip evm_stage_02;

    [Header("걷는 효과음 클립")]

    public AudioClip walk_groundHard;
    public AudioClip walk_groundSoft;

    [Header("스킬 효과음 클립")]

    public AudioClip skill_wind;

    public AudioClip skill_water_set;
    public AudioClip skill_water_splash;

    public AudioClip skill_lightning;

    [Header("버튼 효과음 클립")]
    public AudioClip ui_button_select;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init()
    {

    }

    public void ResetBGM()
    {
        Debug.Log("resetBgm...");
        audioSource_bgm.Stop();
        audioSource_evm.Stop();
    }

    public void Play_Button()
    {
        audioSource_sfx.PlayOneShot(ui_button_select);
    }

    public void StartVolumeLerp(AudioSource _source)
    {
        StartCoroutine(ProcressVolumeLerp(_source, true));
    }

    public void CloseVolumeLerp(AudioSource _source)
    {
        StartCoroutine(ProcressVolumeLerp(_source, false));
    }

    public void StartChangeMusic(AudioSource _as, AudioClip _ac)
    {
        StartCoroutine(ChangeBGM(_as, _ac));
    }

    public IEnumerator ChangeBGM(AudioSource _as, AudioClip _ac)
    {
        yield return StartCoroutine(ProcressVolumeLerp(_as, false));
        _as.clip = _ac;
        yield return StartCoroutine(ProcressVolumeLerp(_as, true));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_source">오디오소스</param>
    /// <param name="_isStart">브금을 시작하는 것인지, 종료하는 것인지를 뜻합니다.</param>
    /// <returns></returns>
    public IEnumerator ProcressVolumeLerp(AudioSource _source, bool _isStart)
    {
        float timer = 0f;
        float progress = 0f;
        float sec = 1f;



        if (_isStart)
        {
            _source.volume = 0f;
            _source.Play();
        }
        else
        {
            if (_source.isPlaying == false)
            {
                Debug.Log("소리가 플레이되고 있는 상태가 아닙니다. 그냥 시작할게요.");
                _isStart = true;
                _source.Play();
            }
            else
            {
                _source.volume = 1f;
            }
        }


        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / sec;

            if (_isStart)
            {
                _source.volume = progress;
            }
            else
            {
                _source.volume = 1f - progress;
            }

            yield return null;
        }

        if (_isStart)
        {
            _source.volume = 1f;
        }
        else
        {
            _source.volume = 0f;
            _source.Stop();
        }


        yield break;
    }


    public void Play_Bgm_StageMain()
    {
        // ResetBGM();
        //audioSource_bgm.clip = bgm_stage_main;
        StartChangeMusic(audioSource_bgm, bgm_stage_main);
        // audioSource_evm.clip = bgm_stage_main;
    }

    public void Stop_Bgm()
    {
        CloseVolumeLerp(audioSource_bgm);
    }

    public void Play_Bgm_Stage00()
    {
        // ResetBGM();

        StartChangeMusic(audioSource_bgm, bgm_stage_00);
        StartChangeMusic(audioSource_evm, evm_stage_00);


    }

    public void Play_Bgm_Stage01()
    {
        //ResetBGM();

        StartChangeMusic(audioSource_bgm, bgm_stage_01);
        StartChangeMusic(audioSource_evm, evm_stage_00);
    }

    public void Play_Bgm_Stage02()
    {
        //  ResetBGM();

        StartChangeMusic(audioSource_bgm, bgm_stage_02);
        StartChangeMusic(audioSource_evm, evm_stage_02);
    }
    public void Play_WalkHard()
    {
        audioSource_sfx.PlayOneShot(walk_groundHard);

    }
    public void Play_WalkSoft()
    {
        // audioSource_sfx.clip = walk_groundSoft;
        audioSource_sfx.PlayOneShot(walk_groundSoft);

    }

}
