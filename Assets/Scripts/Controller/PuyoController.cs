using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoController
{
    private int quickTurnCnt;
    private Board mBoard;
    private Transform mParent;
    private MonoBehaviour mMono;
    
    public PuyoTsumoObj curTsumo { get { return mBoard.MCurTsumo; } }
    public Puyo[,] puyos { get { return mBoard.MPuyos; } }

    private PuyoNavi axis;
    private PuyoNavi around;


    Returnable<bool> checkAgain;
    public PuyoController(Board board, Transform parent)
    {
        quickTurnCnt = 0;
        mBoard = board;
        mParent = parent;
        mMono = mParent.GetComponent<MonoBehaviour>();
        checkAgain = new Returnable<bool>(true);
    }
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return mMono.StartCoroutine(routine);
    }
    public IEnumerator Action()
    {
        do
        {
            checkAgain.value = false;
            SetNavi();
            yield return mBoard.DropPuyo();
            OffNavi();
            do
            {
                yield return mBoard.AfterDrop(checkAgain);
            } while (checkAgain.value);

        } while (!mBoard.IsGameEnd());
    }
    public void Rotate(int key)
    {
        bool quickTurn = false;
        SpecialRotate(key,out quickTurn);
        if (quickTurn && quickTurnCnt == 1) return;
        curTsumo.Rotate(key, quickTurn, quickTurnCnt);
        SetNaviPos();
    }
    public void SpecialRotate(int key,out bool quickTurn)
    {
        Vector3 rotatedPos;
        float offset = 0;
        quickTurn = false;
        if (mBoard.IsBetweenWalls())
        {
            if (quickTurnCnt == 0)
            {
                quickTurn = true;
                quickTurnCnt++;
                return;
            }
            else
            {
                quickTurn = true;

                quickTurnCnt = 0;
                rotatedPos = curTsumo.PredictRotatedPos(key, true);
                if (curTsumo.transform.position.y - (int)curTsumo.transform.position.y > 0) offset = 0.5f;
                curTsumo.Move(curTsumo.transform.position - rotatedPos - new Vector3(0, offset));
                return;

            }
        }

         rotatedPos = curTsumo.PredictRotatedPos(key);
        if (!mBoard.ExistPuyo(rotatedPos.y, rotatedPos.x)) return;
        if (curTsumo.transform.position.y - (int)curTsumo.transform.position.y > 0) offset = 0.5f;
        curTsumo.Move(curTsumo.transform.position-rotatedPos-new Vector3(0,offset));
    }
    public void Move(int key)
    {
        if(!mBoard.CheckMovable(key))return;

        if (key == 0)
        {
            curTsumo.Move(new Vector3(0, -1)); return;
        }
        curTsumo.Move(new Vector3(key, 0));
        SetNaviPos();
    }
    void SetNaviPos()
    {
        float col = curTsumo.ConvertCol();
        float row = mBoard.GetNaviPos(col);
        switch (curTsumo.MState)
        {
            case DirState.Left:
                axis.SetPos(col, row);
                around.SetPos(col - 1, row);
                break;
            case DirState.Up:

                axis.SetPos(col, row);
                around.SetPos(col, row+1);
                break;
            case DirState.Right:
                axis.SetPos(col, row);
                around.SetPos(col+1, row);
                break;
            case DirState.Down:
                axis.SetPos(col, row+1);
                around.SetPos(col, row);
                break;
        }

    }


    void SetNavi()
    {
        axis = PuyoPoolManager.Instance.GetNavi();
        axis.SetActive(true);
        around = PuyoPoolManager.Instance.GetNavi();
        around.SetActive(true);
        axis.SetColor(curTsumo.MAXis.GetColor());
        around.SetColor(curTsumo.MAround.GetColor());
        SetNaviPos();
    }
    void OffNavi()
    {
        axis.SetActive(false);
        around.SetActive(false);
    }
}
