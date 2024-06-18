using System;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    
    private GameObject _tile;
    
    /// <summary>
    /// Данный метод вызывается автоматически при клике на кнопки с изображениями тайлов.
    /// В качестве параметра передается префаб тайла, изображенный на кнопке.
    /// Вы можете использовать префаб tilePrefab внутри данного метода.
    /// </summary>
    public void StartPlacingTile(GameObject tilePrefab)
    {
        _tile = Instantiate(tilePrefab);
    }
    
    private void Update()
    {
        // var mouseScreenPosition = Input.mousePosition;
        // mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        // var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        var cellPosition = _grid.WorldToCell(mousePosition);
        var cellCenterPosition = _grid.GetCellCenterWorld(cellPosition);

        if (_tile != null)
        {
            _tile.transform.position = cellCenterPosition;
        }
    }
}