using UnityEngine;

public class EnemyFloat : MonoBehaviour
{
    [SerializeField]
    float height = 0.1f;

    [SerializeField]
    float period = 1;

    public Vector3 initialPosition;
    private float offset;

    public bool isFloating;

    private void Awake()
    {
        offset = 1 - (Random.value * 2);
    }

    private void Update()
    {
        if (isFloating)
        {
            transform.position = initialPosition - Vector3.up * Mathf.Sin((Time.time + offset) * period) * height;
        }
    }
}
