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
        public Toggle toggle;

        public static DancingLineGameManager Instance;

        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {
            OnOpenMainCanvas();
            toggle.onValueChanged.AddListener(isOn => { OnClickPauseToggle(toggle, isOn); });
        }

        public void OnClickGameStart()
        {
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bStart = true;
            mainCanvas.SetActive(false);
            gameCanvas.SetActive(true);
            loseCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
        }

        public void OnOpenMainCanvas()
        {
            mainCanvas.SetActive(true);
            gameCanvas.SetActive(false);
            loseCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
        }


        public void OnOpenLoseCanvas()
        {
            loseCanvas.SetActive(true);
            mainCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
        }


        public void OnClickPauseToggle(Toggle t, bool isOn)
        {
            GameObject.Find("Line").gameObject.GetComponent<LineController>().bPause = isOn;
            pauseCanvas.SetActive(isOn);
        }
        
    }
}