using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationTimer : MonoBehaviour
{
    public PipeServer server;
    //public KeyCode calibrationKey = KeyCode.C;
    public int timer = 5;
    //public TextMeshProUGUI text;
    public GameObject textBox;
    private Text text;
    public GameObject RecalibrateButton;
    public GameObject CalibrateButton;
    public GameObject StartGameButton;
    public Canvas CalibrationCanvas;

    public GameObject sliders;

    private bool calibrated;

    void Awake()
    {
        //GameManager.onGameOver += GameEnded;
        Avatar.onPrepareCalibration += StartTimer;
        PhaseManager.onStartCalibration += calibrateButton;
        //PhaseManager.onStartGame += GameStarted;
        UnityEngine.Debug.Log(textBox);
        text = textBox.GetComponent<Text>();
    }

    void OnDestroy()
    {
        Avatar.onPrepareCalibration -= StartTimer;
        PhaseManager.onStartCalibration -= calibrateButton;
    }

    private void StartTimer()
    {
        text = textBox.GetComponent<Text>();
        calibrated = false;
        bool shouldEnable = false;
        Avatar[] a = FindObjectsByType<Avatar>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (Avatar aa in a)
        {
            UnityEngine.Debug.Log(aa);
            if (!aa.isActiveAndEnabled) continue;
            if (!aa.calibrationData)
            {
                shouldEnable = true;
                break;
            }
        }
        text.text = shouldEnable ? "Press the button to start the calibration timer." : "";
        StartGameButton.SetActive(false);
        CalibrateButton.SetActive(true);
        RecalibrateButton.SetActive(false);
        sliders.SetActive(true);

        gameObject.SetActive(shouldEnable);
        if (!shouldEnable)
        {
            server.SetVisible(false);
        }
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(calibrationKey))
        {
            if(!calibrated)
            {
                calibrated = true;
                StartCoroutine(Timer());
            }
            else
            {
                StartCoroutine(Notify());
            }
        }
    }*/

    private void calibrateButton()
    {
        if(!calibrated)
        {
            sliders.SetActive(false);
            calibrated = true;
            CalibrateButton.SetActive(false);
            StartCoroutine(Timer());
        }
    }

    /*private void GameStarted()
    {
        CalibrationCanvas.gameObject.SetActive(false);
    }

    private void GameEnded()
    {
        CalibrationCanvas.gameObject.SetActive(true);
        RecalibrateButton.SetActive(true);
        CalibrateButton.SetActive(false);
        sliders.SetActive(true);
        StartGameButton.SetActive(true);
        StartGameButton.GetComponentInChildren<Text>().text = "Restart Game";
        text.text = "Game Over";
    }*/

    private IEnumerator Timer()
    {
        int t = timer;
        while (t > 0)
        {
            text.text = "Copy the avatars starting pose: "+t.ToString();
            yield return new WaitForSeconds(1f);
            --t;
        }
        Avatar[] a = FindObjectsByType<Avatar>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach(Avatar aa in a)
        {
            UnityEngine.Debug.Log(aa);
            if (!aa.isActiveAndEnabled) continue;
            aa.Calibrate();
        }
        if (a.Length>0)
        {
            text.text = "Calibration Completed";
            //textCalibrateButton.text = "Recalibrate";
            CalibrateButton.SetActive(false);
            RecalibrateButton.SetActive(true);
            StartGameButton.SetActive(true);

            sliders.SetActive(false);

            server.SetVisible(false);
        }
        else
        {
            text.text = "Avatar in scene not found...";
        }
        yield return new WaitForSeconds(2f);
    }
    private IEnumerator Notify()
    {
        text.text = "Must restart instance to recalibrate."; // currently a limitation of the way things are set up
        yield return new WaitForSeconds(3f);
        text.text = "";
    }

    /*public void OnSliderChanged(float any)
    {
        StartGameButton.SetActive(false);
    }*/
}
