using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoPoolManager : SingletonBase<PuyoPoolManager>
{
    // Start is called before the first frame update
    [SerializeField] private GameObject puyoObj;
    [SerializeField] private GameObject puyoTsumoObj;
    [SerializeField] private GameObject puyoParticle;
    [SerializeField] private GameObject puyoNavi;




    private List<GameObject> PuyoPool;
    public List<GameObject> PuyoNaviPool;
    public List<GameObject> PuyoTsumoPool;
    public List<GameObject> puyoParticlePool;
    void Awake()
    {
        PuyoPool = new List<GameObject>();
        PuyoTsumoPool = new List<GameObject>();
        puyoParticlePool = new List<GameObject>();
        PuyoNaviPool = new List<GameObject>();
        CreateObject();
    }
    void CreateObject()
    {
        for (int i = 0; i < 200; i++)
        {
            GameObject newPuyo = Instantiate(puyoObj);
            newPuyo.SetActive(false);
            newPuyo.name = string.Copy(puyoObj.name);
            PuyoPool.Add(newPuyo);

            GameObject newParticle = Instantiate(puyoParticle);
            newParticle.SetActive(false);
            newParticle.name = string.Copy(puyoParticle.name);
            puyoParticlePool.Add(newParticle);

        }
        for (int i = 0; i < 4; i++)
        {
            GameObject newTsumo = Instantiate(puyoTsumoObj);
            newTsumo.name = string.Copy(puyoTsumoObj.name+i);
            PuyoTsumoPool.Add(newTsumo);

            GameObject newNavi = Instantiate(puyoNavi);
            puyoNavi.SetActive(false);
            newNavi.name = string.Copy(puyoNavi.name);
            PuyoNaviPool.Add(newNavi);
        }
    }
    void InstantiateNewPuyo()
    {
        GameObject newBlock = Instantiate(puyoObj);
        newBlock.SetActive(false);
        newBlock.name = string.Copy(puyoObj.name); ;
        PuyoPool.Add(newBlock);
    }
    public GameObject GetPuyo()
    {
        for (int i = 0; i < PuyoPool.Count; i++)
        {
            if (!PuyoPool[i].activeSelf)
            {
                return PuyoPool[i];
            }
            else
            {
                if (i == PuyoPool.Count - 1)
                {
                    InstantiateNewPuyo();
                    return PuyoPool[i + 1];
                }
            }
        }
        return null;
    }
    public GameObject GetParticle()
    {
        for (int i = 0; i < puyoParticlePool.Count; i++)
        {
            if (!puyoParticlePool[i].activeSelf)
            {
                return puyoParticlePool[i];
            }
        }
        return null;
    }
    public PuyoNavi GetNavi()
    {
        for (int i = 0; i < PuyoNaviPool.Count; i++)
        {
            if (!PuyoNaviPool[i].activeSelf)
            {
                return PuyoNaviPool[i].GetComponent<PuyoNavi>();
            }
        }
        return null;
    }
}
