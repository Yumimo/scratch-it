using System.Collections.Generic;
using UnityEngine;

public class ScratchCard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public SpriteMask scratchPointer;
    [SerializeField] public LayerMask scratchMask;
    [SerializeField] public Collider2D coverCollider;
    [SerializeField] private GameManager gameManager;

    [Header("Settings")]
    [SerializeField] private float maskSize = 0.5f; 
    [SerializeField] private float percentToWin = 0.75f; 
    
    // [Header("Events")]
    // public UnityEvent OnLevelComplete;

    private Camera cam;
    private Vector3 lastPos;
    private bool isDragging = false;
    private bool isComplete = false;

    private List<Vector2> checkPoints = new List<Vector2>();
    private int totalPoints;
    private int checkpointsEaten = 0;

    private void Start()
    {
        cam = Camera.main;
        GenerateCheckpoints();
    }

    private void Update()
    {
        if (isComplete || cam == null) return;

        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = -cam.transform.position.z;

        Vector3 mouseWorld3 = cam.ScreenToWorldPoint(mouseScreen);
        Vector3 origin = cam.transform.position;
        Vector3 target = mouseWorld3;

        Vector2 dir = (target - origin).normalized;
        float dist = Vector2.Distance(origin, target);

        var hit = Physics2D.Raycast(target, dir, dist, scratchMask);
        bool hasHit = hit.collider != null;

        if (Input.GetMouseButtonDown(0) && hasHit)
        {
            isDragging = true;
            lastPos = target; 
            SpawnMask(target);
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            float distanceMoved = Vector3.Distance(target, lastPos);
            if (distanceMoved > maskSize / 4f) 
            {
                if(!hasHit)return;
                FillGap(lastPos, target);
                lastPos = target;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private void FillGap(Vector3 start, Vector3 end)
    {
        float dist = Vector3.Distance(start, end);
        Vector3 dir = (end - start).normalized;
        int steps = Mathf.CeilToInt(dist / (maskSize / 2f));

        for (int i = 0; i <= steps; i++)
        {
            Vector3 pos = start + (dir * (dist * ((float)i / steps)));
            SpawnMask(pos);
        }
    }

    private void SpawnMask(Vector3 pos)
    {
        var _mask = Instantiate(scratchPointer, pos, Quaternion.identity);
        _mask.transform.SetParent(this.transform); 
        _mask.enabled = true;
        CheckProgress(pos);
        AudioManager.Instance.PlaySfx(0);
    }

    private void GenerateCheckpoints()
    {
        if (coverCollider == null) return;
        
        Bounds b = coverCollider.bounds;
        int gridX = 10;
        int gridY = 10;

        float stepX = b.size.x / gridX;
        float stepY = b.size.y / gridY;

        for (float x = b.min.x + stepX/2; x < b.max.x; x += stepX)
        {
            for (float y = b.min.y + stepY/2; y < b.max.y; y += stepY)
            {
                Vector2 p = new Vector2(x, y);
                if (coverCollider.OverlapPoint(p))
                {
                    checkPoints.Add(p);
                }
            }
        }
        totalPoints = checkPoints.Count;
    }

    private void CheckProgress(Vector2 pos)
    {
        for (int i = checkPoints.Count - 1; i >= 0; i--)
        {
            if (Vector2.Distance(checkPoints[i], pos) < maskSize / 1.5f)
            {
                checkPoints.RemoveAt(i);
                checkpointsEaten++;
            }
        }

        float currentPercent = (float)checkpointsEaten / totalPoints;

        if (currentPercent >= percentToWin)
        {
            isComplete = true;
            gameManager.GameResult();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (var p in checkPoints)
        {
            Gizmos.DrawSphere(p, 0.05f);
        }
    }
}