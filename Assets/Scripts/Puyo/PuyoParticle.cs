using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] p;
    [SerializeField] private PuyoSprites particles;
    private ParticleSystem.TextureSheetAnimationModule[] pm;
    private Color color;
    void Awake()
    {
        pm = new ParticleSystem.TextureSheetAnimationModule[2];
        pm[0] = p[0].textureSheetAnimation;
        pm[1] = p[1].textureSheetAnimation;
    }
    public void SetColor(PuyoColor c)
    {
        pm[0].SetSprite(0, particles.sprites[(int)c]);
        pm[1].SetSprite(0, particles.sprites[(int)c]);
        Invoke("Hide", 1f);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}

