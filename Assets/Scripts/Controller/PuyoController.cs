using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoController
{
    private int quickTurnCnt;
    private Board mBoard;
    private Transform mParent;
    private MonoBehaviour mMono;

    public PuyoTsumoObj curTsumo { get { return mBoard.MCurTsumo; } }
    public Puyo[,] puyos { get { return mBoard.MPuyos; } }

    private PuyoNavi axis;
    private PuyoNavi around;
    private SoundAsset mBasicSFX;
    private SoundAsset mCharacterSpell;

    Returnable<bool> checkAgain;
    List<Puyo> axisMatched;
    List<Puyo> aroundMatched;
    public PuyoController(Board board, Transform parent, SoundAsset basicSFX,SoundAsset characterSpell)
    {
        quickTurnCnt = 0;
        mBoard = board;
        mParent = parent;
        mBasicSFX = basicSFX;
        mCharacterSpell = characterSpell;
        mMono = mParent.GetComponent<MonoBehaviour>();
        checkAgain = new Returnable<bool>(true);
        axisMatched = new List<Puyo>();
        aroundMatched = new List<Puyo>();
    }
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return mMono.StartCoroutine(routine);
    }
    public IEnumerator Action()
    {
        do
        {
            int chain = 0;
            checkAgain.value = false;
            SetNavi();
            yield return mBoard.DropPuyo();
            OffNavi(); 
            TurnOffGlow();
            do
            {
                yield return mBoard.AfterDrop(checkAgain,chain);
                chain++;
            } while (checkAgain.value);

            yield return mBoard.ChangeOrder();
        } while (!mBoard.IsGameEnd());
    }
    public void Rotate(int key)
    {
        bool quickTurn = false;
        SpecialRotate(key, out quickTurn);
        if (quickTurn && quickTurnCnt == 1) return;
        curTsumo.Rotate(key, quickTurn, quickTurnCnt);
        SetNaviPos();
        SoundManager.Instance.PlaySFX(mBasicSFX.clips[9]);
    }
    public void SpecialRotate(int key, out bool quickTurn)
    {
        Vector3 rotatedPos;
        float offset = 0;
        quickTurn = false;
        if (mBoard.IsBetweenWalls())
        {
            if (quickTurnCnt == 0)
            {
                quickTurn = true;
                quickTurnCnt++;
                return;
            }
            else
            {
                quickTurn = true;

                quickTurnCnt = 0;
                rotatedPos = curTsumo.PredictRotatedPos(key, true);
                if (curTsumo.transform.position.y - (int)curTsumo.transform.position.y > 0) offset = 0.5f;
                curTsumo.Move(curTsumo.transform.position - rotatedPos - new Vector3(0, offset));
                return;

            }
        }

        rotatedPos = curTsumo.PredictRotatedPos(key);
        if (!mBoard.ExistPuyo(rotatedPos.y, rotatedPos.x)) return;
        if (curTsumo.transform.position.y - (int)curTsumo.transform.position.y > 0) offset = 0.5f;
        curTsumo.Move(curTsumo.transform.position - rotatedPos - new Vector3(0, offset));
    }
    public void Move(int key)
    {
        if (!mBoard.CheckMovable(key)) 
        {

            return;
        }

        if (key == 0)
        {
            curTsumo.Move(new Vector3(0, -0.5f)); return;
        }
        curTsumo.Move(new Vector3(key, 0));
        SetNaviPos();
        SoundManager.Instance.PlaySFX(mBasicSFX.clips[8]);
    }
    void SetNaviPos()
    {
        TurnOffGlow();
        float col = curTsumo.ConvertCol();
        float row = mBoard.GetNaviPos(col);
        float xAxis = 0f, yAxis = 0f, xAround = 0f, yAround = 0f;
        switch (curTsumo.MState)
        {
            case DirState.Left:
                xAxis = col; yAxis = row;
                xAround = col - 1; yAround = mBoard.GetNaviPos(col - 1);
                break;
            case DirState.Up:
                xAxis = col; yAxis = row;
                xAround = col; yAround = row + 1;
                break;
            case DirState.Right:

                xAxis = col; yAxis = row;
                xAround = col + 1; yAround = mBoard.GetNaviPos(col + 1);
                break;
            case DirState.Down:

                xAxis = col; yAxis = row + 1;
                xAround = col; yAround = row;
                break;
        }
        bool same = false;
        if ((xAxis == xAround || yAxis == yAround) && curTsumo.MAXis.GetColor() == curTsumo.MAround.GetColor())
        {
            same = true;
        }
        axis.SetPos(xAxis, yAxis);
        around.SetPos(xAround, yAround);
        mBoard.CheckExpected(curTsumo.MAXis.GetColor(), (int)yAxis, (int)xAxis, axisMatched);
        mBoard.CheckExpected(curTsumo.MAround.GetColor(), (int)yAround, (int)xAround, aroundMatched);
        if (same)
        {
            if (axisMatched.Count + aroundMatched.Count >= 2)
            {
                for (int i = 0; i < axisMatched.Count; i++)
                {
                    axisMatched[i].Glow(true);
                }
                for (int i = 0; i < aroundMatched.Count; i++)
                {
                    aroundMatched[i].Glow(true);
                }
            }
        }
        else
        {
            if (axisMatched.Count >= 3)
            {
                for (int i = 0; i < axisMatched.Count; i++)
                {
                    axisMatched[i].Glow(true);
                }
            }
            if (aroundMatched.Count >= 3)
            {
                for (int i = 0; i < aroundMatched.Count; i++)
                {
                    aroundMatched[i].Glow(true);
                }
            }
        }
    }
    void TurnOffGlow()
    {
        for (int i = 0; i < axisMatched.Count; i++)
        {
            axisMatched[i].Glow(false);
        }
        for (int i = 0; i < aroundMatched.Count; i++)
        {
            aroundMatched[i].Glow(false);
        }
        axisMatched.Clear();
        aroundMatched.Clear();
    }
    void SetNavi()
    {
        axis = PuyoPoolManager.Instance.GetNavi();
        axis.SetActive(true);
        around = PuyoPoolManager.Instance.GetNavi();
        around.SetActive(true);
        axis.SetColor(curTsumo.MAXis.GetColor());
        around.SetColor(curTsumo.MAround.GetColor());
        SetNaviPos();
    }
    void OffNavi()
    {
        axis.SetActive(false);
        around.SetActive(false);
    }
}
