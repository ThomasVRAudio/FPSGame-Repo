using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Image crossHair;
    [SerializeField] private float aimSpeed;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject aimBlock;

    public static GunManager Instance;

    public GameObject Gun;
    private FireWeapon _currentGun;

    public LayerMask GunIgnoreMask;

    private float _camOriginalFOV;
    private bool _isAiming = false;

    public bool IsAiming { get { return _isAiming; } }
    public float AimSpeed { get { return aimSpeed; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        } else
        {
            Destroy(this);
        }

        var gun = Instantiate(Gun, cam.transform);
        _currentGun = gun.GetComponent<FireWeapon>();

        var block = Instantiate(aimBlock, cam.transform);
        var pos = block.transform.localPosition;
        block.transform.localPosition = new Vector3(pos.x, pos.y, _currentGun.Range);
    }

    void Start()
    {
        _currentGun.WeaponModel.transform.SetPositionAndRotation(_currentGun.DefaultLocation.position, _currentGun.DefaultLocation.rotation);
        _camOriginalFOV = cam.fieldOfView;
        crossHair.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) AimWeapon();
        if (Input.GetMouseButtonUp(1)) StopAimWeapon();

        if (Input.GetMouseButtonDown(0)) _currentGun.Shoot(cam, playerMovement);
    }

    void AimWeapon()
    {
        if (_isAiming) return;
        _isAiming = true;

        StartCoroutine(Transition(50, _currentGun.AimLocation.localPosition, _currentGun.AimLocation.localRotation));
        crossHair.enabled = false;
        playerMovement.IsAiming = true;
    }

    void StopAimWeapon()
    {
        if (!_isAiming) return;
        _isAiming = false;
  
        StartCoroutine(Transition(_camOriginalFOV, _currentGun.DefaultLocation.localPosition, _currentGun.DefaultLocation.localRotation));
        crossHair.enabled = true;
        playerMovement.IsAiming = false;
    }

    private IEnumerator Transition(float fov, Vector3 endPos, Quaternion endRot)
    {
        float camView = cam.fieldOfView;
        Vector3 startPos = _currentGun.WeaponModel.transform.localPosition;
        Quaternion startRot = _currentGun.WeaponModel.transform.localRotation;

        float t = 0;

        while(t < aimSpeed)
        {
            cam.fieldOfView = Mathf.Lerp(camView, fov, aimSpeed / t);
            _currentGun.WeaponModel.transform.localPosition = Vector3.Lerp(startPos, endPos, t / aimSpeed);
            _currentGun.WeaponModel.transform.localRotation = Quaternion.Lerp(startRot, endRot, t / aimSpeed);
            t += Time.deltaTime;
            yield return null;
        }

        _currentGun.WeaponModel.transform.localPosition = endPos;
        _currentGun.WeaponModel.transform.localRotation = endRot;

    }
}
