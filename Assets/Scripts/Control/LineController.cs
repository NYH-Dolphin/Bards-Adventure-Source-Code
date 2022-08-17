using System;
using System.Collections;
using UnityEngine;
using Utils;

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

        // 是否输掉了
        private TimeCountDown loseTimer = new TimeCountDown(0.4f);
        private bool _bLose;
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


        private void OnTriggerStay(Collider other)
        {
            loseTimer.FillTime();
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


        private void Update()
        {
            if (bPress)
            {
                loseTimer.FillTime();
            }
            loseTimer.Tick(Time.deltaTime);
            if (loseTimer.TimeOut)
            {
                _bLose = true;
                StopCoroutine(CreatePrefab());
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        /// <summary>
        /// 持续创建Prefab的过程
        /// </summary>
        /// <returns></returns>
        IEnumerator CreatePrefab()
        {
            while (!_bLose)
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
            if (!_bLose)
            {
                GameObject line = Instantiate(_objLinePrefab);
                line.transform.position = transform.position;
            }
        }


        public void OnLongPress()
        {
            if (!_bLose)
            {
                bPress = true;
            }
            OnClick();
        }


        public void OnCancelLongPress()
        {
            bPress = false;
        }
    }
}