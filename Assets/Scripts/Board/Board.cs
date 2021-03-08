using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Puyo[,] mPuyoes; public Puyo[,] MPuyoes { get { return mPuyoes; } }

    private Transform mParent;
    private Queue<PuyoColor> mDropSet; public Queue<PuyoColor> MPuyoDropSet { get { return mDropSet; } }
    private PuyoTsumoObj[] mNext;
    private BoardRule rule;
    public Board(Transform parent)
    {
        mParent = parent;
        rule = new BoardRule();
        mPuyoes = new Puyo[Util.row, Util.col];
        mNext = new PuyoTsumoObj[4];
        mDropSet = new Queue<PuyoColor>();

    }
    public void CreateTsumo()
    {
        for (int i = 0; i < 4; i++)
        {
            mNext[i]= PuyoPoolManager.Instance.PuyoTsumoPool[i].GetComponent<PuyoTsumoObj>();
            mNext[i].transform.SetParent(mParent);
            mNext[i].Move(Util.nextPos[i]);
            SetPuyo(mNext[i]);
        }
    }
    public void SetPuyo(PuyoTsumoObj tsumo)
    {
        PuyoColor color1 = mDropSet.Dequeue();
        PuyoColor color2 = mDropSet.Dequeue();
        tsumo.SetPuyo(color1, color2);
        mDropSet.Enqueue(color1);
        mDropSet.Enqueue(color2);
    }
    public void ComposeGame()
    {
        CreateDropSet();
        CreateTsumo();
    }

    void CreateDropSet()
    {
        for (int i = 0; i < 128; i++)
        {
            int color = 0;
            if (i <= 1)
            {
                color = rule.Rule1();
                mDropSet.Enqueue((PuyoColor)color);
                color = rule.Rule1();
                mDropSet.Enqueue((PuyoColor)color);
            }
            else
            {
                color = rule.Rule2();
                mDropSet.Enqueue((PuyoColor)color);
                color = rule.Rule2();
                mDropSet.Enqueue((PuyoColor)color);

            }
        }
    }
}
