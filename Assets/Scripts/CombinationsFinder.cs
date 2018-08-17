using System.Collections.Generic;
using UnityEngine;

public interface ICombinationsFinder
{
    List<RectInt> GetCurrentCombinations();
}

public class CombinationsFinder : ICombinationsFinder
{
    private IMatrixStorageReadOnly<IComparable> fieldMatrix;

    public CombinationsFinder(IMatrixStorageReadOnly<IComparable> fieldMatrix)
    {
        this.fieldMatrix = fieldMatrix;
    }

    public List<RectInt> GetCurrentCombinations()
    {
        List<RectInt> foundCombinations = new List<RectInt>();

        for (int i = 0; i < fieldMatrix.Size.x; i++)
        {
            int equalObjectsCount = 0;
            for (int j = 0; j < fieldMatrix.Size.y - 1; j++)
            {
                if (fieldMatrix.GetObjectFromStorage(i, j) != null && fieldMatrix.GetObjectFromStorage(i, j + 1) != null && fieldMatrix.GetObjectFromStorage(i, j).EqualsTo(fieldMatrix.GetObjectFromStorage(i, j + 1)))
                {
                    equalObjectsCount++;
                }
                else
                {
                    if (equalObjectsCount > 1)
                    {
                        foundCombinations.Add(new RectInt(new Vector2Int(i, j - equalObjectsCount), new Vector2Int(1, equalObjectsCount + 1)));
                    }
                    equalObjectsCount = 0;
                }
            }

            if (equalObjectsCount > 1)
            {
                foundCombinations.Add(new RectInt(new Vector2Int(i, fieldMatrix.Size.y - 1 - equalObjectsCount), new Vector2Int(1, equalObjectsCount + 1)));
            }
        }

        for (int j = 0; j < fieldMatrix.Size.y; j++)
        {
            int equalObjectsCount = 0;
            for (int i = 0; i < fieldMatrix.Size.x - 1; i++)
            {
                if (fieldMatrix.GetObjectFromStorage(i, j) != null && fieldMatrix.GetObjectFromStorage(i + 1, j) != null && fieldMatrix.GetObjectFromStorage(i, j).EqualsTo(fieldMatrix.GetObjectFromStorage(i + 1, j)))
                {
                    equalObjectsCount++;
                }
                else
                {
                    if (equalObjectsCount > 1)
                    {
                        foundCombinations.Add(new RectInt(new Vector2Int(i - equalObjectsCount, j), new Vector2Int(equalObjectsCount + 1, 1)));
                    }
                    equalObjectsCount = 0;
                }
            }

            if (equalObjectsCount > 1)
            {
                foundCombinations.Add(new RectInt(new Vector2Int(fieldMatrix.Size.x - 1 - equalObjectsCount, j), new Vector2Int(equalObjectsCount + 1, 1)));
            }
        }

        return foundCombinations;
    }
}
