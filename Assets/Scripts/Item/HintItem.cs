using System;
using DefaultNamespace;
using UnityEngine;

namespace Item
{
    public class HintItem : MonoBehaviour
    {
        public GameObject obj;

        void Start()
        {
            obj.transform.localScale = new Vector3(0, 0, 0);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Line")
            {
                iTween.ScaleTo(obj, iTween.Hash("time", 0.7, "easetype", iTween.EaseType.spring, "scale",
                    new Vector3(1, 1, 1)));
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "Line")
            {
                iTween.ScaleTo(obj, iTween.Hash("time", 0.7, "easetype", iTween.EaseType.spring, "scale",
                    new Vector3(0, 0, 0)));
            }
        }
    }
}