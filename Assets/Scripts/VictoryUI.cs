using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class VictoryUI : MonoBehaviour
{
    public GameObject textBox;
    private Text text;
    private Camera mainCam;
    public GameObject bg;

    public GameObject restartBtn;
    public GameObject quitBtn;
    public GameObject picBtn;

    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        text = textBox.GetComponent<Text>();
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        PictureTime.onTakePic += StartCountdown;

        canvas.worldCamera = mainCam;
    }

    void OnDestroy()
    {
        PictureTime.onTakePic -= StartCountdown;
    }

    public void StartCountdown()
    {
        picBtn.SetActive(false);
        quitBtn.SetActive(false);
        restartBtn.SetActive(false);
        StartCoroutine(countdown());
    }

    private IEnumerator countdown()
    {
        UnityEngine.Debug.Log("START COUNTDOWN");
        text.text = "5";
        yield return new WaitForSeconds (1.0f);
        text.text = "4";
        yield return new WaitForSeconds (1.0f);
        text.text = "3";
        yield return new WaitForSeconds (1.0f);
        text.text = "2";
        yield return new WaitForSeconds (1.0f);
        text.text = "1";
        yield return new WaitForSeconds (1.0f);
        textBox.SetActive(false);
        // yield return new WaitForSeconds (1.0f);
        Snap();
    }

    private void Snap() 
    {
        Resolution res = Screen.currentResolution;
 
        int resWidth = res.width;
        int resHeight = res.height;

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        mainCam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        mainCam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        mainCam.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        Texture2D snapshot = new Texture2D(1,1);
        ImageConversion.LoadImage(snapshot, bytes);

        // ShowPicture(snapshot);
        UnityEngine.Debug.Log("Snap taken");
        string filename = ScreenShotName(resWidth, resHeight);
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));

        text.text = "Picture Saved!";
        textBox.SetActive(true);
        quitBtn.SetActive(true);
        restartBtn.SetActive(true);
    }

    // private void ShowPicture(Texture2D picture)
    // {
    //     RawImage bgImg = bg.GetComponent<RawImage>();
    //     // bgImg.texture = picture;
    //     // Color c = new Color(255,255,0);
    //     // c.a = 1f;
    //     // bgImg.color = c;

    //     float alpha = 1f;
    //     bgImg.CrossFadeAlpha(alpha,1f,false);
    //     // bgImg.SetActive(true);
    //     // waitForButton();
    // }

    public static string ScreenShotName(int width, int height) 
    {
        return string.Format("{0}/VictoryShots/Victory_{1}x{2}_{3}.png", 
                             Application.dataPath, 
                             width, height, 
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
}
