using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public APIManager apiManager;
    
    [SerializeField] private SpriteRenderer result;
    // [SerializeField] private GameObject loseSprite;

    [Range(0,1)]
    [SerializeField] private float winProbability;
    
    [Header("Results")]
    [HideInInspector] public UnityEvent<bool> ResultEvent;
    public UnityEvent WinEvent;
    public UnityEvent LoseEvent;
    
    private bool gameResult = true;

    private void Awake()
    {
        apiManager.OnGetUserData.AddListener(SetResult);
    }

    private void SetResult(UserData arg0)
    {
        gameResult = arg0.reward.isWin;
        StartCoroutine(arg0.reward.LoadSprite(x =>
        {
            result.sprite = x;
        }));
    }

    public void GameResult()
    {
        ResultEvent?.Invoke(gameResult);
        
        var _event =  gameResult ? WinEvent : LoseEvent;
        _event?.Invoke();
    }
}
