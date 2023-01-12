using System.Collections;
using UnityEngine;

public class SetObjectInactive : MonoBehaviour
{
    public static SetObjectInactive Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void SetInActive(GameObject obj, float waitAmount) => StartCoroutine(SetInActiveCoroutine(obj, waitAmount));

    public IEnumerator SetInActiveCoroutine(GameObject obj, float waitAmount)
    {
        yield return new WaitForSeconds(waitAmount);
        obj.SetActive(false);
    }   
}
