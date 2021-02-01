using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 오디오 관련
배경음과 효과음이 분리되어 있다.
*/

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource fx;
    public AudioSource bgm;
    public AudioSource oil_fx;
    public AudioClip ready3;
    public AudioClip ready2;
    public AudioClip ready1;
    public AudioClip start;
    public AudioClip nearmiss;
    public AudioClip car_crash;
    public AudioClip wall_crash;
    public AudioClip turbo;
    public AudioClip turbo_human;
    public AudioClip car_gear_change;

    public AudioClip object_crash_pilon;
    public AudioClip object_crash_delinator;
    public AudioClip object_crash_drum;
    public AudioClip object_crash_plastic_wall;
    public AudioClip object_crash_direction_steal;
    public AudioClip object_crash_stone_wall;

    public AudioClip object_coin_get;

    public AudioClip goal;
    public AudioClip result_win;
    public AudioClip helli_loop;

    public AudioClip bgm_01;
    public AudioClip bgm_02;

    public AudioClip police_talk;
    public AudioClip this_is_police;

    public AudioClip car_bbang;
    public AudioClip car_bbangbbang;

    public AudioClip[] crash;
    public AudioClip[] horn;


    private void Awake()
    {
        Instance = this;
    }

    public static void PlayFx(AudioClip clip)
    {
        Instance.fx.PlayOneShot(clip);
    }

    public static void PlayBGM(AudioClip clip)
    {
        Instance.bgm.clip = clip;
        Instance.bgm.Play();
    }
}
