using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public partial class APIManager : MonoBehaviour
{
    public string gameId ;

    public string debugEmail;
    //Routes
    private const string baseUrl = "https://msys-games-cms-ws.multisyscorp.io";
    private string gameUrl => $"{baseUrl}/pub/game/{gameId}";
    private string emailUrl => $"{baseUrl}/pub/game/{gameId}/play";
    
    //API DATA
    public GameData gameData;
    public UserData userData;
    
    // Unity Event
    public UnityEvent<UserData> OnGetUserData;
    private void Start()
    {
        StartCoroutine(GetRequest(gameUrl, x =>
        {
            gameData = JsonUtility.FromJson<GameData>(x);
        }));
        
        //Login(debugEmail);
    }

    public void Login(string _email)
    {
        var json = JsonUtility.ToJson(new UserData.EmailRequest
        {
            email = _email
        });
        Debug.Log(json);
        StartCoroutine(PostRequest(emailUrl, json, x =>
        {
            userData = JsonUtility.FromJson<UserData>(x);
            OnGetUserData?.Invoke(userData);
        }));
    }

    private IEnumerator GetRequest(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

#if UNITY_2020_2_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
            {
                Debug.LogError("GET Error: " + request.error);
            }
            else
            {
                Debug.Log("GET Response: " + request.downloadHandler.text);
                callback?.Invoke(request.downloadHandler.text);
            }
        }
    }

    private IEnumerator PostRequest(string url, string json, Action<string> callback)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

#if UNITY_2020_2_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
            {
                Debug.LogError("POST Error: " + request.error);
            }
            else
            {
                Debug.Log("POST Response: " + request.downloadHandler.text);
                callback?.Invoke(request.downloadHandler.text);
            }
        }
    }
}