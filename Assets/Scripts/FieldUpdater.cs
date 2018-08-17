using System.Collections.Generic;
using UnityEngine;

public interface IFieldUpdater
{
    IAction UpdateField();
}

public class FieldUpdater : IFieldUpdater
{
    private IMatrixStorageSwappable<IGameObjectable> fieldMatrix;
    private ICombinationsFinder combinationsFinder;
    private IFieldObjectCreator fieldObjectCreator;
    private IFieldObjectRemover fieldObjectRemover;
    private ICoroutinePlayer coroutinePlayer;
    private FieldProperties fieldProperties;
    private float fieldObjectFallingStartSpeed;
    private float fieldObjectFallingVelocity;

    public FieldUpdater(IMatrixStorageSwappable<IGameObjectable> fieldMatrix, ICombinationsFinder combinationsFinder, IFieldObjectCreator fieldObjectCreator, 
        IFieldObjectRemover fieldObjectRemover, ICoroutinePlayer coroutinePlayer, FieldProperties fieldProperties, float fieldObjectFallingStartSpeed, float fieldObjectFallingVelocity)
    {
        this.fieldMatrix = fieldMatrix;
        this.combinationsFinder = combinationsFinder;
        this.fieldObjectCreator = fieldObjectCreator;
        this.fieldObjectRemover = fieldObjectRemover;
        this.coroutinePlayer = coroutinePlayer;
        this.fieldProperties = fieldProperties;
        this.fieldObjectFallingStartSpeed = fieldObjectFallingStartSpeed;
        this.fieldObjectFallingVelocity = fieldObjectFallingVelocity;
    }

    public IAction UpdateField()
    {
        ActionSequence updateActionSequence = new ActionSequence();

        List<RectInt> currentCombinations = combinationsFinder.GetCurrentCombinations();
        while(currentCombinations.Count > 0)
        {
            IAction removalAction = RemoveCombinations(currentCombinations);
            IAction fillingEmptyCellsAction = CreateNewFieldObjectsAndFillEmptyCells();

            ActionSequence updateIterationActionSequence = new ActionSequence();
            updateIterationActionSequence.AddAction((callback) => { removalAction.Play(callback); });
            updateIterationActionSequence.AddAction((callback) => { fillingEmptyCellsAction.Play(callback); });

            updateActionSequence.AddAction((callback) => { updateIterationActionSequence.Play(callback); });

            currentCombinations = combinationsFinder.GetCurrentCombinations();
        }

        return updateActionSequence;
    }

    private IAction RemoveCombinations(List<RectInt> combinations)
    {
        ActionPack removalActions = new ActionPack();

        foreach (RectInt combination in combinations)
        {
            for (int i = combination.position.x; i < combination.position.x + combination.size.x; i++)
            {
                for (int j = combination.position.y; j < combination.position.y + combination.size.y; j++)
                {
                    Vector2Int currentCell = new Vector2Int(i, j);
                    if (fieldMatrix.GetObjectFromStorage(currentCell) != null) removalActions.AddAction(fieldObjectRemover.RemoveFieldObject(currentCell));
                }
            }
        }

        return removalActions;
    }

    private IAction CreateNewFieldObjectsAndFillEmptyCells()
    {
        ActionPack updateActions = new ActionPack();

        for (int i = 0; i < fieldProperties.GetFieldSize().x; i++)
        {
            int emptyCellsCount = 0;
            for (int j = 0; j < fieldProperties.GetFieldSize().y; j++)
            {
                if (fieldMatrix.GetObjectFromStorage(i, j) == null)
                {
                    emptyCellsCount++;
                }
                else if (emptyCellsCount > 0)
                {
                    Vector2Int newCellForCurrentObject = new Vector2Int(i, j - emptyCellsCount);
                    Vector2Int currentCell = new Vector2Int(i, j);

                    Vector2 newPositionForCurrentObject = fieldProperties.CalculateLocalFieldObjectPosition(newCellForCurrentObject);
                    Vector2 currentPosition = fieldProperties.CalculateLocalFieldObjectPosition(currentCell);

                    GameObject targetObject = fieldMatrix.GetObjectFromStorage(i, j).GetGameObject();

                    fieldMatrix.SwapObjectsInStorage(currentCell, newCellForCurrentObject);

                    updateActions.AddAction((callback) => { coroutinePlayer.PlayCoroutine(CoroutineAnimations.AcceleratedMovement(targetObject, currentPosition, newPositionForCurrentObject, fieldObjectFallingStartSpeed, fieldObjectFallingVelocity, callback)); });
                }
            }

            for (int j = fieldProperties.GetFieldSize().y - emptyCellsCount; j < fieldProperties.GetFieldSize().y; j++)
            {
                Vector2Int imaginaryCell = new Vector2Int(i, j + emptyCellsCount);
                Vector2Int actualCell = new Vector2Int(i, j);

                Vector2 positionToCreateObject = fieldProperties.CalculateLocalFieldObjectPosition(imaginaryCell);
                Vector2 positionToMoveObject = fieldProperties.CalculateLocalFieldObjectPosition(actualCell);

                fieldObjectCreator.CreateFieldObject(actualCell, positionToCreateObject);

                GameObject targetObject = fieldMatrix.GetObjectFromStorage(i, j).GetGameObject();

                updateActions.AddAction((callback) => { coroutinePlayer.PlayCoroutine(CoroutineAnimations.AcceleratedMovement(targetObject, positionToCreateObject, positionToMoveObject, fieldObjectFallingStartSpeed, fieldObjectFallingVelocity, callback)); });
            }
        }

        return updateActions;
    }
}