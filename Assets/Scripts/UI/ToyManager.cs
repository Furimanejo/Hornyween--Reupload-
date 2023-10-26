using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class ToyManager : MonoBehaviour
{
    public static string lovenseStatus { get; private set; } = "";
    public static float vibrationScore = 0;
    [SerializeField] Toggle enableVibrations;
    [SerializeField] Toggle showDebugInformation;
    [SerializeField] Slider maxVibrationIntensity;
    [SerializeField] TMP_InputField domain;
    int port = 20010;
    static ToyManager instance = null;
    float clearTimer = 0f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            StartCoroutine(PostCoroutine());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        clearTimer += Time.deltaTime;
        if (clearTimer > 0.5f)
        {
        }
    }

    private void OnGUI()
    {
        if (showDebugInformation.isOn)
            GUI.Box(new Rect(0, 0, 200, 40), lovenseStatus);
    }

    public async void SendTest()
    {
        print("Send Test");
        vibrationScore = 25;
        await System.Threading.Tasks.Task.Delay(500);
        vibrationScore = 50;
        await System.Threading.Tasks.Task.Delay(500);
        vibrationScore = 75;
        await System.Threading.Tasks.Task.Delay(500);
        vibrationScore = 100;
        await System.Threading.Tasks.Task.Delay(500);
        vibrationScore = 0;
    }

    IEnumerator PostCoroutine()
    {
        UnityWebRequest request;
        while (true)
        {
            if (enableVibrations.isOn)
            {
                var sendValue = (int)(maxVibrationIntensity.value * Mathf.Clamp(vibrationScore, 0f, 100f) / 100f);
                vibrationScore = 0.5f * vibrationScore;
                var command = new LovenseCommand()
                {
                    command = "Function",
                    action = $"Vibrate:{sendValue}",
                    timeSec = 5,
                    apiVer = 1
                };
                string postData = JsonUtility.ToJson(command);
                // Using the POST method will corrupt the json data, encoding manually instead
                var URL = $"http://{domain.text}:{port}/command";
                try
                {
                    request = new UnityWebRequest(URL, "POST");
                    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(postData);
                    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");
                    request.certificateHandler = new BypassCertificateHandler();
                    request.timeout = 1;
                }
                catch
                {
                    request = null;
                }

                var timer = Time.time;
                if(request != null)
                {
                    yield return request.SendWebRequest();
#if UNITY_EDITOR
                    print($"sent {sendValue} to {URL} => {request.responseCode} => {request.downloadHandler.text}");
#endif
                    if(request.responseCode != 200)
                    {
                        lovenseStatus = $"Connection error";
                    }
                    else
                    {
                        if (request.downloadHandler.text.Contains("\"code\":400"))
                        {
                            lovenseStatus = $"Finding port...";
                            port++;
                            if (port > 20020)
                                port = 20010;
                        }
                        else if(request.downloadHandler.text.Contains("\"code\":402"))
                        {
                            lovenseStatus = $"No toy connected...";
                        }
                        else
                        {
                            var ping = (int)(1000 * (Time.time - timer));
                            lovenseStatus = $"lovense ping: {ping} ms\nsending vibration: {5*sendValue}%";
                        }
                    }
                    request.disposeCertificateHandlerOnDispose = true;
                    request.disposeDownloadHandlerOnDispose = true;
                    request.disposeUploadHandlerOnDispose = true;
                    request.Dispose();
                }
            }
            else
            {
                lovenseStatus = "Not enabled";
            }
            yield return new WaitForEndOfFrame();
        }
    }

    struct LovenseCommand
    {
        public string command;
        public string action;
        public double timeSec;
        public int apiVer;
    }

    class BypassCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}