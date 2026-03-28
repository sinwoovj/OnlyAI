using Unity.VisualScripting;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Transform startPos, endPos;
    public float speed = 2.0f;

    void Update()
    {
        float t = (Mathf.Cos(Time.time * speed) + 1f) * 0.5f;
        transform.position = Vector3.Lerp(startPos.position, endPos.position, t);
    }
}
