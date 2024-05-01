using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : BaseManager<MusicMgr>
{
    //�����������
    private AudioSource bkMusic=null;
    //�������ִ�С
    private float bkValue = 1;

    //��Ч���
    private GameObject soundObj=null;
    //��Ч�б�
    private List<AudioSource> soundList = new List<AudioSource>();
    //��Ч��С
    private float soundValue = 1;
    /// <summary>
    /// ʵʱ�����Ч�Ƿ񲥷���ϣ���ϼ�ɾ��
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
    /// ���ñ�������������С
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
    /// �ı�������Ч��С
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
    /// ���ű�������
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
    /// ��ͣ��������
    /// </summary>
    public void PauseBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }
    /// <summary>
    /// ֹͣ��������
    /// </summary>
    public void StopBKMusic()
    {
        if (bkMusic == null)
            return; 
        bkMusic.Stop();
    }
    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name,bool isLoop ,UnityAction<AudioSource> callBack=null)
    {
        if(soundObj == null)
        {
            soundObj = new GameObject(name);
            soundObj.name = name;
        }
        //����Ч��Դ������ϣ���Ӳ�������Ч
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
    /// ֹͣ��Ч
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
