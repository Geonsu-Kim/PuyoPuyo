using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoTsumoObj:MonoBehaviour
{
    private PuyoObj mAround;
    private PuyoObj mAxis;

    private void Start()
    {
        
    }
    private void SetPuyo(PuyoObj axis, PuyoObj around)
    {
        mAxis = axis;
        mAround = around;
        mAxis.transform.SetParent(transform);
        mAround.transform.SetParent(transform);
        mAxis.transform.position=new Vector3(transform.position.x, transform.position.y);
        mAround.transform.position = new Vector3(transform.position.x, transform.position.y+1);
    }
    private void DetachPuyo()
    {
        mAxis.transform.SetParent(null);
        mAround.transform.SetParent(null);
    }
    public void Move(Vector3 pos)
    {
        transform.position = pos;
    }
}
