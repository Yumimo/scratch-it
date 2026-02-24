using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class GameData
{
    public InGame game;
    public ExistingData existingPlay;
        
    [Serializable]
    public class ExistingData
    {
        public string email;
        public Reward result;
        public string clainCode;
        public bool isClaimed;
    }

    [Serializable]
    public class InGame
    {
        public string code;
        public string name;
        public string type;
        public Reward[] rewards;
        public Meta meta;

    }

    [Serializable]
    public class Meta
    {
        public string logo;
    }

    [Serializable]
    public class Reward
    {
        public string photo;
        public string name;
        public bool isWin;
        
        [NonSerialized] 
        public Sprite rewardSprite;

        public IEnumerator LoadSprite(Action<Sprite> onLoaded)
        {
            if (string.IsNullOrEmpty(photo))
            {
                onLoaded?.Invoke(null);
                yield break;
            }

            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(photo))
            {
                yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                if (request.result != UnityWebRequest.Result.Success)
#else
                if (request.isNetworkError || request.isHttpError)
#endif
                {
                    Debug.LogError("Failed to load image: " + request.error);
                    onLoaded?.Invoke(null);
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    rewardSprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f)
                    );

                    onLoaded?.Invoke(rewardSprite);
                }
            }
        }
    }
    
}