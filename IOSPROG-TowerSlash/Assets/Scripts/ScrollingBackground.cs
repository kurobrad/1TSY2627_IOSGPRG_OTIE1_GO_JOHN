using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] public float speed = 0.05f;
    [SerializeField] private float dashSpeedMultiplier = 4f;
    [SerializeField] private Renderer bgRenderer;

    private Material _mat;

    private void Start()
    {
        if (bgRenderer == null)
        {
            bgRenderer = GetComponent<Renderer>();
        }

        if (bgRenderer != null)
        {
            _mat = bgRenderer.material;
        }
    }

    private void Update()
    {
        if (_mat == null) return;

        float currentSpeed = speed;

        // dash system background effect
        if (DashSystem.instance != null && DashSystem.instance.IsDashing)
        {
            currentSpeed *= dashSpeedMultiplier;
        }

        _mat.mainTextureOffset += new Vector2(currentSpeed * Time.deltaTime, 0);
    }
}