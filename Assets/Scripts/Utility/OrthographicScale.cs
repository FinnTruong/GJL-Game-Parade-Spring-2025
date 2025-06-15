using UnityEngine;

public class OrthographicScale : MonoBehaviour
{

    private float startOrthographicSize;
    private float startScale;

    private float scalingFactor => startScale / startOrthographicSize;
    private void Start()
    {
        startOrthographicSize = Camera.main.orthographicSize;
        startScale = transform.localScale.x;
    }

    void Update()
    {
        transform.localScale = Vector3.one * Camera.main.orthographicSize * scalingFactor;
    }
}
