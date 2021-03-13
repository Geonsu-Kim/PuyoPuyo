using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PuyoColor
{
    NA=-1,Red, Green,Blue,Yellow,Nuissance
}
public enum PuyoState
{
    Normal,Match
}
public class Puyo 
{
    private int mAdjCount; public int MAdjCount { get { return mAdjCount; } }
    private int mAdjDir; public int MAdjDir { get { return mAdjDir; } } // 11000:right 10100:up 10010:left 10001:down
    private bool mVisited; public bool MVisited { get { return mVisited; } set { mVisited = value; } }
    private PuyoColor mColor; public PuyoColor MColor { get { return mColor; }set { mColor = value;} }
    private PuyoState mState; public PuyoState MState { get { return mState; } set { mState = value; } }
    private PuyoObj mObj;public PuyoObj MObj { get { return mObj; } set { mObj = value;mObj.MPuyo = this; } }
    
    
    public Puyo(PuyoColor color, PuyoObj obj)
    {
        mAdjCount = 0;
        mAdjDir = 0;
        mColor = color;
        mVisited = false;
        mState = PuyoState.Normal;
        mObj = obj;
    }
    public void SetPuyoColor(PuyoColor color)
    {
        mColor = color; 
        mAdjCount = 0;
        mState = PuyoState.Normal;
        mVisited = false;
        mAdjDir = 0;
    }
    public void ArrangeDrop(float distance,float duration=1.0f)
    {
        mObj.ArrangeDrop(distance, duration);
    }
    public void SetActiveFalse()
    {
        mObj.gameObject.SetActive(false);
    }
}
