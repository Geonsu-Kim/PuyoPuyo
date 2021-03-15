using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public struct PosPair
{
    public int x, y;
}
public enum SpecialRotate
{
    Na = -1, PushUp, PushRight, PushLeft
}
public class Board
{
    private Transform mParent;
    private BoardRule rule;
    private Puyo[,] mPuyos; public Puyo[,] MPuyos { get { return mPuyos; } }
    private PuyoTsumoObj[] mNext; public PuyoTsumoObj MCurTsumo { get { return mNext[0]; } }
    private Queue<PuyoColor> mDropSet; public Queue<PuyoColor> MPuyoDropSet { get { return mDropSet; } }

    private SortedSet<int> emptyPos;
    private List<PuyoObj> MovingPuyo;
    private List<PosPair> MatchedList;
    private List<PosPair> PopList;
    public Board(Transform parent)
    {
        mParent = parent;
        rule = new BoardRule();
        mNext = new PuyoTsumoObj[4];
        mPuyos = new Puyo[Util.row, Util.col];
        mDropSet = new Queue<PuyoColor>();
        emptyPos = new SortedSet<int>();
        MovingPuyo = new List<PuyoObj>();
        MatchedList = new List<PosPair>();
        PopList = new List<PosPair>();
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
    public IEnumerator DropPuyo()
    {
        mNext[0].StartFlashing();
        while (!MustStop())
        {
            mNext[0].Drop();
            yield return YieldInstructionCache.WaitForSeconds(0.33f);
        } 
        PutInBoard(mNext[0].MAround, mNext[0].MAXis);

        mNext[0].DetachPuyo();
        ChangeOrder();
        yield return WaitForDropping();
        MovingPuyo.Clear();
    }
    private IEnumerator WaitForDropping()
    {
        bool bContinue = false;
        do
        {
            bContinue = false;
            for (int i = 0; i < MovingPuyo.Count; i++)
            {
                if (MovingPuyo[i].isDropping)
                {
                    bContinue = true;
                    break;
                }
            }
            yield return YieldInstructionCache.WaitForSeconds(0.05f);
        } while (bContinue);
    }
    public IEnumerator AfterDrop(Returnable<bool> CheckAgain)
    {
        yield return EvalutateBoard();
        yield return PopMatchedPuyo(CheckAgain);
        yield return WaitForDropping();
        MovingPuyo.Clear();
        //print();
    }
    public IEnumerator EvalutateBoard()
    {
        Puyo puyo = null;
        for (int i = 0; i < Util.row; i++)
        {
            for (int j = 0; j < Util.col; j++)
            {
                MatchedList.Clear();
                puyo = mPuyos[i, j];
                if (puyo==null||puyo.MVisited) continue;
                puyo.MVisited = true;
                MatchedList.Add(new PosPair() {x= j,y=i});
                CheckAdj(puyo, i, j);
                if (MatchedList.Count >= 4)
                {
                    PopList.AddRange(MatchedList);
                }
            }
        }
        UpdateState();
        yield break;
    }
    public IEnumerator PopMatchedPuyo(Returnable<bool> CheckAgain)
    {
        if (PopList.Count == 0)
        {
            CheckAgain.value = false;
            yield break;
        }

        CheckAgain.value = true;
        for (int i = 0; i < PopList.Count; i++)
        {
            mPuyos[PopList[i].y, PopList[i].x].SetActiveFalse();
            mPuyos[PopList[i].y, PopList[i].x] = null;
        }
        PopList.Clear();
        for (int i = 0; i < Util.col; i++)
        {
            ArrangePuyo(i);
        }
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
    void UpdateState()//방문 체크,인접 이미지 갱신
    {
        for (int i = 0; i < Util.row; i++)
        {
            for (int j = 0; j < Util.col; j++)
            {
              
                if (mPuyos[i,j] == null) continue;
                mPuyos[i,j].MVisited = false;
                mPuyos[i, j].UpdateSprite();
            }
        }
    }
    void ArrangePuyo(int col)
    {
        emptyPos.Clear();
        for (int i = 0; i < Util.row; i++)
        {
            if (mPuyos[i, col] == null)
            {
                emptyPos.Add(i);
            }
        }
        if (emptyPos.Count == 0) return;
        int firstValue = emptyPos.Min;
        Puyo puyo = null;
        for (int i = emptyPos.Min + 1; i < Util.row; i++)
        {
            puyo = mPuyos[i, col];
            if (puyo == null) continue;
            puyo.ArrangeDrop(i - firstValue, (i - firstValue) * 0.1f);
            MovingPuyo.Add(puyo.MObj);
            mPuyos[firstValue, col] = puyo;
            mPuyos[i, col] = null;
            emptyPos.Remove(firstValue);
            emptyPos.Add(i);
            firstValue = emptyPos.Min;
        }
    }

    void PutInBoard(PuyoObj obj1, PuyoObj obj2)
    {
        int obj1Row = (int)obj1.transform.position.y;
        int obj1Col = (int)obj1.transform.position.x;
        int obj2Row = (int)obj2.transform.position.y;
        int obj2Col = (int)obj2.transform.position.x;

        mPuyos[obj1Row, obj1Col] = obj1.MPuyo;
        mPuyos[obj2Row, obj2Col] = obj2.MPuyo;
        if (!ExistPuyo(obj1Row - 1, obj1Col))
        {
            ArrangePuyo(obj1Col);
        }
        if (!ExistPuyo(obj2Row - 1, obj2Col))
        {
            ArrangePuyo(obj2Col);
        }
    }
    void ChangeOrder()
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
    void CheckAdj(Puyo puyo, int row, int col)
    {
        for (int i = 0; i < Util.Dir.Length; i++)
        {
            int nx = col + Util.Dir[i].x;
            int ny = row + Util.Dir[i].y;
            if (ny >= Util.row || ny < 0 || nx >= Util.col || nx < 0) continue;
            if (mPuyos[ny,nx]==null || mPuyos[ny, nx].MVisited) continue;
            if (puyo.MColor == mPuyos[ny, nx].MColor)
            {
                puyo.CalcAdj(i,mPuyos[ny,nx]); 
                mPuyos[ny, nx].CalcAdj(i > 1 ? i - 2 : i + 2,puyo);
                mPuyos[ny, nx].MVisited = true;
                MatchedList.Add(new PosPair() {x=nx,y=ny });
                CheckAdj(mPuyos[ny, nx], ny, nx);
            }

        }
    }
    public bool ExistPuyo(float row, float col)
    {
        if (row >= Util.row || row < 0 || col >= Util.col || col < 0) return true;
        if (mPuyos[(int)row, (int)col] == null)
            return false;
        return true;
    }
    public bool IsBetweenWalls()
    {
        float row = mNext[0].ConvertRow();
        float col = mNext[0].ConvertCol();
        if (ExistPuyo(row, col - 1) && ExistPuyo(row, col + 1)) return true;
        return false;
    }
    public bool IsGameEnd()
    {
        return mPuyos[11, 2] != null;
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
                    return !ExistPuyo(row, col - 2);
            }
        }
        else
        {
            switch (mNext[0].MState)
            {
                case DirState.Right:
                    return !ExistPuyo(row, col + 2);
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
    public bool CheckMoving()
    {
        for (int i = 0; i < MovingPuyo.Count; i++)
        {
            if (MovingPuyo[i].isDropping)
            {
                return true;
            }
        }
        return false;
    }
    void print()
    {
        StringBuilder sb = new StringBuilder(200);
        for (int i = 12; i >= 0; i--)
        {
            for (int j = 0; j < 6; j++)
            {
                if (mPuyos[i, j] == null)
                    sb.Append("0 ");
                else sb.Append("1 ");
            }
            sb.Append("\n");
        }
        Debug.Log(sb.ToString());
    }

}
