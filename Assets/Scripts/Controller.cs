using UnityEngine;

public interface IController
{
    void DisableUserInput();
    void EnableUserInput();
    bool IsUserInputEnabled();
}

[RequireComponent(typeof(BoxCollider2D))]
public class Controller : MonoBehaviour, IController
{
    private FieldProperties fieldProperties;
    private IFieldObjectMarker fieldObjectMarker;
    private IFieldObjectSwapper fieldObjectSwapper;
    private IFieldUpdater fieldUpdater;
    private ICombinationsFinder combinationsFinder;

    private BoxCollider2D inputCollider;

    public void InitializeController(FieldProperties fieldProperties, IFieldObjectMarker fieldObjectMarker, IFieldObjectSwapper fieldObjectSwapper, 
        IFieldUpdater fieldUpdater, ICombinationsFinder combinationsFinder)
    {
        this.fieldProperties = fieldProperties;
        this.fieldObjectMarker = fieldObjectMarker;
        this.fieldObjectSwapper = fieldObjectSwapper;
        this.fieldUpdater = fieldUpdater;
        this.combinationsFinder = combinationsFinder;
    }

    public void DisableUserInput()
    {
        inputCollider.enabled = false;
    }

    public void EnableUserInput()
    {
        inputCollider.enabled = true;
    }

    public bool IsUserInputEnabled()
    {
        return inputCollider.enabled;
    }

    private Vector2 CalculateMousePositionOnField()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition) - fieldProperties.GetFieldContainer().transform.position;
    }

    private bool AreSelectedCellsAdjacent(Vector2Int aCell, Vector2Int bCell)
    {
        return (aCell.x == bCell.x && Mathf.Abs(aCell.y - bCell.y) == 1) || (aCell.y == bCell.y && Mathf.Abs(aCell.x - bCell.x) == 1);
    }

    private void Awake()
    {
        inputCollider = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        Vector2 mouseToWorldPosition = CalculateMousePositionOnField();
        Vector2Int currentlyClickedCell = fieldProperties.CalculateCellFromLocalPosition(mouseToWorldPosition);

        if(fieldObjectMarker.IsAnyFieldObjectMarked())
        {
            Vector2Int previouslyClickedCell = fieldObjectMarker.GetMarkedFieldObjectCell();
            fieldObjectMarker.RemoveMark();

            if(AreSelectedCellsAdjacent(currentlyClickedCell, previouslyClickedCell))
            {
                DisableUserInput();

                ActionSequence fieldUpdateActionSequence = new ActionSequence();

                IAction swapAnimationActionPack = fieldObjectSwapper.SwapFieldObjects(currentlyClickedCell, previouslyClickedCell);
                fieldUpdateActionSequence.AddAction(swapAnimationActionPack);

                if(combinationsFinder.GetCurrentCombinations().Count > 0)
                {
                    IAction fieldUpdateActionPack = fieldUpdater.UpdateField();
                    fieldUpdateActionSequence.AddAction(fieldUpdateActionPack);
                }
                else
                {
                    IAction swapBackAnimationActionPack = fieldObjectSwapper.SwapFieldObjects(currentlyClickedCell, previouslyClickedCell);
                    fieldUpdateActionSequence.AddAction(swapBackAnimationActionPack);
                }
                
                fieldUpdateActionSequence.AddAction((callback) => { EnableUserInput(); callback(); });

                fieldUpdateActionSequence.Play(null);
            }
            else if(currentlyClickedCell != previouslyClickedCell)
            {
                fieldObjectMarker.MarkFieldObject(currentlyClickedCell);
            }
        }
        else
        {
            fieldObjectMarker.MarkFieldObject(currentlyClickedCell);
        }
    }
}