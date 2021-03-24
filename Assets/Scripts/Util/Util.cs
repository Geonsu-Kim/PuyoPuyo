using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public const float x = -9.5f;
    public const float y = -5.5f;
    public const float INF = 256;
    public const int row = 13;
    public const int col = 6;
    public const int maxScore = 99999999;
    public static readonly Vector3 startPos = new Vector3(2, 12);
    public static readonly Vector3[] nextPos =
        { new Vector3(7f, 13f)
    ,new Vector3(7f, 10f)
    ,new Vector3(8f, 8f)
    ,new Vector3(8f, 5f)};
    public static readonly int[] ChainPower = { 1, 8, 16, 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448, 480, 512 };
    public static readonly Vector2Int[] Dir =
    {
        new Vector2Int(1,0),new Vector2Int(0,1),new Vector2Int(-1,0),new Vector2Int(0,-1)
    };
    public static int bitCount(int x)
    {
        if (x == 1) return 1;
        return x % 2 + bitCount(x / 2);
    }
}
