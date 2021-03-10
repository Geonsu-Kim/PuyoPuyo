using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Puyo[,] mPuyoes; public Puyo[,] MPuyoes { get { return mPuyoes; } }

    private Transform mParent;
    private Queue<PuyoColor> mDropSet; public Queue<PuyoColor> MPuyoDropSet { get { return mDropSet; } }
    private BoardRule rule;
    private PuyoTsumoObj[] mNext;public PuyoTsumoObj MCurTsumo { get { return mNext[0]; } }

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
            mNext[i] = PuyoPoolManager.Instance.PuyoTsumoPool[i].GetComponent<PuyoTsumoObj>();
            mNext[i].transform.SetParent(mParent);
            mNext[i].SetPos(Util.nextPos[i]);
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
    public IEnumerator DropPuyo(Returnable<bool> ret, Returnable<bool> gameEnd)
    {
        bool isDropped = false;
        do
        {
            yield return YieldInstructionCache.WaitForSeconds(0.16f);
            mNext[0].Drop();
            isDropped = MustStop();

        } while (!isDropped);
        PutInBoard(mNext[0].MAround, mNext[0].MAXis);

        mNext[0].DetachPuyo();
        ChangeOrder();
        gameEnd.value = IsGameEnd();
        ret.value = true;
    }
    public void PutInBoard(PuyoObj obj1, PuyoObj obj2)
    {
        mPuyoes[(int)(obj1.transform.position.y + 5.5f), (int)(obj1.transform.position.x + 2.5f)] = obj1.MPuyo;
        mPuyoes[(int)(obj2.transform.position.y + 5.5f), (int)(obj2.transform.position.x + 2.5f)] = obj2.MPuyo;
    }
    public void ChangeOrder()
    {
        PuyoTsumoObj temp = mNext[0];

        mNext[0] = mNext[1];
        mNext[1] = mNext[2];
        mNext[2] = mNext[3];
        mNext[3] = temp;
        SetPuyo(mNext[3]);
        for (int i = 0; i < 4; i++)
        {
            mNext[i].SetPos(Util.nextPos[i]);
        }
    }

    public IEnumerator AfterDropProcess()
    {
        yield break;
    }

    bool MustStop()
    {
        float row = mNext[0].ConvertRow();
        float col = mNext[0].ConvertCol();
        if (row == 0f) return true;
        if (row - (int)row == 0) return false;
        else
        {
            row += 0.5f;
            switch (mNext[0].MState)
            {
                case DirState.Right:
                    return ExsistPuyo(row - 1, col + 1) || ExsistPuyo(row - 1, col);
                case DirState.Up:
                    return ExsistPuyo(row - 1, col);
                case DirState.Down:
                    return ExsistPuyo(row - 2, col);
                case DirState.Left:
                    return ExsistPuyo(row - 1, col - 1) || ExsistPuyo(row - 1, col);
            }
        }
        return true;
    }
    bool ExsistPuyo(float row, float col)
    {
            if (mPuyoes[(int)row, (int)col] == null) 
                return false;
        return true;
    }
    bool IsGameEnd()
    {
        return mPuyoes[11, 2] != null;
    }
    public bool CheckMovable(int key)
    {
        float row = mNext[0].ConvertRow();
        float col = mNext[0].ConvertCol();
        if (key==0)
        {
            if (col < 1) return false;
            switch (mNext[0].MState)
            {
                case DirState.Right:
                    return !ExsistPuyo(row, col - 1);
                case DirState.Up:
                    return !ExsistPuyo(row, col-1);
                case DirState.Down:
                    return !ExsistPuyo(row - 1, col-1);
                case DirState.Left:
                    return col>=2&&!ExsistPuyo(row, col - 2);
            }
        }
        else
        {
            if (col >= 5) return false;
            switch (mNext[0].MState)
            {
                case DirState.Right:
                    return col<=3&&!ExsistPuyo(row, col + 2);
                case DirState.Up:
                    return !ExsistPuyo(row , col+1);
                case DirState.Down:
                    return !ExsistPuyo(row - 1, col+1);
                case DirState.Left:
                    return !ExsistPuyo(row, col + 1);
            }
        }
        return false;
    }


}
