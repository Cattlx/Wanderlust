using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPromt => _prompt;

    [SerializeField] private ItemObject _itemObject;

    public bool Interact(Interactor interactor)
    {
        _itemObject.OnHandlePickupItem();
        return true;
    }
}
