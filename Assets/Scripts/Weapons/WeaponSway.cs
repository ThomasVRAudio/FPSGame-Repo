using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float intensity;
    [SerializeField] private float smooth;

    private bool _hasSetRotation = false;

    private Quaternion _originRotation;

    // Start is called before the first frame update
    void Start()
    {
        _originRotation = transform.localRotation;
        _hasSetRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSway();

        if(_hasSetRotation != GunManager.Instance.IsAiming) Invoke(nameof(ChangeRotation), GunManager.Instance.AimSpeed);
    }

    void ChangeRotation()
    {
        _hasSetRotation = GunManager.Instance.IsAiming;
        _originRotation = transform.localRotation;
    }

    void UpdateSway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Quaternion adjustmentX = Quaternion.AngleAxis(-intensity * mouseX, Vector3.up);
        Quaternion adjustmentY = Quaternion.AngleAxis(intensity * mouseY, Vector3.right);
        Quaternion targetRotation = _originRotation * adjustmentX * adjustmentY;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
    }


}
