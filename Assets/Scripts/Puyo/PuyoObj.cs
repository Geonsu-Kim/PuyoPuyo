using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoObj : MonoBehaviour
{
    private Puyo mPuyo; public Puyo MPuyo { set { mPuyo = value; } get { return mPuyo; } }
    private SpriteRenderer mSprite;
    public PuyoColorConfig config;
    public bool isDropping { get; set; }
    private void Awake()
    {
        mSprite = GetComponent<SpriteRenderer>();
        mPuyo = new Puyo(PuyoColor.NA,this);
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
    public void ArrangeDrop(float distance,float duration=1.0f)
    {
        StartCoroutine(MoveTo(distance, duration));
    }
    private IEnumerator MoveTo(float distance, float duration)
    {
        isDropping = true;
        Vector3 start = transform.position;
        Vector3 to = new Vector3(transform.position.x, transform.position.y - distance);
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime * Time.timeScale;
            transform.position = Vector2.Lerp(start, to, time / duration);
            yield return null;

        }
        isDropping = false;

    }
}
