using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    // Complete movement duration
    public float completeMoveDuration = 1f;

    // Distance from point A to B of a complete path
    [HideInInspector]
    public float completePathLength;

    // True if coin is being transolacted, false otherwise
    private bool isMoving;

    // Movement duration
    private float moveDuration;

    // Movement timer
    private float moveTimer = 0;

    // Starting position
    private Vector3 startPosition;

    // Destination
    private Vector3 endPosition;


    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//


    void Start()
    {
        
    }

    void Update()
    {
        if (isMoving)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer > moveDuration)
            {
                StopMoving();
            }
            else
            {
                float ratio = moveTimer / moveDuration;
                transform.position = Vector3.Lerp(startPosition, endPosition, ratio);
            }
        }
    }


    // Sets start and end positions, calculates time to move and initiates movement
    public void StartMoving(Vector3 startPosition, Vector3 endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;

        float currentPathLength = Vector3.Distance(startPosition.normalized, endPosition.normalized);
        if (currentPathLength != completePathLength)
        {
            float distanceRatio = completePathLength / currentPathLength;
            moveDuration = completeMoveDuration / distanceRatio;
        }
        else
        {
            moveDuration = completeMoveDuration;
        }
        isMoving = true;
        moveTimer = 0;
    }

    // Stops moving
    public void StopMoving()
    {
        isMoving = false;
    }
}
