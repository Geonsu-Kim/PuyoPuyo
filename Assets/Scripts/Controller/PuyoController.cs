using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoController
{
    private Board mBoard;
    private Transform mParent;
    private MonoBehaviour mMono;
    
    public PuyoTsumoObj curTsumo { get { return mBoard.MCurTsumo; } }
    public Puyo[,] puyos { get { return mBoard.MPuyoes; } }

    Returnable<bool> gameEnd;
    Returnable<bool> dropped;
    public PuyoController(Board board, Transform parent)
    {
        mBoard = board;
        mParent = parent;
        mMono = mParent.GetComponent<MonoBehaviour>();
        gameEnd = new Returnable<bool>(false);
        dropped = new Returnable<bool>(false);
    }
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return mMono.StartCoroutine(routine);
    }
    public IEnumerator Action()
    {
        do
        {
            dropped.value = false;
            do
            {
                yield return mBoard.DropPuyo(dropped, gameEnd);
            } while (!dropped.value);

        } while (!gameEnd.value);
    }
    public void Rotate(int key)
    {
        curTsumo.Rotate(key);
    }
    public void Move(int key)
    {
        if(!mBoard.CheckMovable(key))return;
        curTsumo.Move(new Vector3(key, 0));
    }
}
