using System;
using System.Linq;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    
    [SerializeField] private Color _highlightingColorRed;
    [SerializeField] private Color _highlightingColorGreen;

    private GameObject _tile;
    private bool[,] _areObjectsInCells = new bool[10, 10];

    private Renderer[] _renderers;
    private Color[] _originalColors;

    /// <summary>
    /// Данный метод вызывается автоматически при клике на кнопки с изображениями тайлов.
    /// В качестве параметра передается префаб тайла, изображенный на кнопке.
    /// Вы можете использовать префаб tilePrefab внутри данного метода.
    /// </summary>
    public void StartPlacingTile(GameObject tilePrefab)
    {
        _tile = Instantiate(tilePrefab);

        _renderers = _tile.GetComponentsInChildren<Renderer>();

        SaveTileOriginalColors();
    }

    private void Update()
    {
        var mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cellPosition = _grid.WorldToCell(mouseWorldPosition);
        var cellCenterPosition = _grid.GetCellCenterWorld(cellPosition);

        // Debug.Log(cellPosition.ToString());


        if (_tile != null)
        {
            _tile.transform.position = cellCenterPosition;

            if (cellPosition.x < 10 && cellPosition.x >= 0 && cellPosition.z >= 0 && cellPosition.z < 10 &&
                !_areObjectsInCells[cellPosition.x, cellPosition.z])
            {
                foreach (var rend in _renderers)
                {
                    rend.material.color = _highlightingColorGreen;
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    for (var i = 0; i < _renderers.Length; i++)
                    {
                        _renderers[i].material.color = _originalColors[i];
                    }
                    
                    // Открепляем ссылку на тайл, чтобы он оставался на выбранном месте
                    _tile = null;
                    
                    // Отмечаем, что ячейка занята
                    _areObjectsInCells[cellPosition.x, cellPosition.z] = true;
                }
            }
            else
            {
                foreach (var rend in _renderers)
                {
                    rend.material.color = _highlightingColorRed;
                }
            }
        }
    }

    private void ChoosePlaceForTile()
    {
        
    }

    private void SaveTileOriginalColors()
    {
        _originalColors = new Color[_renderers.Length];
        _originalColors = _renderers.Select(rend => rend.material.color).ToArray();
    }
}