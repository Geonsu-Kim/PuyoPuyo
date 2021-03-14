using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private bool mInit = false;
    private Board mBoard;
    private PuyoController mController;
    private RotateCommand mCommandRot;
    private MoveCommand mCommandMove;
    void Start()
    {
        Init();
    }
    void Init()
    {
        if (mInit) return;
        mInit = true;
        mBoard = new Board(transform);
        mController = new PuyoController(mBoard,transform);
        mCommandRot = new RotateCommand(mController);
        mCommandMove = new MoveCommand(mController);
        mBoard.ComposeGame();
        mController.StartCoroutine(mController.Action());
    }
    public void Rotate(int key)
    {
        mCommandRot.Execute(key);
    }
    public void Move(int key)
    {
        mCommandMove.Execute(key);
    }
    public bool CheckMoving()
    {
        return mBoard.CheckMoving();
    }
}
