using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Utils;

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

        private bool _bWin = false;

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


        public static DancingLineGameManager Instance;

        private void Awake()
        {
            Instance = this;
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
        }


        public void OnOpenWinCanvas()
        {
            loseCanvas.SetActive(false);
            mainCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            restartCanvas.SetActive(false);
            countDownCanvas.SetActive(false);
            winCanvas.SetActive(true);
        }

        public void OnClickRestartBtn()
        {
            bPause = false;
            bStart = false;
            bWin = false;
            btnEffect.Play();
            OnOpenMainCanvas();
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