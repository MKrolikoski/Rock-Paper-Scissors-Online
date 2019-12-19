using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private RawImage[] healthImages;
    private int nextImageIndex = 2;

    private void Awake()
    {
        healthImages = GetComponentsInChildren<RawImage>();
        Array.Sort(healthImages, (x,y) => x.name.CompareTo(y.name));
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if(damage > healthImages.Length)
        {
            damage = healthImages.Length;
        }
        for (int i = damage; i > 0; --i)
        {
            ToggleImage(nextImageIndex, false);
        }
    }


    public void ResetHealth()
    {
        for (int i = 0; i < healthImages.Length; i++)
        {
            ToggleImage(i, true);
        }
        nextImageIndex = healthImages.Length - 1;
    }

    private void ToggleImage(int imageIndex, bool toggle)
    {
        if(toggle)
        {
            healthImages[imageIndex].color = Color.white;
            nextImageIndex++;
        }
        else
        {
            healthImages[imageIndex].color = Color.black;
            nextImageIndex--;
        }
        Mathf.Clamp(nextImageIndex, 0, healthImages.Length-1);
    }
}
