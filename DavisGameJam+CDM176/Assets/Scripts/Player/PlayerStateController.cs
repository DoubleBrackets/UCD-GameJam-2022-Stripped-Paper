using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    // Context components
    public Rigidbody2D rb;
    public GroundedMovement groundedMovement;
    public MovementProfile movementProfile;
    public LayerIndicatorGenerator layerIndicatorGenerator;
    public Animator animator;
    public Transform spriteContainer;


    private PlayerAbstractState currentState;

    public PlayerAbstractState CurrentState
    {
        get => currentState;
        set => currentState = value;
    }

    // Input stuff
    private float horizontalInput;

    public float HorizontalInput
    {
        get => horizontalInput;
    }

    public System.Action OnJumpPressed;

    private bool canSwitchLayer = true;
    public bool CanSwitchLayer
    {
        get => canSwitchLayer;
        set => canSwitchLayer = value;
    }

    private void Awake()
    {
        currentState = new PlayerIdleState(this);
        currentState.EnterState();
    }

    public void KillPlayer()
    {
        currentState.SwitchState(new PlayerDeadState(this));
    }

    public void RespawnPlayer()
    {
        currentState.SwitchState(new PlayerIdleState(this));
        layerIndicatorGenerator.Regenerate();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpPressed?.Invoke();
        }

        if(CanSwitchLayer)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (GameLayerManager.instance.StripTopLayer())
                {
                    currentState.SwitchState(new PlayerLayerChangeState(this));
                }
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                if (GameLayerManager.instance.UnstripTopLayer())
                {
                    currentState.SwitchState(new PlayerLayerChangeState(this));
                }
            }
        }
        if (horizontalInput != 0)
            spriteContainer.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);

        currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }
}
