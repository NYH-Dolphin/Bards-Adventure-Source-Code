using System;
using UnityEngine;

public class FanItem : MonoBehaviour
{
    private void Update()
    {
        float move = 0.001f * Mathf.Sin(Time.time);
        transform.Translate(Vector3.forward * move);
        transform.Rotate(Vector3.forward, 45 * Time.deltaTime, Space.Self);
    }
}