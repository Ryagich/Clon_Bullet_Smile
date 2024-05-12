using UnityEngine;
using UnityEngine.SceneManagement;

public class BallTrajectory : MonoBehaviour
{
    [SerializeField] private Transform obstacles;
    [SerializeField] private LineRenderer line;
    [SerializeField] private int maxPhysicsFrameIterations = 100;
    [SerializeField] private GameObject _predict;
    [SerializeField] private Movement _movement;
    [SerializeField] private MovementCalculations _calculations;
    
    private Scene simulationScene;
    private PhysicsScene physicsScene;
    private bool state;

    void Start()
    {
        CreatePhysicsScene();
    }

    public void SetEnabledState(bool newState)
    {
        state = newState;
        line.enabled = newState;
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

        foreach (Transform obj in obstacles)
        {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
        }
    }

    public void SimulateTrajectory()
    {
        if (line.enabled == false)
        {
            line.enabled = true;
        }

        var predict = Instantiate(_predict, transform.position, transform.rotation);
        var predictRb = predict.GetComponent<Rigidbody>();

        SceneManager.MoveGameObjectToScene(predict.gameObject, simulationScene);
        _calculations.Move(predictRb, _movement.startPos, _calculations.GetMouseScreenPosition());
        line.positionCount = 1;
        line.SetPosition(0, predict.transform.position);

        while (line.positionCount < maxPhysicsFrameIterations)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);

            line.positionCount++;
            line.SetPosition(line.positionCount - 1, predict.transform.position);
        }

        Destroy(predict.gameObject);
    }
}