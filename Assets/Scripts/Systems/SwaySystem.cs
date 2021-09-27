using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwaySystem : MonoBehaviour
{
    HashSet<SwayData> swayDatas = new HashSet<SwayData>();


    private void Awake()
    {
        SwayData.OnAddSwayData += SwayData_OnAddSwayData;
        SwayData.OnRemoveSwayData += SwayData_OnRemoveSwayData;
    }

    private void SwayData_OnAddSwayData(SwayData data)
    {
        if (!swayDatas.Contains(data))
            swayDatas.Add(data);
    }

    private void SwayData_OnRemoveSwayData(SwayData data)
    {
        if (swayDatas.Contains(data))
            swayDatas.Remove(data);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SwayData data in swayDatas)
        {
            if (data.isEnabled)
            {
                data.swatTimer = Mathf.Min(data.swatTimer + Time.deltaTime, data.swayTime);
                float step = data.swatTimer / data.swayTime;

                float angle = Mathf.Lerp(data.minAngle, data.maxAngle, data.swayCurve.Evaluate(step));
                data.transform.rotation = Quaternion.Euler(0, 0, angle);

                if (step == 1) data.swatTimer = 0;
            }
        }
    }
}
