using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private ParticleSystem pSystem;
    private LineRenderer _lineRenderer;

    private float range = 20f;
    public bool IsActive = false;

    void Start()
    {
        if (pSystem.isPlaying) pSystem.Stop();

        _lineRenderer = GetComponent<LineRenderer>();
        
    }

    void Update()
    {
        if (IsActive == false) return;

        ShootLaser();
    }

    public void TurnOff()
    {
        pSystem.Stop();
        _lineRenderer.enabled = false;
        IsActive = false;
    }

    public void TurnOn(float range = 20f)
    {
        this.range = range;
        IsActive = true;
    }

    void ShootLaser()
    {
        if (_lineRenderer.enabled == false) _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, _lineRenderer.transform.position);

        if (Physics.Raycast(transform.position, transform.rotation * Vector3.up, out RaycastHit hit, range, ~mask))
        {
            if(pSystem.isStopped)
                pSystem.Play();

            pSystem.transform.position = hit.point;
            _lineRenderer.SetPosition(1, hit.point);
        } else
        {
            if (pSystem.isPlaying)
                pSystem.Stop();

            _lineRenderer.SetPosition(1, _lineRenderer.GetPosition(0) + (transform.rotation * Vector3.up * range));
        }      
        
    }
}
