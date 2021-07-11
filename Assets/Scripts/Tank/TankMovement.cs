using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    public float m_Speed = 12f;            
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;

    
    private string m_MovementAxisName;     
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;         


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }
    

    private void Update ()
    {
         // Almaceno los valores de entrada.
        m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis (m_TurnAxisName);
        
         //Llamo a la función que gestiona el audio del motor
        EngineAudio ();
    }


    private void EngineAudio()
    {
        if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
         {
            // ... y si estaba reproduciendo el audio de moverse...
             if (m_MovementAudio.clip == m_EngineDriving)
                 {
                 // ... cambio el audio al de estar parado y lo reproduz
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play ();
                 }
            }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... cambio el audio al de estar parado y lo reproduz
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play ();
            }
        }
    }


    private void FixedUpdate()
    {
        Move();
        Turn();
    }


    private void Move()
    {
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        
        // Aplico ese vector de movimiento al rigidbody del tanqe.
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);
        m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);

    }
}