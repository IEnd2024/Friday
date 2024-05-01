using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������ģ��
/// </summary>
public class InputMgr : BaseManager<InputMgr>
{
    private bool isStart=false;
    /// <summary>
    /// ���캯�����Update����
    /// </summary>
    public InputMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(MyUpdate);
    }
    /// <summary>
    /// �Ƿ���������
    /// </summary>
    /// <param name="isOpen"></param>
    public void StartOrEnd(bool isOpen)
    {
        isStart = isOpen;
    }
    /// <summary>
    /// ��ⰴ�����벢�ַ��¼�
    /// </summary>
    /// <param name="key"></param>
    private void CheckKeyCode(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            EventCenter.GetInstance().EventTrigger("ĳ������", key);
        }
        if (Input.GetKeyUp(key))
        {
            EventCenter.GetInstance().EventTrigger("ĳ��̧��", key);
        }
    }
    
    private void MyUpdate()
    {
        //û�п���������ֱ��return
        if (!isStart) { return; }
        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.D);
    }
}
