using UnityEngine;

public interface IFieldObjectSwapper
{
    IAction SwapFieldObjects(Vector2Int aCell, Vector2Int bCell);
}

public class FieldObjectSwapper : IFieldObjectSwapper
{
    private IMatrixStorageSwappable<IGameObjectable> fieldMatrix;
    private FieldProperties fieldProperties;
    private ICoroutinePlayer coroutinePlayer;
    private float swapDuration;

    public FieldObjectSwapper(IMatrixStorageSwappable<IGameObjectable> fieldMatrix, FieldProperties fieldProperties, ICoroutinePlayer coroutinePlayer, float swapDuration)
    {
        this.fieldMatrix = fieldMatrix;
        this.fieldProperties = fieldProperties;
        this.coroutinePlayer = coroutinePlayer;
        this.swapDuration = swapDuration;
    }

    public IAction SwapFieldObjects(Vector2Int aCell, Vector2Int bCell)
    {
        ActionPack swapAnimationActionPack = GetSwapAnimationActionPack(aCell, bCell);
        fieldMatrix.SwapObjectsInStorage(aCell, bCell);

        return swapAnimationActionPack;
    }

    private ActionPack GetSwapAnimationActionPack(Vector2Int aCell, Vector2Int bCell)
    {
        Vector2 aCellPosition = fieldProperties.CalculateLocalFieldObjectPosition(aCell);
        Vector2 bCellPosition = fieldProperties.CalculateLocalFieldObjectPosition(bCell);

        GameObject aObject = fieldMatrix.GetObjectFromStorage(aCell).GetGameObject();
        GameObject bObject = fieldMatrix.GetObjectFromStorage(bCell).GetGameObject();

        ActionPack swapActionPack = new ActionPack();
        swapActionPack.AddAction((callback) => { coroutinePlayer.PlayCoroutine(CoroutineAnimations.MovementWithEasingFunction(aObject, aCellPosition, bCellPosition, swapDuration, EasingFunctions.EaseOutBack, callback)); });
        swapActionPack.AddAction((callback) => { coroutinePlayer.PlayCoroutine(CoroutineAnimations.MovementWithEasingFunction(bObject, bCellPosition, aCellPosition, swapDuration, EasingFunctions.EaseOutBack, callback)); });

        return swapActionPack;
    }
}