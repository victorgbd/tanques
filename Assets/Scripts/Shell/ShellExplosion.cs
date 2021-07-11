using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere (transform.position, m_ExplosionRadius, m_TankMask);
        
        // Recorro los colliders
        for (int i = 0; i < colliders.Length; i++)
            {
             // Selecciono su Rigidbody.
             Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody> ();
            
            // SI no tienen, paso al siguiente.
             if (!targetRigidbody)
                continue;
            
            // Añado la fiuerza de la explosión.
            targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);
            // Busco el script TankHealth asociado con el Rigidbody.
             TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();
            
             // SI no hay script TankHealth, paso al siguiente.
             if (!targetHealth)
                continue;
              // Calculo el daño a aplicar en función de la distancia a la bomba.
             float damage = CalculateDamage (targetRigidbody.position);
            
             // Aplico el daño al tanque.
             targetHealth.TakeDamage (damage);
             }
        
         // Desacnlo el sistema de aprticulas de la bomba.
         m_ExplosionParticles.transform.parent = null;
        
        // Reproduczco el sistema de pertículas.
         m_ExplosionParticles.Play();
        
         // Reproduzco el audio.
         m_ExplosionAudio.Play();
        
         // Cuando las partículas han terminado, destruyo su objeto asoiado.
         Destroy (m_ExplosionParticles.gameObject, m_ExplosionParticles.
            main.duration);
        
         // Destruyo la bomba.
         Destroy (gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Creo un vector desde la bomba al objetivo.
       Vector3 explosionToTarget = targetPosition - transform.position;
      
        // Calculo la distancia desde la bomba al objetivo.
        float explosionDistance = explosionToTarget.magnitude;
        
         // Calculo la proporción de máxima distancia (radio máximo) desde la explosión al tanque.
        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
        
         // Calculo el daño a esa proporción.
         float damage = relativeDistance * m_MaxDamage;
        
         // Me aseguro de que el mínimo daño siempre es 0.
        damage = Mathf.Max (0f, damage);
       
        //Devuelvo el daño
        return damage;
        }

    }
