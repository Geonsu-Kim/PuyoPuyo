using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Puyo[,] mPuyoes; public Puyo[,] MPuyoes { get { return mPuyoes; } }

    private Transform mParent;
    private Queue<PuyoColor> mDropSet; public Queue<PuyoColor> MPuyoDropSet { get { return mDropSet; } }
    private PuyoTsumoObj mNext1;
    private PuyoTsumoObj mNext2;
    private PuyoTsumoObj mNext3;
    private PuyoTsumoObj mCur;
    private BoardRule rule;
    public Board(Transform parent)
    {
        mPuyoes = new Puyo[Util.row, Util.col];
        rule = new BoardRule();
        mDropSet = new Queue<PuyoColor>();
        mParent = parent;

    }
    public void CreateTsumo()
    {
        mCur = PuyoPoolManager.Instance.PuyoTsumoPool[0].GetComponent<PuyoTsumoObj>();
        mNext1 = PuyoPoolManager.Instance.PuyoTsumoPool[1].GetComponent<PuyoTsumoObj>();
        mNext2 = PuyoPoolManager.Instance.PuyoTsumoPool[2].GetComponent<PuyoTsumoObj>();
        mNext3 = PuyoPoolManager.Instance.PuyoTsumoPool[3].GetComponent<PuyoTsumoObj>();

        mCur.transform.SetParent(mParent);
        mNext1.transform.SetParent(mParent);
        mNext2.transform.SetParent(mParent);
        mNext3.transform.SetParent(mParent);

        mCur.Move(Util.startPos);
        mNext1.Move(Util.next1Pos);
        mNext2.Move(Util.next2Pos);
        mNext3.Move(Util.next3Pos);
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
