using UnityEngine;

public interface IDetect
{
    void OnEnter(Collider other);
    void OnExit(Collider other);
}
