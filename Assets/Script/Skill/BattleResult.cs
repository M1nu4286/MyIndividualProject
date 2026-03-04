using UnityEngine;

public struct BattleResult
{
    public readonly int RmaininigCoinA;
    public readonly int RmaininigCoinB;


    public BattleResult(int remainingCoinA, int remainingCoinB)
    {
        
        RmaininigCoinA = remainingCoinA;
        RmaininigCoinB = remainingCoinB;

        Debug.Log("Battle Result: Player A Remaining Coins: " + RmaininigCoinA + ", Player B Remaining Coins: " + RmaininigCoinB);
    }
}
