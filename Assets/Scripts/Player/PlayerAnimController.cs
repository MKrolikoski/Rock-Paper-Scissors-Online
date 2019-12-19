using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public Animator handAnimator;
    public Animator armAnimator;

    private void Awake()
    {
        if(handAnimator == null)
        {
            Debug.LogError("[PlayerAnimController] Hand Animator not set");
        }
        if (armAnimator == null)
        {
            Debug.LogError("[PlayerAnimController] Arm Animator not set");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
