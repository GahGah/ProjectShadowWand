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

    [Header("UI 효과음 클립")]
    public AudioClip ui_button_select;
    public AudioClip ui_button_push;
    public AudioClip ui_imageButton_push;
    public AudioClip ui_selector_move;
    public AudioClip ui_pause_on;

    [Header("대화 텍스트 출력 클립")]
    public AudioClip talk_textOut;


    private AudioClip currentSfxClip;

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

    public void Play_Skill_Wind()
    {
        audioSource_sfx.volume = 1f;
        currentSfxClip = skill_wind;
        audioSource_sfx.PlayOneShot(skill_wind);
    }

    public void Stop_Skill_Wind()
    {
        currentSfxClip = skill_wind;
        CloseVolumeLerp(audioSource_sfx);
    }

    public void Play_Skill_Water_Set()
    {
        audioSource_sfx.volume = 1f;
        currentSfxClip = skill_water_set;
        audioSource_sfx.PlayOneShot(skill_water_set);
    }

    public void Stop_Skill_Water_Set()
    {
        audioSource_sfx.volume = 1f;
        currentSfxClip = skill_water_set;
        audioSource_sfx.Stop();
    }

    public void Play_Skill_Water_Splash()
    {
        audioSource_sfx.volume = 1f;
        currentSfxClip = skill_water_splash;
        audioSource_sfx.PlayOneShot(skill_water_splash);
    }


    public void Play_Skill_Lightning()
    {
        audioSource_sfx.volume = 1f;
        currentSfxClip = skill_lightning;
        audioSource_sfx.PlayOneShot(skill_lightning);
    }

    public void Play_Talk_TextOut()
    {
        audioSource_sfx.volume = 0.5f;
        currentSfxClip = talk_textOut;
        audioSource_sfx.PlayOneShot(talk_textOut);
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
        float sec = 2f;
        if (_source == audioSource_sfx)
            sec = 0.5f;



        if (_isStart)
        {
            _source.volume = 0f;
            _source.Play();
        }
        else
        {
            if (_source.isPlaying == false)
            {
                //Debug.Log("소리가 플레이되고 있는 상태가 아닙니다. 그냥 시작할게요.");
                //_isStart = true;
                //_source.Stop();
                //if (_source == audioSource_sfx)
                //{
                //    //_source.PlayOneShot(currentSfxClip);
                //}
                //else
                //{
                //    _source.volume = 1f;
                //    _source.Play();
                //}

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

            if (_source.isPlaying == false)
            {
                yield break;
            }
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
            _source.volume = 1f;
        }
        currentSfxClip = null;

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
        Debug.Log("CloseVL");
        CloseVolumeLerp(audioSource_bgm);
        CloseVolumeLerp(audioSource_evm);
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
        audioSource_sfx.volume = 1f;
        audioSource_sfx.PlayOneShot(walk_groundHard);

    }
    public void Play_WalkSoft()
    {
        // audioSource_sfx.clip = walk_groundSoft;
        audioSource_sfx.PlayOneShot(walk_groundSoft);

    }

    public void Play_UI_Selector_Move()
    {
        audioSource_sfx.volume = 1f;
        audioSource_sfx.PlayOneShot(ui_selector_move);
    }

    public void Play_UI_Pause_On()
    {
        audioSource_sfx.volume = 1f;
        audioSource_sfx.PlayOneShot(ui_pause_on);
    }

    public void Play_Button()
    {
        audioSource_sfx.PlayOneShot(ui_button_select);
    }

    public void Play_UI_Button_Push()
    {
        audioSource_sfx.volume = 1f;
        audioSource_sfx.PlayOneShot(ui_button_push);
    }

    public void Play_UI_ImageButton_Push()
    {
        audioSource_sfx.volume = 1f;
        audioSource_sfx.PlayOneShot(ui_imageButton_push);
    }
}
