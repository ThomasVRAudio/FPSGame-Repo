using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    [SerializeField] private GameObject detectorObject;
    private IDetect _detector;

    private void Start()
    {
        _detector = detectorObject.GetComponent<IDetect>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _detector.OnEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _detector.OnExit(other);
    }
}
