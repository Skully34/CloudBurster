using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseHealthHit : MonoBehaviour
{

    [SerializeField] float health;

    [SerializeField] float immunityTime;
    float immunityTimeActual;
    bool immune;

    int freezeAmount;
    [SerializeField] int freezeLimit;
    [SerializeField] float freezeTime;
    float freezeTimeActual;
    bool frozen;
    [SerializeField] public bool MC;
    Rigidbody2D rb;
    CheckpointSystem checkpointSystem;
    
    void Start()
    {
        GetComponent<Rigidbody2D>();
        checkpointSystem = FindFirstObjectByType<CheckpointSystem>();
    }

    
    void Update()
    {
        //damage immunity timer
        if (immune)
        {
            immunityTimeActual -= Time.deltaTime;
        }
        if (immunityTimeActual < 0)
        {
            immune = false;
        }

        //freeze logic
        if (freezeAmount > freezeLimit)
        {
            freezeTimeActual = freezeTime;
            frozen = true;
            rb.simulated = false;
            FreezeEvent();
        }

     if (frozen)
        {
            freezeTimeActual -= Time.deltaTime;
        }
     if (frozen && freezeTimeActual <= 1)
        {
            frozen = false;
            rb.simulated = true;
            UnFreezeEvent();
        }
        
    }

    // damage is float to account for super low damage attacks, but wanting to keep health in simple numbers for the MC.
    public void Hit(float damage, int freezeAtk)
    {
        if (!immune) {
            if (MC)
            {
                if (damage >= 1)
                {
                    damage = 1;
                }
                health -= damage;
                immune = true;
                immunityTimeActual = immunityTime;
            }
            else if (!MC)
            {
                health -= damage;
            }

            freezeAmount += freezeAtk;
            
            if (health <= 0)
            {
                DeathEvent();
            }
        }
    }
    public void FreezeEvent()
    {
        //method for freeze triggers
    }
    public void UnFreezeEvent()
    {
        //method for unfreeze triggers
    }
    public void DeathEvent()
    {
        if (MC)
        {
            Scene currentScene;
            currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        if (!MC)
        {
            Destroy(this.gameObject);
        }
    }
}
