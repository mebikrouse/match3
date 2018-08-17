using UnityEngine;

public interface IFieldObjectMarker
{
    void MarkFieldObject(Vector2Int targetObjectCell);
    void RemoveMark();
    bool IsAnyFieldObjectMarked();
    Vector2Int GetMarkedFieldObjectCell();
}

public class FieldObjectMarker : IFieldObjectMarker
{
    private GameObject mark;
    private Transform markParentTransform;
    private FieldProperties fieldProperties;

    private Vector2Int currentlyMarkedCell;
    private bool isAnyFieldObjectMarked;

    public FieldObjectMarker(GameObject mark, Transform markParentTransform, FieldProperties fieldProperties)
    {
        this.mark = mark;
        this.markParentTransform = markParentTransform;
        this.fieldProperties = fieldProperties;

        InitializeMark();
    }

    public void MarkFieldObject(Vector2Int targetObjectCell)
    {
        mark.transform.localPosition = fieldProperties.CalculateLocalFieldObjectPosition(targetObjectCell);
        mark.SetActive(true);

        currentlyMarkedCell = targetObjectCell;
        isAnyFieldObjectMarked = true;
    }

    public void RemoveMark()
    {
        mark.SetActive(false);
        isAnyFieldObjectMarked = false;
    }

    public bool IsAnyFieldObjectMarked()
    {
        return isAnyFieldObjectMarked;
    }

    public Vector2Int GetMarkedFieldObjectCell()
    {
        if (!isAnyFieldObjectMarked) throw new System.Exception("There are no marked field objects");

        return currentlyMarkedCell;
    }

    private void InitializeMark()
    {
        RemoveMark();

        markParentTransform.position = fieldProperties.GetFieldContainer().position + new Vector3(0, 0, -1);
        mark.transform.parent = markParentTransform;
    }
}