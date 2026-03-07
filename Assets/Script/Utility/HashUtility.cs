using UnityEngine;

public static class HashUtility
{
    public static int HashMachine(string str) 
    {
        unchecked 
        {
            int hash = 5381;

            foreach (char c in str)
            {
                hash = ((hash << 5) + hash) ^ c;
            }
            return hash;
        }
    }
}
