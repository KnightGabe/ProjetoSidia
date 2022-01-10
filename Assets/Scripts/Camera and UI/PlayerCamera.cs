using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Transform followTarget;

    private Vector3 followOffset;

    public bool unparentOnStart = true;
    private bool lerping = false;

    private void Start()
    {
        if (unparentOnStart)
            transform.parent = null;
        followOffset = transform.position - followTarget.position;
    }

    public void ToggleCamera(float ease)
    {
        if (ease <= 0)
        {
            Camera.main.transform.SetParent(transform, false);
            Camera.main.transform.rotation = transform.rotation;
        }
        else
        {
            lerping = true;
            StartCoroutine(LerpCamera(ease, Camera.main.transform.position, Camera.main.transform.rotation)); 
        }
    }

    IEnumerator LerpCamera(float time, Vector3 startPos, Quaternion startRot)
    {
        float alpha = 0;
        while (alpha < 1)
        {
            Camera.main.transform.position = Vector3.Lerp(startPos, transform.position, alpha);
            Camera.main.transform.rotation = Quaternion.Lerp(startRot, transform.rotation, alpha);
            alpha += Time.deltaTime / time;
            yield return null;
        }
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.transform.rotation = transform.rotation;
        lerping = false;
    }

    private void LateUpdate()
    {
        if (Camera.main.transform.IsChildOf(transform))
        {
            if (unparentOnStart && !lerping)
                transform.position = followTarget.position + followOffset;
        }
        
    }

}
