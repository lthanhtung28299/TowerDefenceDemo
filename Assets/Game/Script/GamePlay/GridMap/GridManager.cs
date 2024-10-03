using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoSingleton<GridManager>
{
    [field: SerializeField] public Vector2Int GridSize { get; private set; }
    [SerializeField] private TileNode prefabTileNode;
    [SerializeField] private bool isTest;
    public Dictionary<Vector2Int, TileNode> Grid { get; private set; } = new Dictionary<Vector2Int, TileNode>();
    public List<WalkablePathInfo> WalkablePath { get; private set; } = new List<WalkablePathInfo>();
    
    public TileNode GetTileNode(Vector2Int position)
    {
        return Grid.GetValueOrDefault(position);
    }

    public Node SelectTileNode(Vector2Int position)
    {
        if(!Grid.ContainsKey(position)) return null;
        Grid[position].SelectNode(true);
        return Grid[position].Node;
    }

    public Node DeselectTileNode(Vector2Int position)
    {
        if(!Grid.ContainsKey(position)) return null;
        Grid[position].SelectNode(false);
        return Grid[position].Node;
    }

    public void SetCanBuildNode(Vector2Int pos)
    {
        if(!Grid.ContainsKey(pos)) return;
        Grid[pos].SetNodeCanBuild();
    }
    
    public List<Vector2Int> GetListWalkablePathFromNodes(List<Node> tileNodes)
    {
        List<Vector2Int> pathResult = new List<Vector2Int>();
        foreach (var node in tileNodes)
        {
            DeselectTileNode(node.coordinates);
            pathResult.Add(node.coordinates);
        }
            
        return pathResult;
    }

    public void AddWalkablePath(WalkablePathInfo walkablePath)
    {
        WalkablePath.Add(new WalkablePathInfo(walkablePath));
    }

    public List<Node> GetNodesFromTileNodes()
    {
        List<Node> resultNodes = new List<Node>();
        foreach (var tileNode in Grid)
        {
            resultNodes.Add(new Node(tileNode.Value.Node));
        }

        return resultNodes;
    }

    public void LoadGridMap(string gridMapId)
    {
        // Load dữ liệu map đã tạo sẵn
        var gridMapSaved = GridMapModel.Instance.MapInfos.Find(gridMapInfo => gridMapInfo.mapId == gridMapId);
        if (gridMapSaved == null) return;
        ClearGridMap();
        WalkablePath.Clear();
        GridSize = gridMapSaved.gridSize;
        foreach (var node in gridMapSaved.gridNodes)
        {
            SpawnTileNode(node);
        }
        foreach (var pathInfo in gridMapSaved.walkablePaths)
        {
            WalkablePath.Add(new WalkablePathInfo(pathInfo));
        }
    }

    public void SaveGridMapToSo(string mapId)
    {
        // Lưu dữ liệu vào thông qua tool TileMapCreator GridDataSo
        var savedMap = new GridMapInfo(mapId, GridSize, GetNodesFromTileNodes(), WalkablePath);
        var gridMapData = GridMapSo.GetInstance();
#if UNITY_EDITOR
        EditorUtility.SetDirty(gridMapData);
#endif
        gridMapData.SaveGridMapData(savedMap);
    }

    public void CreateGrid(Vector2Int gridSize)
    {
        //Bắt đầu khởi tạo grid map
        GridSize = gridSize;
        ClearGridMap();
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                SpawnTileNode(position);
            }
        }
    }

    public void ClearGridMap()
    {
        //Xóa grid map
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            DestroyImmediate(transform.GetChild(i).gameObject);
#else
            Destroy(transform.GetChild(i).gameObject);
#endif
        }
        WalkablePath.Clear();
        Grid.Clear();
    }

    private void SpawnTileNode(Vector2Int position)
    {
        if (Grid.ContainsKey(position)) return;
        var tileNodeSpawnGo = PoolSpawner.Instance.Spawn(prefabTileNode.gameObject, position);
        tileNodeSpawnGo.transform.SetParent(transform);
        var tileNodeSpawned = tileNodeSpawnGo.GetComponent<TileNode>();
        tileNodeSpawned.Init(new Node(position, true));
        Grid.Add(position, tileNodeSpawned);
    }
    
    private void SpawnTileNode(Node node)
    {
        // Spawn các node hiển thị
        var position = node.coordinates;
        if (Grid.ContainsKey(position)) return;
        var tileNodeSpawnGo = PoolSpawner.Instance.Spawn(prefabTileNode.gameObject, position);
        tileNodeSpawnGo.transform.SetParent(transform);
        var tileNodeSpawned = tileNodeSpawnGo.GetComponent<TileNode>();
        tileNodeSpawned.Init(node);
        Grid.Add(position, tileNodeSpawned);
    }

    public void ResetNode()
    {
        foreach (var gridNode in Grid)
        {
            gridNode.Value.ResetTileNode();
        }
    }
}