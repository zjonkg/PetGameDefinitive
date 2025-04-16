using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class QrCode : MonoBehaviour
{
    [SerializeField] private RawImage _rawImageBackground;
    [SerializeField] private AspectRatioFitter _aspectRatioFitter;
    [SerializeField] private TextMeshProUGUI _textOut;
    [SerializeField] private RectTransform _scanZone;
    [SerializeField] private ProcessQRController processQRController;

    private bool _isCamAvailable;
    private WebCamTexture _webCamTexture;
    private bool _scanned = false;

    private IEnumerator Start()
    {
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            // 🔁 Esperar hasta que WebCamTexture.devices tenga al menos una cámara
            float timer = 0f;
            while (WebCamTexture.devices.Length == 0 && timer < 2f)
            {
                yield return new WaitForSeconds(0.5f);
                timer += 0.5f;
            }

            if (WebCamTexture.devices.Length > 0)
            {
                SetupCamera();
            }
            else
            {
                _textOut.text = "NO CAMERA FOUND AFTER PERMISSION";
            }
        }
        else
        {
            _textOut.text = "CAMERA PERMISSION DENIED";
        }
    }



    void Update()
    {
        if (!_isCamAvailable || _scanned) return;
        UpdateCameraRender();
        if (_webCamTexture.width > 100)
        {
            Scan();
        }
    }

    private void SetupCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            _isCamAvailable = false;
            return;
        }

        WebCamDevice bestDevice = devices[0];
        int bestResolution = 0;

        foreach (var device in devices)
        {
            WebCamTexture testCam = new WebCamTexture(device.name);
            testCam.Play();
            int resolution = testCam.width * testCam.height;
            testCam.Stop();

            if (resolution > bestResolution)
            {
                bestResolution = resolution;
                bestDevice = device;
            }
        }

        _webCamTexture = new WebCamTexture(bestDevice.name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);
        _webCamTexture.Play();
        _rawImageBackground.texture = _webCamTexture;
        _isCamAvailable = true;
    }

    private void UpdateCameraRender()
    {
        if (!_isCamAvailable) return;

        float ratio = (float)_webCamTexture.width / _webCamTexture.height;
        _aspectRatioFitter.aspectRatio = ratio;

        int orientation = -_webCamTexture.videoRotationAngle;
        bool mirrored = _webCamTexture.videoVerticallyMirrored;

        _rawImageBackground.rectTransform.localEulerAngles = new Vector3(0, mirrored ? 180 : 0, orientation);
    }

    private void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height);

            if (result != null)
            {
                _textOut.text = result.Text;
                processQRController.ValidateQRCodeFromExternal(result.Text, "6");
                _scanned = true;
                StopCamera();
            }
        }
        catch
        {
            _textOut.text = "FAILED TO READ QR CODE";
        }
    }

    private void StopCamera()
    {
        if (_webCamTexture != null)
        {
            _webCamTexture.Stop();
            _isCamAvailable = false;
        }
    }
}
