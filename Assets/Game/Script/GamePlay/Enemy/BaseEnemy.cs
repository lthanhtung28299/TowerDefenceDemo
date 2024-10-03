using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;


public abstract class BaseEnemy : MonoBehaviour
{
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public HealthRendererView HealthRenderer { get; private set; }
    [field: SerializeField] public Transform EnemyRendererTransform { get; private set; }
    [field: SerializeField] public float EnemySpeed { get; private set; }
    [field: SerializeField] public int GoldEarn { get; private set; }
    public WalkablePathInfo WalkablePathInfo { get; private set; }
    public float PathProgress { get; private set; }
    protected Coroutine followCoroutine;
    protected int currentWaypointIndex;
    protected bool isMoving;

    protected virtual void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDeath += HandleDeath;
    }

    protected virtual void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDeath -= HandleDeath;
        StopMoving();
    }

    public void SetUpEnemy(WalkablePathInfo walkablePathInfo)
    {
        this.Show();
        Health.Initialize();
        HealthRenderer.UpdateHealthView(1f);
        WalkablePathInfo = walkablePathInfo;
        currentWaypointIndex = 0;
        StartMoving();
    }

    public void GetWalkablePathInfo()
    {
        var randomPath = GridManager.Instance.WalkablePath[Random.Range(0, GridManager.Instance.WalkablePath.Count)];
        WalkablePathInfo = new WalkablePathInfo(randomPath);
        this.Show();
        Health.Initialize();
        HealthRenderer.UpdateHealthView(1f);
        currentWaypointIndex = 0;
        StartMoving();
    }

    protected virtual void StartMoving()
    {
        GameExecutor.Instance.WaitUtil(() => gameObject.activeInHierarchy,
            () => { followCoroutine = StartCoroutine(FollowPath()); });
    }

    protected virtual void StopMoving()
    {
        StopCoroutine(followCoroutine);
        followCoroutine = null;
        isMoving = false;
    }

    protected IEnumerator FollowPath()
    {
        transform.position = (Vector2) WalkablePathInfo.pathNodesCoordinate.FirstOrDefault();
        for (int i = currentWaypointIndex; i < WalkablePathInfo.pathNodesCoordinate.Count; i++)
        {
            isMoving = true;
            currentWaypointIndex = i;
            PathProgress = currentWaypointIndex / (float) WalkablePathInfo.pathNodesCoordinate.Count;
            Vector2 startPosition = transform.position;
            Vector2Int endPosition = WalkablePathInfo.pathNodesCoordinate[i];
            float travelPercent = 0f;

            var direction = (endPosition - startPosition).normalized;
            EnemyRendererTransform.LookAt2DCoordinate(direction);

            while (travelPercent < 1f && isMoving)
            {
                travelPercent += Time.deltaTime * EnemySpeed;
                transform.position = Vector2.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        OnFinishPath();
    }

    protected virtual void OnFinishPath()
    {
        GamePlayModel.Instance.DecreaseLife(1);
        EnemySpawner.Instance.DespawnEnemy(this);
    }

    protected virtual void HandleTakeDamage(int healthRemain)
    {
        HealthRenderer.UpdateHealthView((float)healthRemain / Health.MaxHealth);
    }

    protected virtual void HandleDeath()
    {
        GamePlayModel.Instance.AddGoldKillEnemy(GoldEarn);
        GameExecutor.Instance.WaitNewFrame(() => { EnemySpawner.Instance.DespawnEnemy(this); });
    }
}