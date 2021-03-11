using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DirState//회전 뿌요의 위치를 기준으로
{
    Left, Up, Right,Down
}
public class PuyoTsumoObj : MonoBehaviour
{
    private PuyoObj mAround; public PuyoObj MAround { get { return mAround; } }
    private PuyoObj mAxis; public PuyoObj MAXis { get { return mAxis; } }
    private DirState mState; public DirState MState {get{return mState; }}
    private void Start()
    {
        mState = DirState.Up;
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
        mAxis = null;
        mAround = null;
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
    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }
    public void Move(Vector3 pos)
    {
        transform.position += pos;
    }
    public void Rotate(int key)
    {
        int state = (int)mState - key;
        if (state == -1) state = 3;
        else if (state == 4) state = 0;
        mState = (DirState)state;
        Debug.Log(mState);
        mAround.transform.localPosition = key*new Vector3(-mAround.transform.localPosition.y, mAround.transform.localPosition.x);
    }
    public void Drop()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y-0.5f);
    }
    public float  ConvertRow()
    {
        return transform.position.y;//+ 5.5f;
    }
    public float ConvertCol()
    {
        return transform.position.x;//+ 2.5f;
    }
    
}
