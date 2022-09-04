using System.Collections;
using System.Collections.Generic;
using Control;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DancingLineGameManager : MonoBehaviour
    {
        public GameObject mainCanvas;
        public GameObject gameCanvas;
        public GameObject loseCanvas;
        public GameObject pauseCanvas;
        public GameObject restartCanvas;
        public GameObject countDownCanvas;
        public GameObject winCanvas;
        public GameObject helpCanvas;

        public Toggle toggle; // 暂停/开始 toggle
        public Button restart; // 重新开始btn
        public Text countDown; // 倒计时
        public AudioSource btnEffect; // 按钮音效


        public AudioSource music; // 音乐


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

        // 输掉游戏
        private bool _bLose = true;

        public bool bLose
        {
            get => _bLose;
            set
            {
                _bLose = value;
                if (_bLose)
                {
                    Instance.toggle.enabled = false;
                }
                else
                {
                    Instance.toggle.enabled = true;
                }
            }
        }

        private bool _bWin;

        public bool bWin
        {
            get => _bWin;
            set
            {
                _bWin = value;
                if (_bWin)
                {
                    Instance.toggle.enabled = false;
                    restart.enabled = false;
                }
            }
        }

        // 游戏调用委托
        public delegate void VoidDelegate();

        // 开始游戏触发
        public VoidDelegate OnGameStart = null;

        // 结束游戏触发
        public VoidDelegate OnGameEnd = null;

        // 开始/结束游戏
        private bool _bStart;

        public bool bStart
        {
            get => _bStart;
            set
            {
                // 结束游戏
                if (!value)
                {
                    music.Stop();
                    OnGameEnd?.Invoke();
                }
                else
                {
                    music.Play();
                    OnGameStart?.Invoke();
                }

                _bStart = value;
            }
        }


        public CrownBehavior[] _listCrowns = new CrownBehavior[3];
        public List<ItemBehavior> _listItems = new List<ItemBehavior>();
        public List<AudioItem> _listAudios = new List<AudioItem>();

        public static DancingLineGameManager Instance;

        private void Awake()
        {
            Instance = this;
            iTween.Init(gameObject); // 记得要初始化 iTween...
        }


        private void Start()
        {
            OnOpenMainCanvas();
            toggle.onValueChanged.AddListener(isOn => { OnClickPauseToggle(toggle, isOn); });
            restart.onClick.AddListener(() =>
            {
                restartCanvas.SetActive(true);
                pauseCanvas.SetActive(false);
                bPause = true;
                toggle.enabled = false;
            });
        }

        // 注册皇冠
        public void RegisterCrown(CrownBehavior crown)
        {
            _listCrowns[crown.iIndex] = crown;
        }

        public void ResisterItem(ItemBehavior item)
        {
            _listItems.Add(item);
        }


        public void RegisterAudio(AudioItem item)
        {
            _listAudios.Add(item);
        }

        private void RefreshAudio()
        {
            foreach (AudioItem audio in _listAudios)
            {
                PlayerPrefs.SetInt(audio.gameObject.name, 0);
            }
        }

        public void OnClickGameStart()
        {
            bStart = false;
            bStart = true;
            bPause = false;
            toggle.isOn = false;
            mainCanvas.SetActive(false);
            gameCanvas.SetActive(true);
            loseCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            restartCanvas.SetActive(false);
            countDownCanvas.SetActive(false);
            winCanvas.SetActive(false);
            helpCanvas.SetActive(false);
            toggle.enabled = true;
            restart.enabled = true;
        }

        public void OnOpenMainCanvas()
        {
            mainCanvas.SetActive(true);
            gameCanvas.SetActive(false);
            loseCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            restartCanvas.SetActive(false);
            countDownCanvas.SetActive(false);
            winCanvas.SetActive(false);
            helpCanvas.SetActive(false);
            RefreshAudio();
        }


        public void OnOpenLoseCanvas()
        {
            loseCanvas.SetActive(true);
            mainCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            restartCanvas.SetActive(false);
            countDownCanvas.SetActive(false);
            winCanvas.SetActive(false);
            helpCanvas.SetActive(false);
        }


        public void OnOpenWinCanvas()
        {
            loseCanvas.SetActive(false);
            mainCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            restartCanvas.SetActive(false);
            countDownCanvas.SetActive(false);
            helpCanvas.SetActive(false);
            winCanvas.SetActive(true);
            winCanvas.GetComponent<WinPanel>().OnOpenWinPanel();
        }

        public void OnClickRestartBtn()
        {
            bPause = false;
            bStart = false;
            bWin = false;
            btnEffect.Play();
            RefreshCrown();
            GameObject.Find("Line").GetComponent<LineController>().RefreshCheckPoint();
            GameObject.Find("Line").GetComponent<LineController>().OnGameEnd();
            GameObject.Find("Main Camera").GetComponent<CameraMovement>().Restart();
            OnOpenMainCanvas();
        }


        public void OnClickCloseButton()
        {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }



        public void OnClickHelpButton()
        {
            btnEffect.Play();
            OnOpenHelpCanvas();
        }

        public void OnCloseHelpCanvas()
        {
            btnEffect.Play();
            OnOpenMainCanvas();
        }

        private void OnOpenHelpCanvas()
        {
            loseCanvas.SetActive(false);
            mainCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            restartCanvas.SetActive(false);
            countDownCanvas.SetActive(false);
            helpCanvas.SetActive(true);
            winCanvas.SetActive(false);
        }
        

        public void OnClickCancelRestart()
        {
            StartCoroutine(PauseCountDown());
        }

        public void OnClickPauseToggle(Toggle t, bool isOn)
        {
            if (!isOn)
            {
                StartCoroutine(PauseCountDown());
            }
            else
            {
                bPause = true;
                pauseCanvas.SetActive(true);
                restart.enabled = false;
                btnEffect.Play();
            }
        }

        public void RefreshCrown()
        {
            foreach (CrownBehavior crown in _listCrowns)
            {
                crown.Refresh();
            }
        }


        /// <summary>
        /// 当结束后从上一个检查点开始
        /// </summary>
        public void OnStartFromCheckPoint()
        {
            OnClickGameStart();
            int index = GameObject.Find("Line").GetComponent<LineController>().lastIndex;
            if (index >= 0)
            {
                _listCrowns[index].bGetCrown = false;
            }

            GameObject.Find("Main Camera").GetComponent<CameraMovement>().Refresh();
            toggle.isOn = true;
        }

        IEnumerator PauseCountDown()
        {
            restartCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            winCanvas.SetActive(false);
            countDownCanvas.SetActive(true);
            toggle.enabled = false;
            restart.enabled = false;
            for (int i = 3; i > 0; i--)
            {
                countDown.text = i.ToString();
                btnEffect.Play();
                yield return new WaitForSeconds(1f);
            }

            bPause = false;
            countDownCanvas.SetActive(false);
            toggle.enabled = true;
            restart.enabled = true;
        }
    }
}