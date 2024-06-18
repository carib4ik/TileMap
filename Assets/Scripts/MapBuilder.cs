using System.Linq;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private Grid _grid;

    // Поля для хранения цветов выделения тайла
    [SerializeField] private Color _highlightingColorRed;
    [SerializeField] private Color _highlightingColorGreen;

    private GameObject _tile;

    // массив равный игровому полю для постройки, чтобы отмечать уже занятые ячейки
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

        // Получаем список рендеров всех частей тайла
        _renderers = _tile.GetComponentsInChildren<Renderer>();

        // Сохраняем цвета частей тайла в отдельный список
        SaveTileOriginalColors();
    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Получаем координату куда указывает курсор на поле
        if (Physics.Raycast(ray, out var hitInfo))
        {
            var worldPosition = hitInfo.point;
            var cellPosition = _grid.WorldToCell(worldPosition);
            var cellCenterPosition = _grid.GetCellCenterWorld(cellPosition);

            // если тайл выбран
            if (_tile != null)
            {
                // постоянно меняем позицию тайла, чтобы он был равен позиции курсора
                _tile.transform.position = cellCenterPosition;

                // Красим тайл в соответсвии с возможностью построить
                // Проверяем, чтобы координаты были внутри поля и что ячейка ещё не занята
                if (cellPosition.x < 10 && cellPosition.x >= 0 && cellPosition.z >= 0 && cellPosition.z < 10 &&
                    !_areObjectsInCells[cellPosition.x, cellPosition.z])
                {
                    PaintTailToGreen();

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        PaintTailToOrigin();

                        // Открепляем ссылку на тайл, чтобы он оставался на выбранном месте
                        _tile = null;

                        // Отмечаем, что ячейка занята
                        _areObjectsInCells[cellPosition.x, cellPosition.z] = true;
                    }
                }
                else
                {
                    PaintTailToRed();
                }
            }
        }
    }

    private void PaintTailToRed()
    {
        // Подсвечиваем все части тайла красным
        foreach (var rend in _renderers)
        {
            rend.material.color = _highlightingColorRed;
        }
    }
    
    private void PaintTailToGreen()
    {
        // Подсвечиваем все части тайла зеленым
        foreach (var rend in _renderers)
        {
            rend.material.color = _highlightingColorGreen;
        }
    }
    
    private void PaintTailToOrigin()
    {
        // Возвращаем всем частям тайла оригинальный цвет
        for (var i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color = _originalColors[i];
        }
    }

    private void SaveTileOriginalColors()
    {
        _originalColors = new Color[_renderers.Length];
        _originalColors = _renderers.Select(rend => rend.material.color).ToArray();
    }
}