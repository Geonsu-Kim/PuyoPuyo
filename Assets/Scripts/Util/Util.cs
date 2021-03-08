using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public const float x = -9.5f;
    public const float y = -5.5f;
    public const int row = 13;
    public const int col = 6;

    public static readonly Vector3 startPos=new Vector3(-7.5f,6.5f);
    public static readonly Vector3 next1Pos = new Vector3(-2.5f, 4.5f);
    public static readonly Vector3 next2Pos = new Vector3(-1.5f, 2.5f);
    public static readonly Vector3 next3Pos = new Vector3(-0.5f, -0.5f);

    public static int bitCount(int x)
    {
        if (x == 1) return 1;
        return x % 2 + bitCount(x / 2);
    }
}
