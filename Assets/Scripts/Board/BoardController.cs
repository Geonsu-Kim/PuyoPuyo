using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private bool mInit = false;
    private Board mBoard;
    private Controller controller;
    void Start()
    {
        Init();
    }
    void Init()
    {
        if (mInit) return;
        mInit = true;
        mBoard = new Board(transform);
        controller = new Controller(mBoard,transform);
        mBoard.ComposeGame();
        controller.StartCoroutine(controller.Action());
    }
}
