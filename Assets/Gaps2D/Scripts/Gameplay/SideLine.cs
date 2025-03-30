using UnityEngine;

public class SideLine : MonoBehaviour
{
    public bool left;

    void Start()
    {
        Vector3 ScreenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        if (left)
            transform.position = new Vector3(-ScreenSize.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), 0, 0);
        else
            transform.position = new Vector3(ScreenSize.x - (GetComponent<SpriteRenderer>().bounds.size.x / 2), 0, 0);
    }

}
