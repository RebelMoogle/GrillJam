using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float Temperature = 1.0f;		    // The player's Temperature. at 1.0 it's coal, at -1.0 it's an icicle
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.
	public AudioClip[] ouchClips;				// Array of clips to play when the player is damaged.
	public float hurtForce = 10f;				// The force with which the player is pushed when hurt.
	private float defaultTemp;		// The default Temperature
    private SpriteRenderer[] playerSprites;

    // The temperature of the environment, player heats to this temp over time. 
    // negative: cooling, positive: heat up
    public float targetTemperature = -0.1f; 
    
	private float lastHitTime;					// The time at which the player was last hit.
	private Animator anim;						// Reference to the Animator on the player

    public float TimeToNormal = 1000.0f; // How much time the temperature of the object needs to change to the environment temp
    private float tempChangeTime = 0.0f;

	void Awake ()
	{
		// Setting up references.
		anim = GetComponent<Animator>();

        playerSprites = GetComponentsInChildren<SpriteRenderer>();
        defaultTemp = targetTemperature;

    }


    void Update()
    {
        if (Mathf.Abs(Temperature - targetTemperature) > 0.01f)
        {
            tempChangeTime += Time.deltaTime;
            Temperature = Mathf.Lerp(Temperature, targetTemperature, tempChangeTime / TimeToNormal);
            if (Mathf.Abs(Temperature - targetTemperature) > 0.01f)
            {
                tempChangeTime = 0.0f;
            }
        }

        UpdateHealthRender();

        // ... and if the player still has Temperature...
        if (Mathf.Abs(Temperature) >= 1.0f)
        {
            // Find all of the colliders on the gameobject and set them all to be triggers.
            Collider2D[] cols = GetComponents<Collider2D>();
            foreach (Collider2D c in cols)
            {
                c.isTrigger = true;
            }

            // Move all sprite parts of the player to the front
            SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer s in spr)
            {
                s.sortingLayerName = "UI";
            }

            // ... disable user Player Control script
            GetComponent<PlayerControl>().enabled = false;

            // ... disable the Gun script to stop a dead guy shooting a nonexistant bazooka
            GetComponentInChildren<Gun>().enabled = false;

            // ... Trigger the 'Die' animation state
            if (anim)
            { anim.SetTrigger("Die"); }
        }
    }

    public void SetTargetTemperature()
    {
        targetTemperature = defaultTemp;
    }
	public void SetTargetTemperature (float newTemp)
	{

		// Reduce the player's Temperature by set damage.
		targetTemperature = newTemp;

        // TODO: react to collisions or extreme heat

		// Play a random clip of the player getting hurt.
		//int i = Random.Range (0, ouchClips.Length);
		//AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
	}


	public void UpdateHealthRender ()
	{
		// Set the Temperature bar's colour to proportion of the way between green and red based on the player's Temperature.
        if(Temperature < 0.0f)
        {
            foreach(SpriteRenderer bodyPart in playerSprites)
            {
                bodyPart.material.color = Color.Lerp(Color.white, Color.blue, -Temperature);
            }  
        }
        else
        {
            foreach (SpriteRenderer bodyPart in playerSprites)
            { bodyPart.material.color = Color.Lerp(Color.white, Color.red, Temperature); }
        }


	}
}
