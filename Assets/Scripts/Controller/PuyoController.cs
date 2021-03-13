using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoController
{
    private Board mBoard;
    private Transform mParent;
    private MonoBehaviour mMono;
    
    public PuyoTsumoObj curTsumo { get { return mBoard.MCurTsumo; } }
    public Puyo[,] puyos { get { return mBoard.MPuyos; } }

    Returnable<bool> checkAgain;
    public PuyoController(Board board, Transform parent)
    {
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
        SpecialRotate(key);
        curTsumo.Rotate(key);
    }
    public void SpecialRotate(int key)
    {
        Vector3 rotatedPos = curTsumo.PredictRotatedPos(key);
        if (!mBoard.ExistPuyo(rotatedPos.y, rotatedPos.x)) return;
        float offset = 0;
        if (curTsumo.transform.position.y - (int)curTsumo.transform.position.y > 0) offset = 0.5f;
        curTsumo.Move(curTsumo.transform.position-rotatedPos-new Vector3(0,offset));
    }
    public void Move(int key)
    {
        if(!mBoard.CheckMovable(key))return;
        curTsumo.Move(new Vector3(key, 0));
    }
}
