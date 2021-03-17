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
    // private Animator mAnimator;
    public PuyoColorConfig config;
    public bool isDropping { get; set; }
    public bool isRotating { get; set; }
    public bool isPopping { get; set; }

    IEnumerator flashing;

    private void Awake()
    {
        mSprite = GetComponent<SpriteRenderer>();
        mPuyo = new Puyo(PuyoColor.NA, this);
        flashing = Flashing();

    }
    public void SetColor(PuyoColor color)
    {
        mPuyo.SetPuyoColor(color);
        UpdateSprite(0);
    }
    public void SetFlashing(bool b)
    {
        if (b)
        {
            StartCoroutine(flashing);
        }
        else
        {
            StopCoroutine(flashing);
            UpdateSprite(0);
        }
    }
    public void StartPoppoing()
    {
        StartCoroutine(Popping());
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
        if (num == -1)
        {
            mSprite.sprite = null;
            return;
        }
        mSprite.sprite = config.sprites[(int)mPuyo.MColor].sprites[num];
    }
    public void ArrangeDrop(float distance, float duration = 1.0f)
    {
        StartCoroutine(MoveTo(distance, duration));
    }
    public void StartRotate(int key, bool quickTurn)
    {
        StartCoroutine(RotateTo(key, quickTurn));
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
    private IEnumerator RotateTo(int key, bool quickTurn)
    {
        float destAngle = quickTurn ? key * 180f : key * 90f;
        destAngle *= Mathf.PI / 180;
        float time = 0f; float x, y;
        Vector3 start = transform.localPosition;
        Vector3 to = Vector3.zero;
        isRotating = true;
        while (time < 0.125f)
        {
            time += Time.smoothDeltaTime * Time.timeScale;
            float angle = Mathf.Lerp(0, destAngle, time / 0.125f);
            Debug.Log(angle);
            x = start.x * Mathf.Cos(angle) - start.y * Mathf.Sin(angle);
            y = start.x * Mathf.Sin(angle) + start.y * Mathf.Cos(angle);
            transform.localPosition = new Vector3(x, y);
            yield return null;
        }
        isRotating = false;
    }
    private IEnumerator Popping()
    {
        int cnt = 0;
        isPopping = true;
        while (cnt < 5)
        {
            cnt++;
            yield return YieldInstructionCache.WaitForSeconds(0.05f);
            UpdateSprite(-1);
            yield return YieldInstructionCache.WaitForSeconds(0.05f);
            UpdateSprite(mPuyo.GetAdj());
        }

        yield return null;
        UpdateSprite(17);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        isPopping = false;

    }
}
