using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayData : MonoBehaviour
{
    public bool isEnabled;

    public float maxAngle;
    public float minAngle;

    [HideInInspector]
    public float swatTimer;
    public float swayTime;

    public AnimationCurve swayCurve;

    public delegate void AddSwayData(SwayData data);
    public static event AddSwayData OnAddSwayData;

    public delegate void RemoveSwayData(SwayData data);
    public static event RemoveSwayData OnRemoveSwayData;

    private void Start()
    {
        OnAddSwayData?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnRemoveSwayData?.Invoke(this);
    }

    private void OnEnable()
    {
        OnAddSwayData?.Invoke(this);
    }

    private void OnDisable()
    {
        OnRemoveSwayData?.Invoke(this);
    }
}
