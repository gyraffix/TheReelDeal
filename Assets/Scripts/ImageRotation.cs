using UnityEngine;

public class ImageRotation : MonoBehaviour
{
    [Range(-45f, 0f)]
    [SerializeField] private float minRotation;
    [Range(0f, 45f)]
    [SerializeField] private float maxRotation;

    private void Awake()
    {
        float angle = Random.Range(minRotation, maxRotation);
        Vector3 rotation = transform.forward * angle;

        transform.localEulerAngles = rotation;
    }
}
