using UnityEngine;

public class ElevatorButton : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    enum Scene { WanderlustClub, TestGround }
    [SerializeField] private Scene SceneToLoad;

    public string InteractionPromt => _prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Press elevator");
        SceneLoader.LoadScene(SceneToLoad.ToString());
        return true;
    }
}
