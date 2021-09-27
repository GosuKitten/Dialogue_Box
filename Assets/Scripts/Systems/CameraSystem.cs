using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public float shakeTime;
    public float shakeStrength;
    float shakenTime;
    bool shaking;

    private void Awake()
    {
        PlayerSystem.OnLand += Shake;
        PlayerSystem.OnCeilingHit += Shake;
    }

    private void Shake()
    {
        shaking = true;
        shakenTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (shaking)
        {
            shakenTime = Mathf.Min(shakenTime + Time.deltaTime, shakeTime);

            float x = Random.Range(-shakeStrength, shakeStrength);
            float y = Random.Range(-shakeStrength, shakeStrength);
            transform.position = new Vector3(x, y, -10);


            if (shakenTime == shakeTime)
            {
                transform.position = new Vector3(0, 0, -10);
                shaking = false;
            }
        }
    }
}
