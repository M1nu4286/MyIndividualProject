using UnityEngine;

public static class HapCalculator 
{
    public static BattleResult CalculateHap(SkillData p1, SkillData p2)
    {
        int p1CoinCount = p1.CoinCount;
        int p2CoinCount = p2.CoinCount;
        int drawCount = 0;

        while (p1CoinCount > 0 && p2CoinCount > 0)
        {
            int p1Power = CalculatePower(p1, p1CoinCount);
            int p2Power = CalculatePower(p2, p2CoinCount);

            if (p1Power > p2Power)
            {
                p2CoinCount--;
            }
            else if (p2Power > p1Power)
            {
                p1CoinCount--;
            }
            else
            {
                Debug.Log("It's a draw for this round!");
                drawCount++;
            }
        }
        return new BattleResult(p1CoinCount, p2CoinCount);
    }

    private static int CalculatePower(SkillData skillData, int currentCoin) 
    {
        int coinFlipResult=0;

        for(int i = 0; i < currentCoin; i++) 
        {
            if(Random.value > 0.5f) 
                coinFlipResult++;
        }

        return skillData.BaseDamage + ( coinFlipResult * skillData.CoinValue);
    }
}
