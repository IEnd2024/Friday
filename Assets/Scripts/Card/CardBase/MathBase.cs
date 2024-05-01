using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathBase:BaseManager<MathBase>
{

    public bool IsOverLap(Transform myObj, Transform targetObj)
    {
        Rect rect1 = RectTransToScreenPos(myObj as RectTransform, null);
        Rect rect2 = RectTransToScreenPos(targetObj as RectTransform, null);

        return rect1.Overlaps(rect2);
    }
    /// <summary>
    /// 相机传入主要取决于rt都画布，如果画布使用camera模式，则传入对应的相机,反之为null
    /// </summary>
    /// <param name="rt"></param>
    /// <param name="cam"></param>
    /// <returns></returns>
    private Rect RectTransToScreenPos(RectTransform rt, Camera cam)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector2 v0 = RectTransformUtility.WorldToScreenPoint(cam, corners[0]);
        Vector2 v1 = RectTransformUtility.WorldToScreenPoint(cam, corners[2]);
        Rect rect = new Rect(v0, v1 - v0);
        return rect;
    }


}
