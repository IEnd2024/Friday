using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入控制模块
/// </summary>
public class InputMgr : BaseManager<InputMgr>
{
    private bool isStart=false;
    /// <summary>
    /// 构造函数添加Update监听
    /// </summary>
    public InputMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(MyUpdate);
    }
    /// <summary>
    /// 是否开启输入检测
    /// </summary>
    /// <param name="isOpen"></param>
    public void StartOrEnd(bool isOpen)
    {
        isStart = isOpen;
    }
    /// <summary>
    /// 检测按键输入并分发事件
    /// </summary>
    /// <param name="key"></param>
    private void CheckKeyCode(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            EventCenter.GetInstance().EventTrigger("某键按下", key);
        }
        if (Input.GetKeyUp(key))
        {
            EventCenter.GetInstance().EventTrigger("某键抬起", key);
        }
    }
    
    private void MyUpdate()
    {
        //没有开启输入检测直接return
        if (!isStart) { return; }
        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.D);
    }
}
