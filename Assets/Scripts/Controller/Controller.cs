using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller
{
    private Board mBoard;
    private Transform mParent;
    private MonoBehaviour mMono;


    Returnable<bool> gameEnd;
    Returnable<bool> dropped;
    public Controller(Board board, Transform parent)
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

}
