using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class LineController : MonoBehaviour
    {
        public float fSpeed; // 移动速度
        public bool bLeft; // 向左移动

        public GameObject objLinePrefab1;
        public GameObject objLinePrefab2;
        public Material mat1;
        public Material mat2;

        // 长按
        public bool bPress
        {
            get => _bPress;
            set
            {
                _bPress = value;
                if (value)
                {
                    _objLinePrefab = objLinePrefab2;
                    gameObject.GetComponent<MeshRenderer>().material = mat2;
                }
                else
                {
                    _objLinePrefab = objLinePrefab1;
                    gameObject.GetComponent<MeshRenderer>().material = mat1;
                }
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
        }

        private bool _bPress;

        private GameObject _objLinePrefab;

        private void Start()
        {
            _objLinePrefab = objLinePrefab1;
            StartCoroutine(CreatePrefab());
            GameObject line = Instantiate(_objLinePrefab);
            line.transform.position = transform.position;
        }


        /// <summary>
        /// 持续创建Prefab的过程
        /// </summary>
        /// <returns></returns>
        IEnumerator CreatePrefab()
        {
            while (true)
            {
                GameObject line = Instantiate(_objLinePrefab);
                line.transform.position = transform.position;
                yield return new WaitForSeconds(0.01f);
            }
        }


        private void LateUpdate()
        {
            if (bLeft)
            {
                transform.Translate(Vector3.left * fSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                transform.Translate(Vector3.forward * fSpeed * Time.deltaTime, Space.World);
            }
        }


        public void OnClick()
        {
            bLeft = !bLeft;
            GameObject line = Instantiate(_objLinePrefab);
            line.transform.position = transform.position;
        }


        public void OnLongPress()
        {
            bPress = true;
            OnClick();
        }


        public void OnCancelLongPress()
        {
            bPress = false;
        }
    }
}