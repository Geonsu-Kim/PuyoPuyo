using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoObj : MonoBehaviour
{
    private const string paramColor = "Color";
    private const string paramStop = "Stop";
    private const string paramStart = "paramStart";
    private Puyo mPuyo; public Puyo MPuyo { set { mPuyo = value; } get { return mPuyo; } }
    private SpriteRenderer mSprite;
    private IEnumerator flashing;
   // private Animator mAnimator;
    public PuyoColorConfig config;
    public bool isDropping { get; set; }
    private void Awake()
    {
        mSprite = GetComponent<SpriteRenderer>();
        mPuyo = new Puyo(PuyoColor.NA,this);
        flashing = Flashing();
    }
    public void SetColor(PuyoColor color)
    {
        mPuyo.SetPuyoColor(color);
        UpdateSprite(0);
    }
    public void StartFlahsing()
    {
        StartCoroutine(flashing);
        UpdateSprite(0);
    }
    public void StopFlashing()
    {
        StopCoroutine(flashing);
    }
    public IEnumerator Flashing()
    {
        do
        {
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            UpdateSprite(16);
               yield return YieldInstructionCache.WaitForSeconds(0.1f);
            UpdateSprite(0);
        } while (true);
    }
    public void UpdateSprite(int num)
    {
        mSprite.sprite = config.sprites[(int)mPuyo.MColor].sprites[num];
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
