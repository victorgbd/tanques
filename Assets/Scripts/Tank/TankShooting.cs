using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;

    
    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;                


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }
    

    private void Update()
    {
        // Asigno el valor mínimo al slider.
        m_AimSlider.value = m_MinLaunchForce;
        //Si llego al valor máximo y no lo he lanzado...
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // ... uso el valor máximo y disparo.
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire ();
        }
        // Si no, si ya he pulsado el botón de disparo...
        else if (Input.GetButtonDown (m_FireButton))
        {
            // ... reseteo el booleano de dipsaro y la fuerza de disparo.
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;
            // Cambio el clip de audio al de cargando y lo reproduzco.
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play ();
        }
        //Si no, si estoy manteniendo presionado el botón de disparo y aún nohe disparado...
        else if (Input.GetButton (m_FireButton) && !m_Fired)
        {
            // Incremento la fuerza de disparo y actualizo el slider.
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_CurrentLaunchForce;
        }
        //Si no, si ya he soltado el botón de disparo y aún no he lanzado...
        else if (Input.GetButtonUp (m_FireButton) && !m_Fired)
        {
            // ... disparo.
            Fire ();
        }
    }


    private void Fire()
    {
        // Ajusto el booleano a true para que solo se lance una vez.
        m_Fired = true;
        // Creo una instancia de la bomba y guardo una referencia en su Rigidbody.
        Rigidbody shellInstance =
                Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        // Ajusto la velocidad de la bomba en la dirección de disparo.
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; ;
        // Cambio el audio al de disparo y lo reproduzco.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play ();
        //Reseteo la fuerza de lanzamiento como precaución ante posibles eventos de botón "perdidos".
        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}