using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoObj : MonoBehaviour
{
    private const string paramColor = "Color";
    private const string paramStop = "Stop";
    private const string paramStart = "paramStart";
    private const string paramFlashing = "Flashing";
    private const string paramGlow = "Glow";
    private const string paramWidth = "_ShineWidth";
    private Puyo mPuyo; public Puyo MPuyo { set { mPuyo = value; } get { return mPuyo; } }
    private Vector3 prevDest;
    private SpriteRenderer mSprite;
    // private Animator mAnimator;
    public PuyoColorConfig config;
    public bool isDropping { get; set; }
    public bool isRotating { get; set; }
    public bool isPopping { get; set; }

    IEnumerator prevRotate;

    private void Awake()
    {
        mSprite = GetComponent<SpriteRenderer>();
        mPuyo = new Puyo(PuyoColor.NA, this);
        prevDest = this.transform.localPosition;
    }
    private void OnEnable()
    {
        mSprite.material.SetFloat("_ShineWidth", 0f);
    }
    public void SetColor(PuyoColor color)
    {
        mPuyo.SetPuyoColor(color);
        UpdateSprite(0);
    }
    public PuyoColor GetColor()
    {
        return mPuyo.MColor;
    }
    public void SetFlashing(bool b)
    {
        if (b)
        {
            StartCoroutine(paramFlashing);
        }
        else
        {
            StopCoroutine(paramFlashing);
        }
    }
    public void SetGlow(bool b)
    {
        if (b)
        {
            StartCoroutine(paramGlow);
        }
        else
        {
            StopCoroutine(paramGlow);
            mSprite.material.SetFloat(paramWidth, 0f);
        }
    }
    public void StartPoppoing(AudioClip basicSFX, AudioClip characterSpell)
    {
        StartCoroutine(Popping(basicSFX, characterSpell));
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
    public void ArrangeDrop(float distance,AudioClip clip,float duration = 1.0f)
    {
        StartCoroutine(MoveTo(distance, duration,clip));
    }
    public void StartRotate(int key, bool quickTurn)
    {
        if (prevRotate != null&&isRotating)
        {
            StopCoroutine(prevRotate);
        }
        prevRotate = RotateTo(key, quickTurn);
        StartCoroutine(prevRotate);
    }

    public void CallParticle()
    {
        GameObject particle = PuyoPoolManager.Instance.GetParticle();
        if (particle == null) return;
        PuyoParticle puyoParticle = particle.GetComponent<PuyoParticle>();
        particle.transform.position = transform.position;
        puyoParticle.SetColor(mPuyo.MColor);
        particle.SetActive(true);


    }
    private IEnumerator MoveTo(float distance, float duration, AudioClip clip)
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
        time = 0f;
        SoundManager.Instance.PlaySFX(clip);
        while (time < 4f)
        {
            if (time < 2f)
            {
                yield return YieldInstructionCache.WaitForSeconds(0.05f);
                UpdateSprite(20);
                yield return YieldInstructionCache.WaitForSeconds(0.05f);
                UpdateSprite(0);
            }
            else
            {
                yield return YieldInstructionCache.WaitForSeconds(0.05f);
                UpdateSprite(21);
                yield return YieldInstructionCache.WaitForSeconds(0.05f);
                UpdateSprite(0);
            }
            time++;
        }
        yield return YieldInstructionCache.WaitForSeconds(0.05f);
        isDropping = false;

    }
    private IEnumerator RotateTo(int key, bool quickTurn)
    {
        float destAngle = quickTurn ? key * 180f : key * 90f;
        destAngle *= Mathf.PI / 180;
        Vector3 start = transform.localPosition;
        transform.localPosition = prevDest;
        prevDest = GetRotatePos(start, destAngle);
        float time = 0f; 
        isRotating = true;
        while (time < 0.125f)
        {
            time += Time.smoothDeltaTime * Time.timeScale;
            float angle = Mathf.Lerp(0, destAngle, time / 0.125f);

            transform.localPosition = GetRotatePos(start, angle);
            yield return null;
        }
        isRotating = false;
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
    private IEnumerator Popping(AudioClip basicSFX, AudioClip characterSpell)
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
        SoundManager.Instance.PlaySFX(basicSFX); SoundManager.Instance.PlaySFX(characterSpell);
        yield return YieldInstructionCache.WaitForSeconds(0.3f);
        CallParticle();
        UpdateSprite(18);
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        UpdateSprite(19);

        yield return YieldInstructionCache.WaitForSeconds(0.1f);

        isPopping = false;

    }
    public IEnumerator Glow()
    {
        float t = 0f;
        while (true)
        {
            do
            {
                yield return null;
                t += Time.deltaTime;
                mSprite.material.SetFloat(paramWidth, Mathf.Lerp(0f, 1f, t*4f));
            } while (t<=0.25f);
            t = 0f;
            do
            {
                yield return null;
                t += Time.deltaTime;
                mSprite.material.SetFloat(paramWidth, Mathf.Lerp(1f, 0.3f, t* 4f));

            } while (t <= 0.25f);
            t = 0f;

        }
    }
    Vector3 GetRotatePos(Vector3 start,float angle)
    {
        float x = start.x * Mathf.Cos(angle) - start.y * Mathf.Sin(angle);
        float y = start.x * Mathf.Sin(angle) + start.y * Mathf.Cos(angle);
        Vector3 ret=new Vector3(x,y);
        return ret;
    }
}
