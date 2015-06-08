using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    float shakeAmt = .05f;
    bool shaking = false;
    float shakeCount = 0f;
    float shakeTime = 1f;
    Vector3 originalCameraPosition;

    void Start()
    {
        originalCameraPosition = transform.position;
    }

    void Update()
    {
        if (shaking && shakeCount < shakeTime)
        {
            float quakeAmt = Random.value * shakeAmt * 2 - shakeAmt;
            Vector3 pp = this.transform.position;
            if (Random.Range(0, 2) == 0)
            {
                pp.y += quakeAmt;
            }
            else
            {
                pp.x += quakeAmt;
            }
            this.transform.position = pp;
            shakeCount += Time.deltaTime;
        }
        else if (shakeCount > shakeTime)
        {
            shaking = false;
            shakeCount = 0;
            this.transform.position = originalCameraPosition;
        }
    }

    public void startShakingCamera(float maxTime = .2f, float shakeAmount = .05f)
    {
        originalCameraPosition = this.transform.position;
        shaking = true;
        this.shakeTime = maxTime;
        this.shakeAmt = shakeAmount;
    }

    public void stopShakingCamera()
    {
        shaking = false;
        this.transform.position = originalCameraPosition;
    }
}