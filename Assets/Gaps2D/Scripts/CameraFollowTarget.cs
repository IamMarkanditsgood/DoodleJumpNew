using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [Header("Object to follow")]
    public GameObject TargetYFollow;
    //public GameObject TargetXFollow;
    [Space()]
    public GameObject leftLine, rightLine;

    [Header("Follow speed")]
    [Range(0.0f, 50.0f)]
    public float speed = 6f;

    [Header("Camera Offset")]
    [Range(-10.0f, 10.0f)]
    public float yOffset;
    //[Range(-10.0f, 10.0f)]
    //public float xOffset;

    [Space(15)]
    public UIManager uIManager;

    float interpolation;
    Vector3 position;

    //camera follow the player, move sidelines with camera
    void LateUpdate()
    {
        if (uIManager.gameState == GameState.PLAYING)
        {
            interpolation = speed * Time.deltaTime;

            position = transform.position;

            if (TargetYFollow.transform.position.y + yOffset > transform.position.y)
                position.y = Mathf.Lerp(transform.position.y, TargetYFollow.transform.position.y + yOffset, interpolation);
            //position.x = Mathf.Lerp(transform.position.x, TargetXFollow.transform.position.x + xOffset, interpolation);

            transform.position = position;

            //move side lines with camera up
            leftLine.transform.position = new Vector2(leftLine.transform.position.x, transform.position.y);
            rightLine.transform.position = new Vector2(rightLine.transform.position.x, transform.position.y);
        }
        else if (uIManager.gameState == GameState.REVIVE)
        {
            interpolation = speed * Time.deltaTime;

            position = transform.position;


            position.y = Mathf.Lerp(transform.position.y, TargetYFollow.transform.position.y + yOffset, interpolation);
            //position.x = Mathf.Lerp(transform.position.x, TargetXFollow.transform.position.x + xOffset, interpolation);

            transform.position = position;

            //move side lines with camera up
            leftLine.transform.position = new Vector2(leftLine.transform.position.x, transform.position.y);
            rightLine.transform.position = new Vector2(rightLine.transform.position.x, transform.position.y);
        }
    }

    //reset camera position
    public void ResetCameraPosition()
    {
        transform.position = new Vector3(0, 0, -10);

        leftLine.transform.position = new Vector2(leftLine.transform.position.x, 0);
        rightLine.transform.position = new Vector2(rightLine.transform.position.x, 0);
    }
}