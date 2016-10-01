using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class PlayerHealth : MonoBehaviour
{
    public float highDeathTemp = 1.0f; // can die from high heat
    public float lowDeathTemp = -1.0f; // can die from low temp
    public float Temperature = 1.0f;		    // The player's Temperature. at 1.0 it's coal, at -1.0 it's an icicle
    public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.
    public AudioClip[] heatClips;				// Array of clips to play when the player is damaged.
    public AudioClip[] iceClips;
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

    private bool isDying = false;

    void Awake ()
    {
        // Setting up references.
        anim = GetComponent<Animator>();

        playerSprites = GetComponentsInChildren<SpriteRenderer>();
        defaultTemp = targetTemperature;

    }


    void Update()
    {
        if (isDying)
            return;

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
        if (Temperature >= highDeathTemp || Temperature <= lowDeathTemp )
        {
            isDying = true;
            // ... disable user Player Control script
            GetComponent<PlayerControl>().enabled = false;

            // ... Trigger the 'Die' animation state
            // TODO: play different depeding on ice or heat death

            
            Die(Temperature >= highDeathTemp);
        }
    }

    private void Die(bool heatDeath)
    {
        if(gameObject.tag == "Player")
        {
            if (heatDeath)
            {
                int i = UnityEngine.Random.Range(1, heatClips.Length);
                AudioSource.PlayClipAtPoint(heatClips[i], transform.position);

                foreach (SpriteRenderer bodyPart in playerSprites)
                { bodyPart.material.color = Color.white; }

                if (anim)
                { anim.SetTrigger("Die"); }

            }
            else
            {
                int i = UnityEngine.Random.Range(1, iceClips.Length);
                AudioSource.PlayClipAtPoint(iceClips[i], transform.position);

                foreach (SpriteRenderer bodyPart in playerSprites)
                { bodyPart.material.color = Color.white; }

                if (anim)
                { anim.SetTrigger("IceDie"); }
            }
        }
        

        //yield return new WaitForSeconds(0.1f);
        Destroy(gameObject, 2);
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
        if(gameObject.tag == "Player")
        {
            if(newTemp > 0.0f)
            {
                AudioSource.PlayClipAtPoint(heatClips[0], transform.position);
            }
            else
            {
                AudioSource.PlayClipAtPoint(iceClips[0], transform.position);
            }
            
        }
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
