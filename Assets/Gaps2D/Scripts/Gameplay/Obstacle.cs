using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public SpriteRenderer sprite;

    Transform cameraTransform;
    float rotation;
    Vector3 currentEulerAngles;

    //init obstacle
    public void Init(Transform camera, Color color, float rotationSpeed)
    {
        cameraTransform = camera;
        sprite.color = color;

        if (Random.Range(0,2) == 0)
            rotation = rotationSpeed;
        else
            rotation = -rotationSpeed;
    }

    //rotate obstacle, destroy rotating circle when reach position under the screen
    void Update()
    {
        currentEulerAngles += new Vector3(0, 0, 1) * Time.deltaTime * rotation;
        transform.eulerAngles = currentEulerAngles;

        if (cameraTransform.position.y - transform.position.y > 15f)
        {
            GameManager.Instance.NewObstacle();
            Destroy(transform.parent.gameObject);
        }

    }
}
