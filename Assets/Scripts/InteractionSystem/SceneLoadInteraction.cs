using UnityEngine;

public class SceneLoadInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private Scenes.SceneEnum _sceneToLoad;


    public string InteractionPromt => _prompt;

    public bool Interact(Interactor interactor)
    {
        SceneLoader.LoadScene(_sceneToLoad.ToString());
        return true;
    }
}
