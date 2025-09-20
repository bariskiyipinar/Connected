using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform defaultTarget;

    private Transform target;

    [Header("Y Sınırları")]
    public float minY = -1f;
    public float maxY = 2f;

    [Header("Takip Hızı")]
    public float lerpSpeed = 10f; // Daha canlı takip için artırdım

    void Start()
    {
        target = defaultTarget;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = new Vector3(
            Mathf.Lerp(transform.position.x, target.position.x, lerpSpeed * Time.deltaTime),
            Mathf.Clamp(Mathf.Lerp(transform.position.y, target.position.y, lerpSpeed * Time.deltaTime), minY, maxY),
            transform.position.z
        );

        transform.position = targetPos;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ResetToDefaultTarget()
    {
        target = defaultTarget;
    }
}
