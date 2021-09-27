using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForthSystem : MonoBehaviour
{
    HashSet<BackAndForthData> bafData = new HashSet<BackAndForthData>();

    private void Awake()
    {
        BackAndForthData.OnAddBackAndForthData += SwayData_OnAddBackAndForthData;
        BackAndForthData.OnRemoveBackAndForthData += SwayData_OnRemoveBackAndForthData;
    }

    private void SwayData_OnAddBackAndForthData(BackAndForthData data)
    {
        if (!bafData.Contains(data))
        {
            // set origian and target positions
            data.originalPos = data.transform.localPosition;
            data.targetPos = data.originalPos + data.targetOffset;

            // apply time offset
            data.moveTimer = data.moveTime * data.timerOffset;

            bafData.Add(data);
        }
    }

    private void SwayData_OnRemoveBackAndForthData(BackAndForthData data)
    {
        if (bafData.Contains(data))
            bafData.Remove(data);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (BackAndForthData data in bafData)
        {
            if (data.isEnabled)
            {
                data.moveTimer = Mathf.Min(data.moveTimer + Time.deltaTime, data.moveTime);
                float step = data.moveTimer / data.moveTime;

                data.transform.position = Vector3.Lerp(data.originalPos, data.targetPos, data.moveCurve.Evaluate(step));

                if (step == 1) data.moveTimer = 0;
            }
        }
    }
}
