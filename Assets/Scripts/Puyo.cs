using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PuyoColor
{
    Red,Green,Blue,Yellow,Nuissance,NA
}
public class Puyo 
{
    private int mAdjCount; public int MAdjCount { get { return mAdjCount; } }
    private int mAdjDir; public int MAdjDir { get { return mAdjDir; } }
    private PuyoColor mColor; public PuyoColor MColor { get { return mColor; } }
    private PuyoObj mObj; public PuyoObj MObj
    {
        get { return mObj; }
        set { mObj = value;mObj.MPuyo = this; }
    }
    public Puyo(PuyoColor color)
    {
        mAdjCount = 0;
        mAdjDir = 1 << 4;
        mColor = color;
    }

}
