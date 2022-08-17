using System.Collections;
using System.Collections.Generic;
using UI;
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

        public AudioSource music; // 音乐

        private GameObject lines; // 存放生成的line prefab的父物体

        // 是否输掉游戏
        private TimeCountDown loseTimer = new TimeCountDown(0.15f); // 计时器
        private bool _bLose = true;

        public bool bLose
        {
            get => _bLose;
            set
            {
                _bLose = value;
                if (_bLose)
                {
                    DancingLineGameManager.Instance.toggle.enabled = false;
                }
                else
                {
                    DancingLineGameManager.Instance.toggle.enabled = true;
                }
            }
        }

        // 返回主页面的时间
        private TimeCountDown backTime = new TimeCountDown(3f);

        // 开始游戏
        private bool _bStart;

        public bool bStart
        {
            get => _bStart;
            set
            {
                // 结束游戏
                if (!value)
                {
                    // 放置为原来的位置
                    gameObject.transform.position = new Vector3(0, 0, 0);
                    music.Stop();
                    DancingLineGameManager.Instance.OnOpenLoseCanvas();
                    
                    // 删除所有的prefab
                    List<Transform> lst = new List<Transform>();
                    foreach (Transform child in lines.transform)
                    {
                        lst.Add(child);
                        Debug.Log(child.gameObject.name);
                    }

                    foreach (var t in lst)
                    {
                        Destroy(t.gameObject);
                    }
                }
                else
                {
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    music.Play();
                    bLeft = false;
                    bLose = false;
                }
                
                _bStart = value;
            }
        }


        // 暂停游戏
        private bool _bPause;

        public bool bPause
        {
            get => _bPause;
            set
            {
                
                if (value)
                {
                    music.Pause();
                }
                else
                {
                    music.Play();
                }
                _bPause = value;
            }
        }

        // 是否长按
        private bool _bPress;

        public bool bPress
        {
            get => _bPress;
            set
            {
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
                _bPress = value;
            }
        }


        private void OnTriggerStay(Collider other)
        {
            loseTimer.FillTime();
        }

        private GameObject _objLinePrefab;

        private void Start()
        {
            _objLinePrefab = objLinePrefab1;
            lines = GameObject.Find("[Line]");
            StartCoroutine(CreatePrefab());
        }


        private void Update()
        {
            if (bStart && !bPause)
            {
                if (bPress)
                {
                    loseTimer.FillTime();
                }

                // loseTimer.Tick(Time.deltaTime);
                // if (loseTimer.TimeOut)
                // {
                //     bLose = true; // 输掉游戏
                //     gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                // }
                //
                //
                // if (bLose)
                // {
                //     backTime.Tick(Time.deltaTime);
                //     if (backTime.TimeOut)
                //     {
                //         bStart = false; // 取消开始游戏
                //         backTime.FillTime();
                //     }
                // }
            }
        }

        private void LateUpdate()
        {
            if (bStart && !bPause)
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
        }

        /// <summary>
        /// 生成Prefab
        /// </summary>
        void GeneratePrefab()
        {
            GameObject line = Instantiate(_objLinePrefab, lines.transform, true);
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
                if (bStart && !_bLose && !bPause)
                {
                    GeneratePrefab();
                }

                yield return new WaitForSeconds(0.01f);
            }
        }


        public void OnClick()
        {
            bLeft = !bLeft;
            if (!_bLose)
            {
                GeneratePrefab();
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