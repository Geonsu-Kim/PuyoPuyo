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
            yield return mBoard.DropPuyo();
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
        curTsumo.Move(new Vector3(key, 0));
    }
}
