using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PuyoFactory
{
    public static Puyo SpawnPuyo(PuyoColor color)
    {
        Puyo puyo = new Puyo(color);
        return puyo;
    }
}
