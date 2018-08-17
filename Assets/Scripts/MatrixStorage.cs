using UnityEngine;

public interface IMatrixStorageReadOnly<out T>
{
    Vector2Int Size { get; }

    T GetObjectFromStorage(Vector2Int cell);
    T GetObjectFromStorage(int x, int y);
}

public interface IMatrixStorageSwappable<out T> : IMatrixStorageReadOnly<T>
{
    void SwapObjectsInStorage(Vector2Int aCell, Vector2Int bCell);
}

public interface IMatrixStorageSetToDefault<out T> : IMatrixStorageReadOnly<T>
{
    void SetObjectToDefault(Vector2Int cell);
}

public interface IMatrixStorage<T> : IMatrixStorageReadOnly<T>
{
    void SetObjectInStorage(Vector2Int cell, T obj);
    void SetObjectInStorage(int x, int y, T obj);
}

public class MatrixStorage<T> : IMatrixStorage<T>, IMatrixStorageSwappable<T>, IMatrixStorageSetToDefault<T>
{
    private T[,] matrix;
    private Vector2Int matrixSize;

    public Vector2Int Size
    {
        get
        {
            return matrixSize;
        }
    }

    public MatrixStorage(Vector2Int matrixSize)
    {
        this.matrixSize = matrixSize;
        matrix = new T[matrixSize.x, matrixSize.y];
    }

    public T GetObjectFromStorage(Vector2Int cell)
    {
        return GetObjectFromStorage(cell.x, cell.y);
    }

    public T GetObjectFromStorage(int x, int y)
    {
        return matrix[x, y];
    }

    public void SwapObjectsInStorage(Vector2Int aCell, Vector2Int bCell)
    {
        T temp = GetObjectFromStorage(aCell);
        SetObjectInStorage(aCell, GetObjectFromStorage(bCell));
        SetObjectInStorage(bCell, temp);
    }

    public void SetObjectInStorage(Vector2Int cell, T obj)
    {
        SetObjectInStorage(cell.x, cell.y, obj);
    }

    public void SetObjectInStorage(int x, int y, T obj)
    {
        matrix[x, y] = obj;
    }

    public void SetObjectToDefault(Vector2Int cell)
    {
        matrix[cell.x, cell.y] = default(T);
    }
}
