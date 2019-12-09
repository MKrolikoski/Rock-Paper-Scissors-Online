using UnityEngine;
using UnityEngine.UI;


public class HealthUI : MonoBehaviour
{
    public GameObject healthDisplay;
    public Transform target;
    public bool active;

    Transform ui;
    public RawImage[] healthImages;
    Transform cam;

	void Start ()
    {

    }

    public void SetupHealthBar()
    {
        cam = Camera.main.transform;
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(healthDisplay, c.transform).transform;
                healthImages = ui.GetComponentsInChildren<RawImage>();
                break;
            }
        }
        //GetComponent<PlayerStats>().onDamageTaken += OnDamageTaken;
        activateHealthBar();
    }

    void LateUpdate ()
    {
        if(ui != null)
        {
            ui.position = target.position;
            ui.forward = -cam.forward;
        }
	}

    public void activateHealthBar()
    {
        ui.gameObject.SetActive(true);
    }

    public void deactivateHealthBar()
    {
        ui.gameObject.SetActive(false);
    }

    private void OnDamageTaken(int damage)
    {
    }
}
