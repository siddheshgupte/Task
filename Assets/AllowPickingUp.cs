
using UnityEngine;

public class AllowPickingUp : MonoBehaviour {

    //Type allows us to later recognize which zone the putting down should be triggered on
    public string type;

    // If it has been put down don't allow picking up
    public bool has_been_put_down = false;
    
}
