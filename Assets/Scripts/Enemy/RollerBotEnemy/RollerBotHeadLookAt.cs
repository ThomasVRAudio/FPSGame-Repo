using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerBotHeadLookAt : MonoBehaviour
{
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject turrets;
    [SerializeField] private float headSpeed = 100f;
    [SerializeField] private float turretSpeed = 80f;

    public void LookAtTarget(Vector3 target)
    {
        head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation,
                  Quaternion.LookRotation((target - transform.position).normalized) * Quaternion.Euler(0, 90f, 0),
                  Time.deltaTime * headSpeed);

        turrets.transform.rotation = Quaternion.RotateTowards(turrets.transform.rotation,
          Quaternion.LookRotation((target - transform.position).normalized) * Quaternion.Euler(0, -180f, 0),
          Time.deltaTime * turretSpeed);
    }
}
