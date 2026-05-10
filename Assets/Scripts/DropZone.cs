using UnityEngine;

public class DropZone : MonoBehaviour
{
    [Tooltip("How much money the player gets for cleaning an item")]
    public float itemValue = 5.00f;

    // This runs the millisecond something touches our invisible trigger box
    void OnTriggerEnter(Collider other)
    {
        // 1. Is the object that touched the zone an interactable item?
        if (other.CompareTag("Interactable"))
        {
            // 2. Add the money to our HUD!
            TavernManager.instance.AddMoney(itemValue);

            // 3. If the player is currently holding it, break the tether
            SpringJoint joint = other.GetComponent<SpringJoint>();
            if (joint != null)
            {
                Destroy(joint);
            }

            // 4. Delete the mug/plate to simulate "washing" it!
            Destroy(other.gameObject);
        }
    }
}