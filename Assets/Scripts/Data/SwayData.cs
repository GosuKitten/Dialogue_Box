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

    //temp testing for dialogue sway controls
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            maxAngle += .05f;
            minAngle -= .05f;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            maxAngle = Mathf.Max(maxAngle - .05f, 0);
            minAngle = Mathf.Min(minAngle + .05f, 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            maxAngle = .5f;
            minAngle = -.5f;
        }
    }

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
