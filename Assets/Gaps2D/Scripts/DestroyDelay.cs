using System.Collections;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{
    public float destroyDelay = .5f;

    // destroy te game object after time
    void Start()
    {
        StartCoroutine(DestroyObject(destroyDelay));
    }

    IEnumerator DestroyObject(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(gameObject);
    }
}
