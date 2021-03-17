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
    private bool mVisited; public bool MVisited { get { return mVisited; } set { mVisited = value; } }
    private PuyoColor mColor; public PuyoColor MColor { get { return mColor; }set { mColor = value;} }
    private PuyoState mState; public PuyoState MState { get { return mState; } set { mState = value; } }
    private PuyoObj mObj;public PuyoObj MObj { get { return mObj; } set { mObj = value;mObj.MPuyo = this; } }

    private Puyo[] mAdjPuyo; public Puyo[] MAdjPuyo { get { return mAdjPuyo; } set { mAdjPuyo = value;  } }
    public Puyo(PuyoColor color, PuyoObj obj)
    {
        mColor = color;
        mVisited = false;
        mState = PuyoState.Normal;
        mObj = obj;
        mAdjPuyo = new Puyo[4];
    }
    public void SetPuyoColor(PuyoColor color)
    {
        mColor = color;
        mState = PuyoState.Normal;
        mVisited = false;
        for (int i = 0; i < 4; i++)
        {
            mAdjPuyo[i] = null;
        }
    }
    public void ArrangeDrop(float distance,float duration=1.0f)
    {
        for (int i = 0; i < 4; i++)
        {
            DetachAdj(i);
        }
        mObj.UpdateSprite(0);
        mObj.ArrangeDrop(distance, duration);
    }
    public void StartPopping()
    {
        mObj.StartPoppoing();
    }
    public void SetActiveFalse()
    {
        mObj.gameObject.SetActive(false);
    }
    public void DetachAdj(int adj)
    {
        if (mAdjPuyo[adj] == null) return;
        mAdjPuyo[adj].MAdjPuyo[adj > 1 ? adj - 2 : adj + 2] = null;
        mAdjPuyo[adj].UpdateSprite();
        mAdjPuyo[adj] = null;
    }
    public void CalcAdj(int adj,Puyo puyo)
    {
        mAdjPuyo[adj] = puyo;
    }
    public void UpdateSprite()
    {
        mObj.UpdateSprite(GetAdj());
    }
    public int GetAdj()
    {
        int adjDir = 0;
        for (int i = 0; i < 4; i++)
        {
            if (mAdjPuyo[i] != null) adjDir |= 1 << i;
        }
        return adjDir;
    }
}
