using System.Collections;
using UnityEngine;

public class LaserTrap : MonoBehaviour, IDetect
{
    [SerializeField] private float rotateStartUpTime;
    [SerializeField] private AnimationCurve rotateStartUpCurve;
    [SerializeField] private float rotateSpeed = 0;
    [SerializeField] private float laserRange = 40f;
    [SerializeField] private Laser[] Lasers;
    
    private float _curRotateSpeed;
    private IEnumerator _startRotatingCoroutine;
    private IEnumerator _stopRotatingCoroutine = null;

    void Update()
    {
        transform.Rotate(Vector3.up, _curRotateSpeed * Time.deltaTime);
    }

    public void OnEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() == null) return;

        _startRotatingCoroutine = StartRotating();
        if(_stopRotatingCoroutine != null) StopCoroutine(_stopRotatingCoroutine);
        StartCoroutine(_startRotatingCoroutine);


    }

    public void OnExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() == null) return;
        _stopRotatingCoroutine = StopRotating();
        StopCoroutine(_startRotatingCoroutine);
        StartCoroutine(_stopRotatingCoroutine);
    }

    private IEnumerator StartRotating()
    {
        foreach (var laser in Lasers)
            laser.TurnOn(laserRange);

        float t = 0;

        float rotateSpeedAtStart = _curRotateSpeed;

        while ( t < rotateStartUpTime)
        {
            _curRotateSpeed = Mathf.Lerp(rotateSpeedAtStart, rotateSpeed, rotateStartUpCurve.Evaluate(t / rotateStartUpTime));
            t += Time.deltaTime;
            yield return null;
        }

        _curRotateSpeed = rotateSpeed; 
    }

    private IEnumerator StopRotating()
    {
        float t = 0;

        float rotateSpeedAtStart = _curRotateSpeed;

        while (t < rotateStartUpTime)
        {
            _curRotateSpeed = Mathf.Lerp(rotateSpeedAtStart, 0 , rotateStartUpCurve.Evaluate(t / rotateStartUpTime));
            t += Time.deltaTime;
            yield return null;
        }

        foreach (var laser in Lasers)
            laser.TurnOff();

        _curRotateSpeed = 0;
    }
}
