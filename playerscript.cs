using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerscript : MonoBehaviour
{
    [SerializeField]private Rigidbody _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _lanewidth;
    [SerializeField] private float _currentLaneIndex;
    [SerializeField] private int _minLaneIndex = -1;
    [SerializeField] private int _maxLaneIndex = 1 ;
    [SerializeField] private int _laneSwitchSpeed = 3 ;
    [SerializeField] private float _jumpForce=5; 
    [SerializeField] private Transform _groundRayCastPoint;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] private roadpiecespawner _roadPieceSpawner;
    [SerializeField] private GameObject _gameOverPanel;
    private bool _isGrounded;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider[] _ragdollRigidColliders;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody=GetComponent<Rigidbody>();
        Time.timeScale=1;
        ChangeStateOfRagdollColliders(false);
        
    }
    void ChangeStateOfRagdollColliders(bool val)
    {
        foreach(Collider r  in _ragdollRigidColliders)
            r.enabled=val;
    }

    // Update is called once per frame
    void Update()
    {

        Inputs();
        CheckIfGrounded();
        Movement();
        Animations();
        
    }

    void Animations()
    {
        float angle =Vector3.SignedAngle(Vector3.forward ,new Vector3(_currentLaneIndex*_lanewidth-transform.position.x,0,1),transform.up);
        _animator.SetFloat("Xinput",angle/75);
    }

    void Movement()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x,_rigidbody.velocity.y,_speed);
        Vector3 targetposition= new Vector3(_currentLaneIndex*_lanewidth,transform.position.y,transform.position.z);
        transform.position = Vector3.Lerp(transform.position,targetposition,Time.deltaTime*_laneSwitchSpeed);
    }

    void Inputs()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
           LaneChange(+1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
           LaneChange(-1);   
        if(Input.GetKeyDown(KeyCode.Space)&& _isGrounded)
        {
            Jump();
        }
    }

    void LaneChange(int x)
    {
        if(x>0 && _currentLaneIndex==_maxLaneIndex)
            return;
        if(x<0 && _currentLaneIndex ==_minLaneIndex)
            return;
        _currentLaneIndex += x;
    }

    void Jump()
    {
        _rigidbody.AddForce(Vector3.up*_jumpForce,ForceMode.Impulse);
        _animator.SetTrigger("Jump");
    }

    void CheckIfGrounded()
    {
        if(Physics.Raycast(_groundRayCastPoint.position,Vector3.down,.1f,_groundLayer))
             _isGrounded = true ;
        else
             _isGrounded = false;
    }

     private void OnTriggerEnter(Collider other)
    {
          if(other.gameObject.CompareTag("SpawnTrigger"))
               _roadPieceSpawner.SpawnRandomRoadPiece();
    }

    private void OnCollisionEnter(Collision collision )
    {
         if(collision.transform.CompareTag("Obstacle"))
         {
              //Time.timeScale=0;
              GameOver();
              _animator.enabled=false;
         }
    }

    void GameOver()
    {
        _gameOverPanel.SetActive(true);
        _animator.enabled=false;
        ChangeStateOfRagdollColliders(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }


}
