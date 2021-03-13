using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRule //색패보정
{
    private int[] mColorCount;
    private int firstTwo;//첫 2수의 색패
    public BoardRule()
    {
        mColorCount = new int[4] { 0, 0, 0, 0 };//Red Green Blue Yellow
        firstTwo = 1 << 4;
    }
    public int Rule1()//첫 2수는 1~3색
    {
        int color=0;
        if (Util.bitCount(firstTwo) < 4)
        {
            color = Random.Range(0, 4);
            firstTwo |= (1 << color);
        }
        else
        {
            int mustNot = 0;
;            for (int i = 0; i <=3 ; i++)
            {
                if ((firstTwo & (1 << i)) == 0)
                {
                    mustNot = i;
                    break;
                }
            }
            do
            {
                color = Random.Range(0, 4);
            } while (color==mustNot);
        }
        mColorCount[color]++;
        return color;
    }
    public int Rule2()//128수 뿌요의 각 색깔의 수는 64개
    {
        int color = 0;
        do
        {
            color = Random.Range(0, 4);
        } while (mColorCount[color]==64);
        mColorCount[color]++;
        return color;
    }
}
