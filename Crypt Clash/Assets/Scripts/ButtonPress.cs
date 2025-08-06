using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    // Reference to the door GameObject to be destroyed/moved/rotated
    public GameObject door;

    public enum DoorActionType { Destroy, Move, Rotate }
    public DoorActionType actionType = DoorActionType.Destroy;

    // Target position for moving the door
    public Vector3 moveToPosition;
    // Target rotation for rotating the door (Euler angles)
    public Vector3 rotateToEulerAngles;


    // Detect player collision with the button
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player (tagged as "Player")
        if (other.CompareTag("Player"))
        {
            // Change button color to green
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.green;
            }
            if (door != null)
            {
                switch (actionType)
                {
                    case DoorActionType.Destroy:
                        Destroy(door);
                        break;
                    case DoorActionType.Move:
                        door.transform.position = moveToPosition;
                        break;
                    case DoorActionType.Rotate:
                        door.transform.rotation = Quaternion.Euler(rotateToEulerAngles);
                        break;
                }
            }
        }
    }
}
