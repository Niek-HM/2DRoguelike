//for a more detailed script lifecycle overview go to:
//https://docs.unity3d.com/Manual/ExecutionOrder.html
//input system documentation:
//https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/index.html

using UnityEngine;
using UnityEngine.InputSystem;

//using this attribute, Unity will make sure that the specified MonoBehaviour/s-
//exist on the same GameObject that this MonoBehaviour exists on
[RequireComponent(typeof(Rigidbody2D))]
public class BasicMovementScript : MonoBehaviour, _2DRoguelikeDef_Input.IPlayerActions
{
    //y formula: -(collider2D.y/2)-(collider2D.x/2)-0.04
    private Vector3 groundCheckPositionOffset = new Vector3(0.0f, -0.36f, 0.0f);

    //called only in the editor - use for drawing gizmos
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position+groundCheckPositionOffset, 0.295f);
    }

    private Rigidbody2D rb;
    private _2DRoguelikeDef_Input input;
    [SerializeField] private BasicMovementData movementData;
    private Vector2 movementVector = Vector2.zero;

    //first method called by Unity in this script's life cycle - use for gathering references
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        input = new _2DRoguelikeDef_Input();
    }

    //second, always called when the object becomes enabled - use for initializing variables when enabled
    private void OnEnable() {
        input.Enable();
    }

    //third - use for exchanging information between scripts
    private void Start() {
        //set the input callbacks to be recieved by this instance
        input.Player.SetCallbacks(this);
    }

    //fourth - use for physics specific cases
    private void FixedUpdate() {
        //call Move with the desired frame dependant velocity
        Move(movementVector*movementData.movementForce*Time.fixedDeltaTime);
    }

    //fifth, called every frame - use for cases where per frame updates are crucial
    // private void Update() {}

    //sixth, called at the end of the game logic cycle
    // private void LateUpdate() {}

    //seventh, called when the object is disabled, also called just before OnDestroy
    private void OnDisable() {
        input.Disable();
    }

    //eighth, called if the object was destroyed
    //private void OnDestroy() {}

    //called in FixedUpdate because it is dependant on physics
    private void Move(Vector2 movement) {
        //the percentage of force to be applied dependant on the current velocity
        float percentage = Mathf.Clamp01(movementData.maxMovementSpeed-rb.velocity.magnitude);
        //apply movement velocity
        rb.velocity += movement*percentage;
    }

    //input OnMove event called for the assigned move key/s - use for input specific cases
    public void OnMove(InputAction.CallbackContext context) {
        //update the movement vector to be used for the next physics dependant Move call
        movementVector.x = context.ReadValue<float>();
    }

    //input events can be called up to three times, once for each key action
    public void OnJump(InputAction.CallbackContext context) {
        //makes sure that we only jump when the key action was started(began pressing)
        // if(!context.started)
        //     return;

        //performed(pressed)
        if(!context.performed)
            return;

        //canceled(stopped pressing)
        // if(!context.canceled)
        //     return;

        //OverlapCircle returns true if the given circle paramaters overlap with another collider
        //makes sure player is grounded before applying jump force
        if(Physics2D.OverlapCircle(transform.position+groundCheckPositionOffset, 0.295f, movementData.groundLayers))
            rb.AddForce(transform.up*movementData.jumpForce, ForceMode2D.Impulse);
    }
}
