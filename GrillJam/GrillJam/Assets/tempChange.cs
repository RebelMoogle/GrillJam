using UnityEngine;
using System.Collections;

public class tempChange : MonoBehaviour {

    // The temperature this inflicts on others (negative: cold, positive: hot)
    public float exoTemp = 0.1f; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.SetTargetTemperature(exoTemp);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.SetTargetTemperature(); // default Temp
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        PlayerHealth player = col.collider.gameObject.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.SetTargetTemperature(exoTemp);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        PlayerHealth player = col.collider.gameObject.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.SetTargetTemperature();
        }
    }


}
