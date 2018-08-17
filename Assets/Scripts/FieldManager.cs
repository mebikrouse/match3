using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldManager : MonoBehaviour
{
    [SerializeField]
    private Vector2Int fieldSize;
    [SerializeField]
    private Vector2 fieldScale;
    [SerializeField]
    private Transform fieldContainer;

    [SerializeField]
    private Controller controller;

    [SerializeField]
    private GameObject fieldObjectMarkerGameObject;
    [SerializeField]
    private Transform fieldObjectMarkerContainer;

    [SerializeField]
    private CoroutinePlayer coroutinePlayer;

    [SerializeField]
    private float swapDuration;
    [SerializeField]
    private float fieldObjectFallingStartSpeed;
    [SerializeField]
    private float fieldObjectFallingVelocity;

    [SerializeField]
    private GameObject[] fieldObjects;

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void FillFieldWithObjects(IFieldObjectCreator fieldObjectCreator)
    {
        for (int i = 0; i < fieldSize.x; i++)
        {
            for(int j = 0; j < fieldSize.y; j++)
            {
                Vector2Int currentCell = new Vector2Int(i, j);
                fieldObjectCreator.CreateFieldObject(currentCell);
            }
        }
    }

    private void Awake()
    {
        MatrixStorage<FieldObject> fieldMatrix = new MatrixStorage<FieldObject>(fieldSize);

        FieldProperties fieldProperties = new FieldProperties(fieldContainer, fieldSize, fieldScale);
        ICombinationsFinder combinationsFinder = new CombinationsFinder(fieldMatrix);

        IFieldObjectMarker fieldObjectMarker = new FieldObjectMarker(fieldObjectMarkerGameObject, fieldObjectMarkerContainer, fieldProperties);
        IFieldObjectSwapper fieldObjectSwapper = new FieldObjectSwapper(fieldMatrix, fieldProperties, coroutinePlayer, swapDuration);
        IFieldObjectCreator fieldObjectCreator = new FieldObjectCreator(fieldMatrix, fieldProperties, fieldObjects);
        IFieldObjectRemover fieldObjectRemover = new FieldObjectRemover(fieldMatrix);
        IFieldUpdater fieldUpdater = new FieldUpdater(fieldMatrix, combinationsFinder, fieldObjectCreator, fieldObjectRemover, coroutinePlayer, 
            fieldProperties, fieldObjectFallingStartSpeed, fieldObjectFallingVelocity);

        controller.InitializeController(fieldProperties, fieldObjectMarker, fieldObjectSwapper, fieldUpdater, combinationsFinder);

        FillFieldWithObjects(fieldObjectCreator);
    }
}