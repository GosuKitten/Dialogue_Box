using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForthData : MonoBehaviour
{
    public bool isEnabled;

    [HideInInspector]
    public float moveTimer;
    public float moveTime;
    [Range(0,1)]
    public float timerOffset;

    [HideInInspector]
    public Vector3 originalPos;
    [HideInInspector]
    public Vector3 targetPos;
    public Vector3 targetOffset;

    public AnimationCurve moveCurve;

    public delegate void AddBackAndForthData(BackAndForthData data);
    public static event AddBackAndForthData OnAddBackAndForthData;

    public delegate void RemoveBackAndForthData(BackAndForthData data);
    public static event RemoveBackAndForthData OnRemoveBackAndForthData;

    private void Start()
    {
        OnAddBackAndForthData?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnRemoveBackAndForthData?.Invoke(this);
    }

    private void OnEnable()
    {
        OnAddBackAndForthData?.Invoke(this);
    }

    private void OnDisable()
    {
        OnRemoveBackAndForthData?.Invoke(this);
    }
}
