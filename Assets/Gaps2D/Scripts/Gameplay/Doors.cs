using UnityEngine;

public class Doors : MonoBehaviour
{
    public SpriteRenderer part1, part2;
    public Animator doorAnimator;

    Transform cameraTransform;

    //init doors
    public void Init(Transform camera, Color color)
    {
        cameraTransform = camera;
        part1.color = color;
        part2.color = color;

        //if current score is more than 10 enable door animation
        doorAnimator.enabled = ScoreManager.Instance.currentScore > 10;
    }

    //destroy doors when reach position under the screen
    void Update()
    {
        if (cameraTransform.position.y - transform.position.y > 15f)
        {
            Destroy(transform.gameObject);
        }

    }
}
