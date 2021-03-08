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
    public void SetPuyo(PuyoColor color1, PuyoColor color2)
    {
        mAxis = CallPuyoObj();
        mAround = CallPuyoObj();
        SetColor(color1, color2);
        mAxis.transform.SetParent(transform);
        mAround.transform.SetParent(transform);
        mAxis.transform.position=new Vector3(transform.position.x, transform.position.y);
        mAround.transform.position = new Vector3(transform.position.x, transform.position.y+1);
    }
    public void DetachPuyo()
    {
        mAxis.transform.SetParent(null);
        mAround.transform.SetParent(null);
    }
    PuyoObj CallPuyoObj()
    {
        GameObject obj = PuyoPoolManager.Instance.GetPuyo();
        PuyoObj puyo = obj.GetComponent<PuyoObj>();
        obj.SetActive(true);
        return puyo;
    }
    void SetColor(PuyoColor color1,PuyoColor color2)
    {
        mAxis.SetColor(color1);
        mAround.SetColor(color2);
    }
    public void Move(Vector3 pos)
    {
        transform.position = pos;
    }

}
