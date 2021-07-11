using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;          
    public Slider m_Slider;                        
    public Image m_FillImage;                      
    public Color m_FullHealthColor = Color.green;  
    public Color m_ZeroHealthColor = Color.red;    
    public GameObject m_ExplosionPrefab;
    
   
    private AudioSource m_ExplosionAudio;          
    private ParticleSystem m_ExplosionParticles;   
    private float m_CurrentHealth;  
    private bool m_Dead;            


    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }
    

    public void TakeDamage(float amount)
    {
        // Reducimos la salud según la cantidad de daño recibida.
         m_CurrentHealth -= amount;
        
         // Actualizamos el slider de salud con esos valores
         SetHealthUI ();
        
         // Si la salud es menor que 0 y aún no lo he explotado, llamo al
            //método OnDeath (al morir).
         if (m_CurrentHealth <= 0f && !m_Dead)
         {
             OnDeath ();
         }

    }


    private void SetHealthUI()
    {
        m_Slider.value = m_CurrentHealth;
       
        // Creo un color para el slider entre verde y rojo en función del porcentaje de salud
        m_FillImage.color = Color.Lerp (m_ZeroHealthColor,
            m_FullHealthColor, m_CurrentHealth / m_StartingHealth);

    }


    private void OnDeath()
    { 
        m_Dead = true;
        
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive (true);
       
        // Reproduzco el sistema de partículas del tanque explotando.
        m_ExplosionParticles.Play ();
        
        // Reproduzco el audio del tanque explotando.
        m_ExplosionAudio.Play();
        
        // Desactivo el tanque.
        gameObject.SetActive (false);
    }
}