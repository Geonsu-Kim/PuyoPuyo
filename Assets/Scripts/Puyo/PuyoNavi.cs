using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoNavi : MonoBehaviour
{
    [SerializeField] private PuyoSprites puyoSprites;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetColor(PuyoColor color)
    {
        Debug.Log(color);
        spriteRenderer.sprite = puyoSprites.sprites[(int)color];
    }
    public void SetPos(float x,float y)
    {
        transform.position = new Vector3(x, y);
    }
    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }
}
