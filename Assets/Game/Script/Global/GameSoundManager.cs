using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameSoundManager : MonoSingleton<GameSoundManager>
{
    [SerializeField] private AudioSource musicAudio, effectAudio;
    // public float MusicVolume => SettingModel.Instance.Music;
    // public float EffectAudioVolume => SettingModel.Instance.Sound;

    // private Sequence _sq;
    //
    // public void PlaySoundEffect(AudioClip clip)
    // {
    //     effectAudio.PlayOneShot(clip);
    // }
    //
    // public void PlayMusicAudio(AudioClip clip)
    // {
    //     _sq?.Kill();
    //     _sq = DOTween.Sequence()
    //         .Append(DOVirtual.Float(MusicVolume, 0, 1.5f, s => musicAudio.volume = s).SetEase(Ease.InOutQuart)
    //             .OnComplete(() =>
    //             {
    //                 musicAudio.clip = clip;
    //                 musicAudio.loop = true;
    //                 musicAudio.Play();
    //             }))
    //         .Append(DOVirtual.Float(0, MusicVolume, 1.5f, s => musicAudio.volume = s).SetEase(Ease.InOutQuart))
    //         .SetUpdate(true);
    // }
    //
    // public void StopMusicAudio()
    // {
    //     _sq?.Kill();
    //     _sq = DOTween.Sequence()
    //         .Append(DOVirtual.Float(MusicVolume, 0, 1.5f, s => musicAudio.volume = s).SetEase(Ease.InOutQuart)
    //             .OnComplete(() =>
    //             {
    //                 musicAudio.Stop();
    //                 musicAudio.clip = null;
    //             }))
    //         .SetUpdate(true);
    // }
}