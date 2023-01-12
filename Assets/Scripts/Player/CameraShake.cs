using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake On Fall Impact")]
    [SerializeField] private AnimationCurve fallCurve;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float fallRecovery = 0.2f;
    [SerializeField] private float jumpRecovery = 0.2f;
    [SerializeField] private Vector3 CameraFallOffset;
    [SerializeField] private Vector3 CameraJumpOffset;

    private Vector3 _curPosition;
    private Vector3 _curRotation;

    private void Start()
    {
        _curPosition = transform.localPosition;
        _curRotation = transform.localEulerAngles;
    }

    public IEnumerator Shake(float magnitude, float length, float shakeSpeed = 20f, float rotationPower = 5f)
    {
        float t = 0;


        float randomX = Random.Range(0, 10);
        float randomY = Random.Range(0, 10);
        float randomZ = Random.Range(0, 10);

        while (t < length)
        {
            transform.localPosition = _curPosition + (new Vector3(Mathf.PerlinNoise(Time.time * shakeSpeed + randomX, randomX) - 0.5f,
                                                            Mathf.PerlinNoise(Time.time * shakeSpeed + randomY, randomY) - 0.5f,
                                                            Mathf.PerlinNoise(Time.time * shakeSpeed + randomZ, randomZ) - 0.5f) * magnitude * (1 -  t / length));

            transform.localEulerAngles = _curRotation + new Vector3(0f, 0, (Mathf.PerlinNoise(Time.time * shakeSpeed + randomY, randomZ) - 0.5f) * magnitude * (1 - t / length) * rotationPower);

            

            t += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _curPosition;
        transform.localEulerAngles = _curRotation;
    }

    public void OnLanded(PlayerMovement player)
    {
        player.OnLand -= OnLanded;
        StartCoroutine(Landing());
    }

    private IEnumerator Landing()
    {
        float t = 0;

        while (t < fallRecovery)
        {
            transform.localPosition = Vector3.Lerp(_curPosition, _curPosition + CameraFallOffset, fallCurve.Evaluate(t / fallRecovery));
            t += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _curPosition;

    }

    public void OnJump() => StartCoroutine(Jump());

    private IEnumerator Jump()
    {
        float t = 0;

        while (t < jumpRecovery)
        {
            transform.localPosition = Vector3.Lerp(_curPosition, _curPosition + CameraJumpOffset, jumpCurve.Evaluate(t / jumpRecovery));
            t += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _curPosition;

    }
}
