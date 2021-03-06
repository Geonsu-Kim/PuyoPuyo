using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoObj : MonoBehaviour
{
    private Puyo mPuyo; public Puyo MPuyo { set { mPuyo = value; } get { return mPuyo; } }
    private SpriteRenderer mSprite;
    private void Awake()
    {
        mSprite = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        SetSprite();
    }
    public void SetSprite()
    {

    }
    public void SetAdjType()
    {

    }
}
