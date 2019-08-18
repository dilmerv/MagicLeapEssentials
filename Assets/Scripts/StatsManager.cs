using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    [SerializeField]
    private Text bulletsUsedText;

    [SerializeField]
    private Text hitItemsText;

    private int bulletsUsedCount = 0;
    
    private int hitItemsCount = 0;

    private static StatsManager _instance;
    
    public static StatsManager Instance 
    {
        get 
        {
            if(_instance == null)
            { 
                GameObject go = GameObject.Find("StatsManager");
                if(go == null)
                {
                    go = new GameObject("StatsManager");
                    _instance = go.AddComponent<StatsManager>();
                }
                else 
                {
                    _instance = go.GetComponent<StatsManager>();
                }
            }
            return _instance;
        }
    }

    public void IncrementBulletsUsed()
    {
        bulletsUsedCount++;
        bulletsUsedText.text = $"BULLETS USED: <color=blue>{bulletsUsedCount}</color>";
        Debug.Log(bulletsUsedCount);
    }

    public void IncrementItemsHit()
    {
        hitItemsCount++;
        hitItemsText.text = $"ITEMS HIT: <color=red>{hitItemsCount}</color>";
    }
}
