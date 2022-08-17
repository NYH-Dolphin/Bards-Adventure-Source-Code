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

        public Toggle toggle; // 暂停/开始 toggle
        public Button restart; // 重新开始btn
        public Text countDown; // 倒计时
        public AudioSource btnEffect; // 按钮音效
        public AudioSource countdownEffect; // 倒计时音效

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
                GameObject.Find("Line").gameObject.GetComponent<LineController>().bPause = true;
                toggle.enabled = false;
            });
        }
        
        
        

        public void OnClickGameStart()
        {
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bStart = false;
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bStart = true;
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bPause = false;
            toggle.isOn = false;
            mainCanvas.SetActive(false);
            gameCanvas.SetActive(true);
            loseCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            restartCanvas.SetActive(false);
            countDownCanvas.SetActive(false);
        }

        public void OnOpenMainCanvas()
        {
            mainCanvas.SetActive(true);
            gameCanvas.SetActive(false);
            loseCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            restartCanvas.SetActive(false);
            countDownCanvas.SetActive(false);
        }


        public void OnOpenLoseCanvas()
        {
            loseCanvas.SetActive(true);
            mainCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            restartCanvas.SetActive(false);
            countDownCanvas.SetActive(false);
        }

        public void OnClickRestartBtn()
        {
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bPause = false;
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bStart = false;
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
                GameObject.Find("Line").gameObject.GetComponent<LineController>().bPause = true;
                pauseCanvas.SetActive(true);
                restart.enabled = false;
                btnEffect.Play();
            }
        }


        IEnumerator PauseCountDown()
        {
            restartCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            countDownCanvas.SetActive(true);
            toggle.enabled = false;
            restart.enabled = false;
            for (int i = 3; i > 0; i--)
            {
                countDown.text = i.ToString();
                btnEffect.Play();
                yield return new WaitForSeconds(1f);
            }

            GameObject.Find("Line").gameObject.GetComponent<LineController>().bPause = false;
            countDownCanvas.SetActive(false);
            toggle.enabled = true;
            restart.enabled = true;
        }
    }
}