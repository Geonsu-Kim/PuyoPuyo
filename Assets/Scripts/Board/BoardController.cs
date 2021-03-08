using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private bool mInit = false;
    private Board mBoard;
    void Start()
    {
        Init();
    }
    void Init()
    {
        if (mInit) return;
        mInit = true;
        mBoard = new Board(transform);
        mBoard.ComposeGame();
    }
}
