using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform cameraTransform;
    private GameObject currentObject;

    private void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnInteractPressed += Interact;
        }
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnInteractPressed -= Interact;
        }
    }

    private void Update()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance, interactableLayer))
        {
            currentObject = hit.collider.gameObject;
        }
        else
        {
            currentObject = null;
        }
    }

    private void Interact()
    {
        if(currentObject != null)
        {
            currentObject.GetComponent<IInteractable>()?.Interact(gameObject);
            Debug.Log($"Interacted with {currentObject.name}");
        }
        else
        {
            Debug.Log("No interactable object in range.");
        }
    }

    private void OnDrawGizmos()
    {
        if (cameraTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * distance);
        }
    }
}
