using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoTsumo
{
    private Puyo mP1;
    private Puyo mP2;
    private PuyoTsumoObj mObj; 
    public PuyoTsumoObj MObj
    {
        get { return mObj; }
        set
        {
            mObj = value;
            mObj.MTsumo = this;
        }
    }
    public PuyoTsumo(Puyo p1,Puyo p2)
    {
        mP1 = p1;
        mP2 = p2;
    }
}
