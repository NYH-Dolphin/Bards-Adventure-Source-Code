using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Control;
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

        // 皇冠相关
        private bool _bLastIsLeft; // 记录上一次步骤是否是向左
        public bool[] crownGet = new bool[3]; // 获得的皇冠数量
        private Vector3 _lastPos; // 上一次获得皇冠的时候的位置
        private Quaternion _lastRot; // 上一次获得皇冠的旋转
        private float _lastTime; // 上一次获得皇冠的时间点
        public int lastIndex;

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

        private TimeCountDown _startInterval = new TimeCountDown(0.5f);

        private TimeCountDown _longPress = new TimeCountDown(0.15f);

        private void Update()
        {
            // 避免空格冲突问题
            if (DancingLineGameManager.Instance.state == State.GAME && PlayerPrefs.GetInt("enter") == 1)
            {
                if (_startInterval.TimeOut)
                {
                    PlayerPrefs.SetInt("enter", 0);
                    _startInterval.FillTime();
                }
                else
                {
                    _startInterval.Tick(Time.deltaTime);
                    Vector3 dir = bLeft ? Vector3.left : Vector3.forward;
                    transform.Translate(dir * fSpeed * Time.deltaTime, Space.World);
                    return;
                }
            }

            if (DancingLineGameManager.Instance.bStart && !DancingLineGameManager.Instance.bLose &&
                !DancingLineGameManager.Instance.bPause)
            {
                if (!BPress)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        _longPress.Tick(Time.deltaTime);
                        if (_longPress.TimeOut)
                        {
                            OnLongPress();
                        }
                    }
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    if (!_longPress.TimeOut)
                    {
                        OnClick();
                    }
                    else
                    {
                        _longPress.FillTime();
                        OnCancelLongPress();
                    }
                }
            }


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
                    if (!_bObstacle)
                    {
                        loseTimer.FillTime();
                    }
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
                        // GameObject.Find("Main Camera").GetComponent<CameraMovement>().enabled = true;
                    }
                }

                if (!_bObstacle)
                {
                    Vector3 dir = bLeft ? Vector3.left : Vector3.forward;
                    transform.Translate(dir * fSpeed * Time.deltaTime, Space.World);
                }
            }
        }


        public void RecordCheckPoint(int index)
        {
            _bLastIsLeft = bLeft;
            crownGet[index] = true;
            _lastPos = transform.position;
            _lastRot = transform.rotation;
            _lastTime = DancingLineGameManager.Instance.music.time;
            lastIndex = index;
        }


        public void RefreshCheckPoint()
        {
            _bLastIsLeft = false;
            for (int i = 0; i < crownGet.Length; i++)
            {
                crownGet[i] = false;
            }

            _lastPos = new Vector3(0, 0, 0);
            _lastRot = new Quaternion(0, 0, 0, 0);
            _lastTime = 0f;
            lastIndex = -1;
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
                // Debug.Log($"collide {other.gameObject.name}");
                DancingLineGameManager.Instance.bLose = true; // 输掉游戏
                _bObstacle = true;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            // 游戏胜利
            else if (other.gameObject.layer == 8)
            {
                DancingLineGameManager.Instance.bWin = true; // 赢了
            }
        }


        /// <summary>
        /// 结束游戏触发委托
        /// </summary>
        public void OnGameEnd()
        {
            // 放置为原来的位置
            gameObject.transform.position = _lastPos;
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            DancingLineGameManager.Instance.OnOpenLoseCanvas();
            DancingLineGameManager.Instance.RefreshItems();

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
            bLeft = _bLastIsLeft;
            DancingLineGameManager.Instance.bLose = false;
            DancingLineGameManager.Instance.music.time = _lastTime;
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

                yield return new WaitForSeconds(1 / 6f - 0.03f);
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