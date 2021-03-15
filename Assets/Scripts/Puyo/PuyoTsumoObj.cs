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
        mState = DirState.Up;
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
        mAxis.StopFlashing();
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
    public void StartFlashing()
    {
        mAxis.StartFlahsing();
    }
    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }
    public void Move(Vector3 pos)
    {
        transform.position += pos;
    }
    public void Rotate(int key, bool quickTurn, int quickTurnCnt)
    {
        mState = ChangeRotateState(key, quickTurn);
        mAround.transform.localPosition = RealRotate(key, quickTurn);
    }
    public DirState ChangeRotateState(int key, bool quickTurn)//회전상태 변환
    {
        if (quickTurn)
        {
            if (mState == DirState.Down) return DirState.Up;
            else { return DirState.Down; }
        }

        int state = (int)mState - key;

        if (state == -1) return DirState.Down;
        else if (state == 4) return DirState.Left;
        return (DirState)state;
    }
    public Vector3 RealRotate(int key,bool quickTurn)//실제 회전적용
    {
        if(quickTurn)
            return new Vector3(-mAround.transform.localPosition.x, -mAround.transform.localPosition.y);
        return key * new Vector3(-mAround.transform.localPosition.y, mAround.transform.localPosition.x);
    }
    public Vector3 PredictRotatedPos(int key, bool quickTurn = false)
    {
        Vector3 rotatedPos = RealRotate(key, quickTurn);
        return transform.position + rotatedPos;
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
