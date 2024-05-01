using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : BaseManager<MusicMgr>
{
    //背景音乐组件
    private AudioSource bkMusic=null;
    //背景音乐大小
    private float bkValue = 1;

    //音效组件
    private GameObject soundObj=null;
    //音效列表
    private List<AudioSource> soundList = new List<AudioSource>();
    //音效大小
    private float soundValue = 1;
    /// <summary>
    /// 实时检测音效是否播放完毕，完毕即删除
    /// </summary>
    public MusicMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(Update);
    }
    private void Update()
    {
        for(int i=soundList.Count-1; i>=0; i--)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }
    /// <summary>
    /// 设置背景音乐音量大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeBKValue(float value)
    {
        bkValue = value;
        if(bkMusic == null)
            return;
        bkMusic.volume = bkValue;
    }
    /// <summary>
    /// 改变所有音效大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundValue(float value)
    {
        soundValue = value;
        for(int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = soundValue;
        }
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBKMusic(string name)
    {
        if (bkMusic == null)
        {
            GameObject obj = new GameObject(name);
            obj.name = "BKMusic";
            bkMusic = obj.AddComponent<AudioSource>();
        }
        ResMgr.GetInstance().LoadAsync<AudioClip>("Music/BK/" + name, (clip) =>
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkValue;
            bkMusic.Play();
        });
    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }
    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBKMusic()
    {
        if (bkMusic == null)
            return; 
        bkMusic.Stop();
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name,bool isLoop ,UnityAction<AudioSource> callBack=null)
    {
        if(soundObj == null)
        {
            soundObj = new GameObject(name);
            soundObj.name = name;
        }
        //当音效资源加载完毕，添加并播放音效
        ResMgr.GetInstance().LoadAsync<AudioClip>("Music/Sound/" + name, (clip) =>
        {
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.loop = isLoop;
            audioSource.volume = bkValue;
            audioSource.Play();
            soundList.Add(audioSource);
            if (callBack != null)
                callBack(audioSource);
        });
    }
    /// <summary>
    /// 停止音效
    /// </summary>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            GameObject.Destroy(source.gameObject);
        }
    }
}
