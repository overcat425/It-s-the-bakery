using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("���")]
    [SerializeField] AudioClip[] bgmClips;
    AudioSource[] bgmSources;
    [SerializeField] float bgmVolume;
    AudioHighPassFilter bgmHighPassFilter;

    [Header("ȿ����")]
    [SerializeField] AudioClip[] effectClips;
    [SerializeField] float effectVolume;
    AudioSource[] effectSources;
    [SerializeField] int channels;
    int channelIndex;
    public enum Effect { Click, Btn, Customer, Counter, Horn, Horn2, Gauge, Scale }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject);}
        //instance = this;
        Init();
    }
    private void Start()
    {
        SoundManager.instance.PlayBgm(true);
    }
    void Init()
    {
        GameObject bgmObject = new GameObject("BgmObject");
        bgmObject.transform.parent = transform;     // ����Ŵ��� �ڽ� ������Ʈ�� ����
        bgmSources = new AudioSource[2];        // ���� ��� 2��
        for (int i = 0; i < bgmSources.Length; i++)
        {
            bgmSources[i] = bgmObject.AddComponent<AudioSource>();  // bgm ���� �� bgm�� ������Ʈ��
            bgmSources[i].playOnAwake = true;
            bgmSources[i].loop = true;
            bgmSources[i].clip = bgmClips[i];
            bgmSources[i].volume = bgmVolume;
        }
        bgmHighPassFilter = Camera.main.GetComponent<AudioHighPassFilter>();

        GameObject effectObject = new GameObject("EffectObject");
        effectObject.transform.parent = transform;
        effectSources = new AudioSource[channels];
        for (int i = 0; i < effectSources.Length; i++)
        {
            effectSources[i] = effectObject.AddComponent<AudioSource>();
            effectSources[i].playOnAwake = false;
            effectSources[i].volume = effectVolume;
            effectSources[i].bypassListenerEffects = true; // ����������н����� ����
        }
    }
    public void PlaySound(Effect effect)
    {
        for (int i = 0; i < effectSources.Length; i++)
        {
            int loopChannel = (i + channelIndex) % effectSources.Length;    // ä�μ��� �Ѿ�� �ʵ��� ��
            if (effectSources[loopChannel].isPlaying) continue; // ���� �����������Ʈ ��� ����
            channelIndex = loopChannel;
            effectSources[loopChannel].clip = effectClips[(int)effect];
            effectSources[loopChannel].Play();
            break;
        }
    }
    public void PlayBgm(bool isPlaying)     // ��� ����
    {
        if (isPlaying)
        {
            //bgmSources[1].Stop();
            bgmSources[0].Play();
        }
        else
        {
            bgmSources[0].Stop();
            //bgmSources[1].Play();
        }
    }
    public void StopBgm(bool isPlaying)
    {
        bgmHighPassFilter.enabled = isPlaying;
    }
    public void BtnSound()
    {
        PlaySound(Effect.Click);
    }
    public void ToggleMute()
    {
        if (AudioListener.volume > 0)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }
}
