using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;


public class ProcessQRController : MonoBehaviour
{
    private string apiUrl = "https://api-management-pet-production.up.railway.app/pets/validate_qr";
    private string apiUrl2 = "https://api-management-pet-production.up.railway.app/pets/assign_pet";


    public void ValidateQRCodeFromExternal(string qrCode, string userId)
    {
        StartCoroutine(ValidateQRCode(qrCode, userId));
    }

    private IEnumerator ValidateQRCode(string qrCode, string userId)
    {
        string requestUrl = apiUrl + "?content=" + UnityWebRequest.EscapeURL(qrCode);

        yield return HttpService.Instance.SendRequest<QRResponse>(
            requestUrl,
            "POST",
            null,
            (response) =>
            {
                if (response.valid)
                {
                    Debug.Log("QR válido, asignando mascota...");
                    StartCoroutine(AssignPet(qrCode, userId));
                }
            },
            (error) =>
            {
                Debug.LogError("Error en validación: " + error);
                AnimationError.Instance.ShowPopup("Error", "QR inválido");
            }
        );
    }

    private IEnumerator AssignPet(string qrCode, string userId)
    {
        var requestBody = new
        {
            qr_code = qrCode,
            user_id = userId
        };

        yield return HttpService.Instance.SendRequest<object>(
            apiUrl2,
            "POST",
            requestBody,
            (response) =>
            {
                // Manejo de la respuesta exitosa
                Debug.Log("Respuesta de asignación: Mascota asignada correctamente.");
                SceneManager.LoadScene("House");
            },
            (error) =>
            {
                // Manejo del error
                Debug.LogError("Error en asignación: " + error);
                AnimationError.Instance.ShowPopup("Error", "Mascota ya asignada a otro usuario");
            }
        );
    }
}

[System.Serializable]
public class QRResponse
{
    public bool valid { get; set; }
    public string message { get; set; }
}
