using UnityEngine;

public interface IFieldObjectRemover
{
    IAction RemoveFieldObject(Vector2Int targetFieldObjectCell);
}

public class FieldObjectRemover : IFieldObjectRemover
{
    private IMatrixStorageSetToDefault<IDestroyable> fieldMatrix;

    public FieldObjectRemover(IMatrixStorageSetToDefault<IDestroyable> fieldMatrix)
    {
        this.fieldMatrix = fieldMatrix;
    }

    public IAction RemoveFieldObject(Vector2Int targetFieldObjectCell)
    {
        IDestroyable fieldObjectToRemove = fieldMatrix.GetObjectFromStorage(targetFieldObjectCell);
        fieldMatrix.SetObjectToDefault(targetFieldObjectCell);

        if (fieldObjectToRemove != null) return new ActionSingle((callback) => { fieldObjectToRemove.Destroy(callback); });
        else return null;
    }
}
