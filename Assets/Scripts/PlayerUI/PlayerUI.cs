using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public HealthDisplay playerHealthDisplay;
    private Text textDisplay;
    private float textShowTime = 2.0f;
    private float timeSinceLastTextClear;

    private void Awake()
    {
        textDisplay = GetComponentInChildren<Text>();
        textDisplay.text = "";
        if (playerHealthDisplay == null)
        {
            Debug.LogError("[PlayerUI] Player health display not set.");
        }
    }

   
    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastTextClear = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timeSinceLastTextClear > textShowTime)
        {
            textDisplay.text = "";
        }
    }

    public void DisplayText(string text)
    {
        textDisplay.text = text;
        timeSinceLastTextClear = Time.time;
    }
}
