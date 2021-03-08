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
    public Puyo(PuyoColor color)
    {
        mAdjCount = 0;
        mAdjDir = 1 << 4;
        mColor = color;
    }
    public void SetPuyoColor(PuyoColor color)
    {
        mColor = color;
    }
}
