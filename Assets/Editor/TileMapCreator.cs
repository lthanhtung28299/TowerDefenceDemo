using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(TileMapCreator))]
public class TileMapCreator : EditorWindow
{
    private Vector2Int _mapSize;
    private string _gridMapId;
    private bool _editGridNodeModeActive;
    private readonly string[] _mainTabBar = {"TileMapManager", "TileNodeMapEdit"};
    private readonly string[] _toolbarString = {"SetSelectNode", "SetDeselectNode"};

    private Ray _ray;
    private Vector3 _mousePos;
    private Vector2 _hitPos;
    private int _toolbarInt = 0;
    private int _toolbarIntEdit = 0;

    private List<Node> _nodesSelected = new List<Node>();
    private GridManager GridManager => GridManager.Instance;

    [MenuItem("Tools/TileMapCreator")]
    public static void ShowWindow()
    {
        TileMapCreator window = GetWindow<TileMapCreator>();
        window.titleContent = new GUIContent("TileMapCreator");
    }

    public void OnGUI()
    {
        EditorGUILayout.LabelField($"Chose Tab:");
        _toolbarInt = GUILayout.Toolbar(_toolbarInt, _mainTabBar, GUILayout.Height(30));

        switch (_toolbarInt)
        {
            case 0:
                //Hiển thị các function để tạo, lưu trữ, load dữ liệu trong GridDataSo
                ShowTileMapManagerGUI();
                break;
            case 1:
                //Sau khi khởi tạo grid map có thể chuyển sang tab này để tùy chỉnh và gắn dữ liệu các node
                ShowTileNodeMapEditGUI();
                break;
        }
    }

    private void ShowTileMapManagerGUI()
    {
        _mapSize = EditorGUILayout.Vector2IntField("Map Size:", _mapSize, GUILayout.ExpandWidth(true));
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Grid Map ID:");
        _gridMapId = EditorGUILayout.TextField(_gridMapId);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        if (GUILayout.Button("Load Grid Map With Id"))
        {
            LoadGridMap();
        }

        if (GUILayout.Button("Save Grid Map With Id"))
        {
            SaveGridMap();
        }

        if (GUILayout.Button("Create Grid Map"))
        {
            CreateGridMap();
        }

        if (GUILayout.Button("Clear Grid Map"))
        {
            ClearGridMap();
        }
    }
    
    private Vector2 _scrollPosition;

    private void ShowTileNodeMapEditGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Edit Grid Map Active:");
        _editGridNodeModeActive = EditorGUILayout.Toggle(_editGridNodeModeActive);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Edit action:");
        _toolbarIntEdit = GUILayout.Toolbar(_toolbarIntEdit, _toolbarString, GUILayout.Height(30));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        if (GUILayout.Button("Add Walkable Paths"))
        {
            AddWalkablePath();
        }

        if (GUILayout.Button("Set Build Node"))
        {
            SetNodeCanBuild();
        }

        if (GUILayout.Button("Clear Tile Nodes Selected"))
        {
            ClearSelectTileNode();
        }
        EditorGUILayout.Separator();
        if (_nodesSelected.Count > 0)
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach (var node in _nodesSelected)
            {
                if(node == null) continue;
                EditorGUILayout.LabelField($"Path: {node.coordinates}");
            }
            EditorGUILayout.EndScrollView();
        }
    }


    private void CreateGridMap()
    {
        GridManager.CreateGrid(_mapSize);
    }

    private void ClearGridMap()
    {
        GridManager.ClearGridMap();
    }

    private void SaveGridMap()
    {
        GridManager.SaveGridMapToSo(_gridMapId);
    }

    private void LoadGridMap()
    {
        GridManager.LoadGridMap(_gridMapId);
    }

    private void OnSelectNode(Vector2Int pos)
    {
        bool setSelect = _toolbarIntEdit == 0;
        var nodeSelected = setSelect ? GridManager.SelectTileNode(pos) : GridManager.DeselectTileNode(pos);
        if (nodeSelected == null) return;
        switch (setSelect)
        {
            case true when !_nodesSelected.Contains(nodeSelected):
                _nodesSelected.Add(nodeSelected);
                break;
            case false when _nodesSelected.Contains(nodeSelected):
                _nodesSelected.Remove(nodeSelected);
                break;
        }

        Repaint();
    }

    private void AddWalkablePath()
    {
        //Sau khi chọn nhiều node đã được bôi xanh sẽ add toàn bộ node thành 1 đường enemy có thể đi theo (nên bôi xanh theo thứ tự enemy di chuyển qua các ô)
        if (_nodesSelected is {Count: 0}) return;
        var walkablePathInfo = new WalkablePathInfo(GridManager.GetListWalkablePathFromNodes(_nodesSelected));
        GridManager.AddWalkablePath(walkablePathInfo);
        ClearSelectTileNode();
        Repaint();
    }

    private void SetNodeCanBuild()
    {
        //Sau khi chọn nhiều node được bôi xanh sẽ set các ô được chọn có thể xây nhà
        if (_nodesSelected is {Count: 0}) return;
        foreach (var node in _nodesSelected)
        {
            GridManager.SetCanBuildNode(node.coordinates);
        }
        ClearSelectTileNode();
        Repaint();
    }

    private void ClearSelectTileNode()
    {
        //Bỏ chọn toàn bộ node đã chọn
        foreach (var node in _nodesSelected)
        {
            GridManager.DeselectTileNode(node.coordinates);
        }
        _nodesSelected.Clear();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnScene;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnScene;
    }

    void OnScene(SceneView scene)
    {
        if (this != null && _editGridNodeModeActive)
        {
            Event e = Event.current;
            _mousePos = e.mousePosition;
            
            //Tính toán vị trí chuột trên camera scene view tới vị trí thế giới
            float ppp = EditorGUIUtility.pixelsPerPoint;
            _mousePos.y = scene.camera.pixelHeight - _mousePos.y * ppp;
            _mousePos.x *= ppp;
            _mousePos.z = 0;
            _ray = scene.camera.ScreenPointToRay(_mousePos);
            
            //Làm tròn số gần nhất để lấy được tọa độ grid mong muốn
            var gridPos = Vector2Int.RoundToInt(_ray.origin);
            
            //Nhấn chuột phải hoặc giữ phím N di rồi di chuyển chuột sẽ chạy các action ở dưới
            if ((e.type == EventType.MouseDown && e.button == 1) || (e.type == EventType.KeyDown && e.keyCode == KeyCode.N))
            {
                // Thêm Node được chọn vào list node rồi chạy function AddWalkablePath() để thêm đường chỉ định có thể đi của enemy
                OnSelectNode(gridPos);
            }

            if (e.type != EventType.KeyDown) return;
            switch (e.keyCode)
            {
                case KeyCode.A:
                    //khi đang bật editMode nhấn phím tắt A để chuyển sang Add
                    _toolbarIntEdit = 0;
                    break;
                case KeyCode.R:
                    //khi đang bật editMode nhấn phím tắt R để chuyển sang Remove
                    _toolbarIntEdit = 1;
                    break;
            }
            Repaint();
        }
    }
}