using UnityEngine;

public interface IFieldObjectCreator
{
    void CreateFieldObject(Vector2Int cell);
    void CreateFieldObject(Vector2Int cell, Vector2 position);
}

public class FieldObjectCreator : IFieldObjectCreator
{
    private IMatrixStorage<FieldObject> fieldMatrix;
    private FieldProperties fieldProperties;
    private GameObject[] fieldObjects;

    public FieldObjectCreator(IMatrixStorage<FieldObject> fieldMatrix, FieldProperties fieldProperties, GameObject[] fieldObjects)
    {
        this.fieldMatrix = fieldMatrix;
        this.fieldProperties = fieldProperties;
        this.fieldObjects = fieldObjects;
    }

    public void CreateFieldObject(Vector2Int cell)
    {
        Vector2 actualPosition = fieldProperties.CalculateLocalFieldObjectPosition(cell);
        CreateFieldObject(cell, actualPosition);
    }

    public void CreateFieldObject(Vector2Int cell, Vector2 position)
    {
        GameObject createdObject = GameObject.Instantiate(SelectFieldObjectPrefab(fieldObjects));

        createdObject.transform.parent = fieldProperties.GetFieldContainer();
        createdObject.transform.localPosition = position;
        createdObject.transform.localRotation = Quaternion.Euler(Vector3.zero);

        fieldMatrix.SetObjectInStorage(cell, createdObject.GetComponent<FieldObject>());
    }

    private GameObject SelectFieldObjectPrefab(GameObject[] prefabs)
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }
}
