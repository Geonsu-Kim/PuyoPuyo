using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoObj : MonoBehaviour
{
    private Puyo mPuyo; public Puyo MPuyo { set { mPuyo = value; } get { return mPuyo; } }
    private SpriteRenderer mSprite;
    public PuyoColorConfig config;
    private void Awake()
    {
        mSprite = GetComponent<SpriteRenderer>();
        mPuyo = new Puyo(PuyoColor.NA);
    }
    public void SetColor(PuyoColor color)
    {
        mPuyo.SetPuyoColor(color);
        UpdateSprite();
    }
    void UpdateSprite()
    {
        mSprite.sprite = config.sprites[(int)mPuyo.MColor].sprites[0];
    }
}
