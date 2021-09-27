using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSystem : MonoBehaviour
{
    List<RotationData> rotationObjects = new List<RotationData>();

    private void Awake()
    {
        RotationData.OnAddRotationData += RotationData_OnAddRotationData;
        RotationData.OnRemoveRotationData += RotationData_OnRemoveRotationData;
    }

    void RotationData_OnAddRotationData(RotationData rd)
    {
        if (!rotationObjects.Contains(rd)) rotationObjects.Add(rd);
    }

    void RotationData_OnRemoveRotationData(RotationData rd)
    {
        if (rotationObjects.Contains(rd)) rotationObjects.Remove(rd);
    }

    void Update()
    {
        foreach (RotationData ro in rotationObjects)
        {
            if (ro.isEnabled)
            {
                ro.transform.Rotate(0, 0, ro.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
