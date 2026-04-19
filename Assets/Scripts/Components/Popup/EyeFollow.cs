
using TMPro;
using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    private Vector3 startPosition;
    [Header("Restrictions")]
    public float maxX;
    public float maxY;

    [Header("Speed")]
    public float speed;

    [Header("Target")]
    public GameObject target;

    void Start()
    {
        startPosition = gameObject.transform.position;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        target = PopupSpawnManager.Instance.currentPopup;
        if (target != null && transform.position.y <= maxY + startPosition.y
            && transform.position.y >= startPosition.y - maxY
            && transform.position.x >= startPosition.x - maxX
            && transform.position.x <= startPosition.x + maxX)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, step);
        }
    }
}
