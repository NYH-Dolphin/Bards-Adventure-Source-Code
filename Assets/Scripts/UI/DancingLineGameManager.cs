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
        
        public Toggle toggle;
        public Button restart;
        
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
                GameObject.Find("Line").gameObject.GetComponent<LineController>().bPause = true;
            });
        }

        public void OnClickGameStart()
        {
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bStart = false;
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bStart = true;
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bPause = false;
            mainCanvas.SetActive(false);
            gameCanvas.SetActive(true);
            loseCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            restartCanvas.SetActive(false);
        }

        public void OnOpenMainCanvas()
        {
            mainCanvas.SetActive(true);
            gameCanvas.SetActive(false);
            loseCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            restartCanvas.SetActive(false);
        }


        public void OnOpenLoseCanvas()
        {
            loseCanvas.SetActive(true);
            mainCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            restartCanvas.SetActive(false);
        }

        public void OnClickRestartBtn()
        {
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bPause = false;
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bStart = false;
            OnOpenMainCanvas();
        }

        public void OnClickCancelRestart()
        {
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bPause = false;
        }

        public void OnClickPauseToggle(Toggle t, bool isOn)
        {
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bPause = isOn;
            pauseCanvas.SetActive(isOn);
        }
        
    }
}