using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CrownItem : MonoBehaviour
    {
        private CrownBehavior _crownBehavior;


        private void Start()
        {
            _crownBehavior = transform.parent.GetComponent<CrownBehavior>();
        }

        private void Update()
        {
            float move = 0.001f * Mathf.Sin(Time.time);
            transform.Translate(Vector3.forward * move);
            transform.Rotate(Vector3.forward, 45 * Time.deltaTime, Space.Self);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Contains("Line"))
            {
                _crownBehavior.OnGetCrown(other.transform.GetComponent<LineController>());
            }
        }
    }
}