using JetBrains.Annotations;
using UnityEngine;

public class DoorRotation : MonoBehaviour
{
    [SerializeField] private Transform _target = null; // Target transform indicating the door's closed rotation
    [SerializeField] private float _rotationAngle = 90f; // Rotation angle
    private bool _isOpen; // Door state
    private Quaternion _closedRotation; // Closed rotation
    private Quaternion _openRotation; // Open rotation
    private Quaternion _targetRotation; // Target rotation
    [SerializeField] private float _rotationSpeed = 2f; // Speed of rotation

    private void Start()
    {
        _closedRotation = transform.rotation; // Assuming the door's initial rotation is its closed state
        // Calculate open rotation based on the closed rotation and the rotation angle
        _openRotation = _closedRotation * Quaternion.Euler(0, _rotationAngle, 0);
    }

    [UsedImplicitly]
    public void ToggleDoor()
    {
        // Toggle the state
        _isOpen = !_isOpen;

        // Set the target rotation based on the new state
        _targetRotation = _isOpen ? _openRotation : _closedRotation;
    }

    private void Update()
    {
        // Smoothly rotate the door towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * _rotationSpeed);
    }
}