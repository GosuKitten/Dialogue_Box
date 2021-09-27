using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationData : MonoBehaviour
{
    public bool isEnabled;
    public float rotationSpeed;

    public delegate void AddRotationData(RotationData rd);
    public static event AddRotationData OnAddRotationData;

    public delegate void RemoveRotationData(RotationData rd);
    public static event AddRotationData OnRemoveRotationData;

    void Start()
    {
        OnAddRotationData?.Invoke(this);
    }

    void OnDestroy()
    {
        OnRemoveRotationData?.Invoke(this);
    }
}
