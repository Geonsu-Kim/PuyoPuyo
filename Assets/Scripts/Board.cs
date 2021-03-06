using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Puyo[,] mPuyoes; public Puyo[,] MPuyoes { get { return mPuyoes; } }

    private PuyoTsumo mNext1; 

    private List<PuyoTsumo> PuyoDropSet; 

    public Board()
    {
        mPuyoes = new Puyo[ConstantValues.row, ConstantValues.col];
        PuyoDropSet = new List<PuyoTsumo>();

    }
}
