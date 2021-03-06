using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private bool mInit = false;
    private Board board;
    void Start()
    {
    }
    void Init()
    {
        if (mInit) return;
        mInit = true;
        board = new Board();
    }
}
