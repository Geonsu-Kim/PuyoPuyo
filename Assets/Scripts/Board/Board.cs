using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public enum SpecialRotate
{
    Na = -1, PushUp, PushRight, PushLeft, HalfTurn
}
public class Board
{
    private Transform mParent;
    private BoardRule rule;
    private Puyo[,] mPuyoes; public Puyo[,] MPuyoes { get { return mPuyoes; } }
    private PuyoTsumoObj[] mNext; public PuyoTsumoObj MCurTsumo { get { return mNext[0]; } }
    private Queue<PuyoColor> mDropSet; public Queue<PuyoColor> MPuyoDropSet { get { return mDropSet; } }


    private SortedSet<int> emptyPos;
    private List<PuyoObj> mMovingPuyo; 
    public Board(Transform parent)
    {
        mParent = parent;
        rule = new BoardRule();
        mNext = new PuyoTsumoObj[4];
        mPuyoes = new Puyo[Util.row, Util.col];
        mDropSet = new Queue<PuyoColor>();
        emptyPos = new SortedSet<int>();
        mMovingPuyo = new List<PuyoObj>();

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
        Debug.Log(mNext[0].MState);
        bool isDropped = false;
        do
        {
            mNext[0].Drop();
            yield return YieldInstructionCache.WaitForSeconds(0.33f);

            isDropped = MustStop();

        } while (!isDropped);
        PutInBoard(mNext[0].MAround, mNext[0].MAXis);

        mNext[0].DetachPuyo();
        ChangeOrder();
        gameEnd.value = IsGameEnd();
        ret.value = true;
        yield return WaitForDropping();
        mMovingPuyo.Clear();
    }
    public void ArrangePuyo(int col)
    {
        emptyPos.Clear();
        for (int i = 0; i < Util.row; i++)
        {
            if (mPuyoes[i, col] == null)
            {
                emptyPos.Add(i);
            }
        }
        if (emptyPos.Count == 0) return;
        int firstValue = emptyPos.Min;
        for (int i = emptyPos.Min+1; i < Util.row; i++)
        {
            Puyo puyo = mPuyoes[i, col];
            if (puyo == null) continue;
            puyo.ArrangeDrop(i - firstValue, (i - firstValue)*0.1f);
            mMovingPuyo.Add(puyo.MObj);
            mPuyoes[firstValue, col] = puyo;
            mPuyoes[i, col] = null;
            emptyPos.Remove(firstValue);
            emptyPos.Add(i);
            firstValue = emptyPos.Min; 
        }
    }
    private IEnumerator WaitForDropping()
    {
        bool bContinue = false;
        do
        {
            bContinue = false;
            for (int i = 0; i < mMovingPuyo.Count; i++)
            {
                if (mMovingPuyo[i].isDropping)
                {
                    bContinue = true;
                    break;
                }
            }
            yield return YieldInstructionCache.WaitForSeconds(0.05f);
        } while (bContinue);
    }


    public void PutInBoard(PuyoObj obj1, PuyoObj obj2)
    {
        int obj1Row = (int)obj1.transform.position.y;
        int obj1Col = (int)obj1.transform.position.x;
        int obj2Row = (int)obj2.transform.position.y;
        int obj2Col = (int)obj2.transform.position.x;

        mPuyoes[obj1Row, obj1Col] = obj1.MPuyo;
        mPuyoes[obj2Row, obj2Col] = obj2.MPuyo;
        if (!ExistPuyo(obj1Row-1, obj1Col))
        {
            ArrangePuyo(obj1Col);
        }
        if (!ExistPuyo(obj2Row-1, obj2Col))
        {
            ArrangePuyo(obj2Col);
        }
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
        if (row - (int)row != 0) return false;
        else
        {
            switch (mNext[0].MState)
            {
                case DirState.Right:
                    return ExistPuyo(row - 1, col + 1) || ExistPuyo(row - 1, col);
                case DirState.Up:
                    return ExistPuyo(row - 1, col);
                case DirState.Down:
                    return ExistPuyo(row - 2, col);
                case DirState.Left:
                    return ExistPuyo(row - 1, col - 1) || ExistPuyo(row - 1, col);
            }
        }
        return true;
    }
    public bool ExistPuyo(float row, float col)
    {
        if (row >= Util.row || row < 0 || col >= Util.col||col<0) return true;
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
        if (key == -1)
        {
            switch (mNext[0].MState)
            {
                case DirState.Right:
                    return !ExistPuyo(row, col - 1);
                case DirState.Up:
                    return !ExistPuyo(row, col - 1);
                case DirState.Down:
                    return !ExistPuyo(row - 1, col - 1);
                case DirState.Left:
                    return  !ExistPuyo(row, col - 2);
            }
        }
        else
        {
            switch (mNext[0].MState)
            {
                case DirState.Right:
                    return  !ExistPuyo(row, col + 2);
                case DirState.Up:
                    return !ExistPuyo(row, col + 1);
                case DirState.Down:
                    return !ExistPuyo(row - 1, col + 1);
                case DirState.Left:
                    return !ExistPuyo(row, col + 1);
            }
        }
        return false;
    }


}
