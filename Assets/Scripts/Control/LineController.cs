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
        private GameObject _objLinePrefab;
        public Material mat1;
        public Material mat2;

        private GameObject _lines; // 存放生成的line prefab的父物体

        // 是否输掉游戏
        private TimeCountDown loseTimer = new TimeCountDown(0.15f); // 计时器

        private TimeCountDown winTimer = new TimeCountDown(5f);

        // 是否碰到障碍物
        private bool _bObstacle;

        // 返回主页面的世界
        private TimeCountDown backTime = new TimeCountDown(2f);

        // 是否长按
        private bool _bPress;

        public bool BPress
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

        private void Start()
        {
            _objLinePrefab = objLinePrefab1;
            _lines = GameObject.Find("[Line]");
            StartCoroutine(CreatePrefab());
            DancingLineGameManager.Instance.OnGameStart += OnGameStart;
            DancingLineGameManager.Instance.OnGameEnd += OnGameEnd;
        }

        private void Update()
        {
            if (DancingLineGameManager.Instance.bWin)
            {
                winTimer.Tick(Time.deltaTime);
                if (winTimer.TimeOut)
                {
                    DancingLineGameManager.Instance.OnOpenWinCanvas();
                    winTimer.FillTime();
                    return;
                }

                Vector3 dir = bLeft ? Vector3.left : Vector3.forward;
                transform.Translate(dir * fSpeed * Time.deltaTime, Space.World);
                return;
            }


            if (DancingLineGameManager.Instance.bStart && !DancingLineGameManager.Instance.bPause)
            {
                if (BPress)
                {
                    loseTimer.FillTime();
                }

                loseTimer.Tick(Time.deltaTime);
                if (loseTimer.TimeOut)
                {
                    DancingLineGameManager.Instance.bLose = true; // 输掉游戏
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                }


                if (DancingLineGameManager.Instance.bLose)
                {
                    backTime.Tick(Time.deltaTime);
                    if (backTime.TimeOut)
                    {
                        DancingLineGameManager.Instance.bStart = false; // 结束游戏
                        backTime.FillTime();
                    }
                }

                if (!_bObstacle)
                {
                    Vector3 dir = bLeft ? Vector3.left : Vector3.forward;
                    transform.Translate(dir * fSpeed * Time.deltaTime, Space.World);
                }
            }
        }


        private void OnTriggerStay(Collider other)
        {
            // 碰撞是的地面的情况
            if (other.gameObject.layer == 6)
            {
                loseTimer.FillTime();
            }
            // 碰撞的是障碍物的情况
            else if (other.gameObject.layer == 7)
            {
                Debug.Log($"collide {other.gameObject.name}");
                DancingLineGameManager.Instance.bLose = true; // 输掉游戏
                _bObstacle = true;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            // 游戏胜利
            else if (other.gameObject.layer == 8)
            {
                Debug.Log(other.gameObject.name);
                DancingLineGameManager.Instance.bWin = true; // 赢了
            }
        }


        /// <summary>
        /// 结束游戏触发委托
        /// </summary>
        private void OnGameEnd()
        {
            // 放置为原来的位置
            gameObject.transform.position = new Vector3(0, 0, 0);
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            DancingLineGameManager.Instance.OnOpenLoseCanvas();

            // 删除所有的prefab
            List<Transform> lst = new List<Transform>();
            foreach (Transform child in _lines.transform)
            {
                lst.Add(child);
            }

            foreach (var t in lst)
            {
                Destroy(t.gameObject);
            }
        }

        /// <summary>
        /// 开始游戏触发委托
        /// </summary>
        private void OnGameStart()
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            bLeft = false;
            DancingLineGameManager.Instance.bLose = false;
            _bObstacle = false;
        }


        /// <summary>
        /// 生成Prefab
        /// </summary>
        void GeneratePrefab()
        {
            GameObject line = Instantiate(_objLinePrefab, _lines.transform, true);
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
                if (DancingLineGameManager.Instance.bStart && !DancingLineGameManager.Instance.bLose &&
                    !DancingLineGameManager.Instance.bPause)
                {
                    GeneratePrefab();
                }

                yield return new WaitForSeconds(1 / 6f - 0.01f);
            }
        }


        public void OnClick()
        {
            if (!DancingLineGameManager.Instance.bWin)
            {
                bLeft = !bLeft;
            }

            if (!DancingLineGameManager.Instance.bLose)
            {
                GeneratePrefab();
            }
        }


        public void OnLongPress()
        {
            if (!DancingLineGameManager.Instance.bLose)
            {
                BPress = true;
            }

            OnClick();
        }


        public void OnCancelLongPress()
        {
            BPress = false;
        }
    }
}