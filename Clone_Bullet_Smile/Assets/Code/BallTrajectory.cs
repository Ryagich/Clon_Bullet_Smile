using UnityEngine;
using UnityEngine.SceneManagement;

public class BallTrajectory : MonoBehaviour
{
    [SerializeField] private Transform _obstacles;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _maxPhysicsFrameIterations = 100;
    [SerializeField] private GameObject _predict;
    [SerializeField] private Movement _movement;
    [SerializeField] private MovementCalculations _calculations;

    private Scene simulationScene;
    private PhysicsScene physicsScene;
    private bool state;
    private Vector3[] linePositions;

    private void Start()
    {
        CreatePhysicsScene();
        linePositions = new Vector3[_maxPhysicsFrameIterations];
        _line.positionCount = linePositions.Length;
    }

    public void SetEnabledState(bool newState)
    {
        state = newState;
        _line.enabled = newState;
    }

    private void Update()
    {
        if (state)
        {
            SimulateTrajectory();
        }
    }

    private void CreatePhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        physicsScene = simulationScene.GetPhysicsScene();
        
        foreach (Transform obj in _obstacles)
        {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
        }
    }

    private void SimulateTrajectory()
    {
        if (_line.enabled == false)
        {
            _line.enabled = true;
        }

        var predict = Instantiate(_predict, transform.position, transform.rotation);
        var predictRb = predict.GetComponent<Rigidbody>();

        SceneManager.MoveGameObjectToScene(predict.gameObject, simulationScene);
        _calculations.Move(predictRb, _movement.StartPosition, _calculations.GetMouseScreenPosition());
        for (var i = 0; i < _maxPhysicsFrameIterations; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);
            linePositions[i] = predict.transform.position;            
        }

        _line.SetPositions(linePositions);
        Destroy(predict.gameObject);
    }
}