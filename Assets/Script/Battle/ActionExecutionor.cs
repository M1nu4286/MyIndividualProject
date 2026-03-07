using UnityEngine;

public class ActionExecutionor
{
    public void BattleLog(int[] playerOrder, int[] enemyOrder, RuntimeEntity[] player, RuntimeEntity[] enemy)
    {
        int totalCount = playerOrder.Length + enemyOrder.Length;
        for (int i = 0; i < totalCount; i++)
        {
            if (i < playerOrder.Length)
            {
                for (int j = 0; j < player.Length; j++)
                {
                    if (player[j].index == playerOrder[i])
                    {
                        Debug.Log($"Player {player[j].BaseData.EntityID} takes action.");
                        break;
                    }
                }
            }
            if( i < enemyOrder.Length)
            {
                for (int j = 0; j < player.Length; j++)
                {
                    if (enemy[j].index == enemyOrder[i])
                    {
                        Debug.Log($"Player {enemy[j].BasseData.EntityID} takes action.");
                        break;
                    }
                }
            }
        }
    }
}
