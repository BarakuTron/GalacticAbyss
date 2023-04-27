using UnityEngine;
using UnityEngine.UI;

public class GemCounter : MonoBehaviour
{
    public static GemCounter Instance;

    public int gemCount = 0;
    public Text gemCounterText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGem(int amount)
    {
        gemCount += amount;
        UpdateGemCounterText();
    }

    private void UpdateGemCounterText()
    {
        gemCounterText.text = "Gems: " + gemCount;
    }
}
