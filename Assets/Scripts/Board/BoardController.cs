using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private bool mInit = false;
    private Board mBoard;
    private PuyoController mController;
    private RotateCommand mCommandRot;
    private MoveCommand mCommandMove;
    [SerializeField]private SoundAsset mBasicSFX;
    [SerializeField] private SoundAsset mCharacterSpell;
    void Start()
    {
        Init();
    }
    void Init()
    {
        if (mInit) return;
        mInit = true;
        mBoard = new Board(transform, mBasicSFX, mCharacterSpell);
        mController = new PuyoController(mBoard,transform, mBasicSFX, mCharacterSpell);
        mCommandRot = new RotateCommand(mController);
        mCommandMove = new MoveCommand(mController);
        mBoard.ComposeGame();
        mController.StartCoroutine(mController.Action());
    }
    public void Rotate(int key)
    {
        mCommandRot.Execute(key);
    }
    public void Move(int key)
    {
        mCommandMove.Execute(key);
    }
    public bool CanControl()
    {
        return mBoard.canControl;
    }
}
