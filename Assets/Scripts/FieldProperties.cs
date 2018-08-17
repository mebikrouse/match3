using UnityEngine;

public class FieldProperties
{
    private Transform fieldContainer;
    private Vector2Int fieldSize;
    private Vector2 fieldScale;

    public FieldProperties(Transform fieldContainer, Vector2Int fieldSize, Vector2 fieldScale)
    {
        this.fieldContainer = fieldContainer;
        this.fieldSize = fieldSize;
        this.fieldScale = fieldScale;
    }

    public Transform GetFieldContainer()
    {
        return fieldContainer;
    }

    public Vector2Int GetFieldSize()
    {
        return fieldSize;
    }

    public Vector2 GetFieldScale()
    {
        return fieldScale;
    }

    public Vector2 CalculateLocalFieldObjectPosition(Vector2Int cell)
    {
        return new Vector2(cell.x * fieldScale.x, cell.y * fieldScale.y);
    }

    public Vector2Int CalculateCellFromLocalPosition(Vector2 position)
    {
        position += fieldScale / 2f;
        return new Vector2Int(Mathf.FloorToInt(position.x / fieldScale.x), Mathf.FloorToInt(position.y / fieldScale.y));
    }
}