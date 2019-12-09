using UnityEngine;

public class EnemyHealthUI : MonoBehaviour
{
    public GameObject healthDisplayGO;
    public Transform healthDisplayRoot;
    [HideInInspector]
    public HealthDisplay healthDisplay;

    private Transform playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetupUI()
    {
        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            if (cam.name == "LocalPlayerCamera")
            {
                playerCamera = cam.transform;
                break;
            }
        }
        if (healthDisplayGO == null || healthDisplayRoot == null)
        {
            Debug.LogError("[EnemyHealthUI] HealthDisplay or HealthDisplayRoot not set in the inspector");
        }
        else if (playerCamera == null)
        {
            Debug.LogError("[EnemyHealthUI] playerCamera not set");
        }
        else
        {
            foreach (Canvas c in FindObjectsOfType<Canvas>())
            {
                if (c.renderMode == RenderMode.WorldSpace)
                {
                    healthDisplay = Instantiate(healthDisplayGO, c.transform).GetComponent<HealthDisplay>();
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healthDisplay != null)
        {
            healthDisplay.transform.position = transform.position;
            healthDisplay.transform.forward = -playerCamera.forward;
        }
    }
}
