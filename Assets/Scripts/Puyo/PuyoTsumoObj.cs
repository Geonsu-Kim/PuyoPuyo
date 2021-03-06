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
    public bool isMoving { get; set; }
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
        mAxis.transform.localPosition = Vector3.zero;
        mAround.transform.localPosition = Vector3.up;
        
    }
    public PuyoColor GetAxisColor()
    {
        return mAxis.MPuyo.MColor;
    }
    public PuyoColor GetAroundColor()
    {
        return mAround.MPuyo.MColor;
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
    public void StartFlashing()
    {
        mAxis.SetFlashing(true);
    }
    public void SetPos(Vector3 pos,bool smooth=false)
    {
        if (!smooth)
        {
            transform.position = pos;
            return;
        }
        Vector3 start = transform.position;
        StartCoroutine(Moving(start, pos));
    }


    public void Move(Vector3 pos)
    {
        transform.position += pos;
    }
    private IEnumerator Moving(Vector3 start,Vector3 pos)
    {
        isMoving = true;
        float time = 0f;
        while (time < 0.33f)
        {
            transform.position = Vector3.Lerp(start, pos, time / 0.33f);
            time += Time.deltaTime * Time.timeScale;
            yield return null;
        }
        transform.position = pos;
        isMoving = false;
    }
    public void Rotate(int key, bool quickTurn, int quickTurnCnt)
    {
        mState = ChangeRotateState(key, quickTurn);
      mAround.StartRotate(key, quickTurn);
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
            return new Vector3(-mAround.DestPos.x, -mAround.DestPos.y);
        return key * new Vector3(-mAround.DestPos.y, mAround.DestPos.x);
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
        int _x = (int)transform.position.x;
        return (float)_x;//+ 2.5f;
    }
}
