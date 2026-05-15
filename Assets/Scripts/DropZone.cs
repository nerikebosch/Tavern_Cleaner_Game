using UnityEngine;

public class DropZone : MonoBehaviour
{
    [Tooltip("How much money the player gets for cleaning an item")]
    public float itemValue = 5.00f;

    // This runs the millisecond something touches the invisible trigger box
    void OnTriggerEnter(Collider other)
    {
        // Is the object that touched the zone an interactable item
        if (other.CompareTag("Interactable"))
        {
            // Add the money to our HUD
            TavernManager.instance.AddMoney(itemValue);

            // If the player is currently holding it, break the tether
            SpringJoint joint = other.GetComponent<SpringJoint>();
            if (joint != null)
            {
                Destroy(joint);
            }

            // Delete the mug/plate to simulate washing it!
            Destroy(other.gameObject);
        }
    }
}