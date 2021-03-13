using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PuyoColor
{
    NA=-1,Red, Green,Blue,Yellow,Nuissance
}
public class Puyo 
{
    private int mAdjCount; public int MAdjCount { get { return mAdjCount; } }
    private int mAdjDir; public int MAdjDir { get { return mAdjDir; } } // 11000:right 10100:up 10010:down 10001:left
    private PuyoColor mColor; public PuyoColor MColor { get { return mColor; }set { mColor = value;} }
    private PuyoObj mObj;public PuyoObj MObj { get { return mObj; } set { mObj = value;mObj.MPuyo = this; } }
    public Puyo(PuyoColor color, PuyoObj obj)
    {
        mAdjCount = 0;
        mAdjDir = 0;
        mColor = color;
        mObj = obj;
    }
    public void SetPuyoColor(PuyoColor color)
    {
        mColor = color;
    }
    public void ArrangeDrop(float distance,float duration=1.0f)
    {
        mObj.ArrangeDrop(distance, duration);
    }
}
