
using UnityEngine;
using TMPro;

public class PickUpObj : MonoBehaviour {

    // Keep track of if currently carrying and the carried object
    // currently_carrying -> to avoid picking up the other sphere
    // carried_obj -> to put down the sphere
    bool currently_carrying = false;
    GameObject carried_obj;

    // How far the sphere is to be held in front of the user
    public int distance;
    // After two objects are kept down, show end message
    public int num_of_kept_down = 0;
    Camera cam;

    public TextMeshProUGUI finish_text;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

        // After two objects are kept down, show end message
        if (num_of_kept_down >= 2)
        {
            finish_text.enabled = true; 
        }

        // Carry() if currently_carrying isn't null
        if (currently_carrying)
        {
            carry(carried_obj);
        }
	}

    void carry(GameObject object_to_carry)
    {

        object_to_carry.transform.position = cam.transform.position +
                                                cam.transform.forward * distance +
                                                    cam.transform.up * -(0.5f) ; 
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // If collided object has this script and the hasn't been put down then pickup
        // If it has been picked up, then it's not allowed to be picked up 
        if (other.transform.GetComponent<AllowPickingUp>() != null &&
            !other.transform.GetComponent<AllowPickingUp>().has_been_put_down)
        {
            pickUp(other.transform.gameObject);
        }

        // If the collided zone has this script and we are carrying a sphere and the types of both match, then put the object down 
        if (other.transform.GetComponent<AllowPuttingDown>()!= null && carried_obj &&
            (other.transform.GetComponent<AllowPuttingDown>().type == carried_obj.GetComponent<AllowPickingUp>().type))
        {
            putDownCarried();
        }

    }
    

    void pickUp(GameObject obj_to_pickup)
    {
        if (!currently_carrying)
        {
            // Pick up object
            currently_carrying = true;
            carried_obj = obj_to_pickup;
            obj_to_pickup.GetComponent<Rigidbody>().isKinematic = true;

        }
    }

    void putDownCarried()
    {
        // Call only if currently_carrying a sphere
        if (currently_carrying)
        {
            currently_carrying = false;

            // Put down 
            carried_obj.GetComponent<Rigidbody>().isKinematic = false;

            carried_obj.GetComponent<Rigidbody>().useGravity = true;

            // Set params for further processing
            carried_obj.GetComponent<AllowPickingUp>().has_been_put_down = true;
            carried_obj = null;

            // When num_of_kept_down == 2, end
            num_of_kept_down += 1;
        }
    }

}
