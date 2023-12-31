using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public int level;

    private Vector3 initialPosition;

    public float moveSpeed = 1.0f;
    public float layerHeight = 1.0f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isMoving = false;

    private GameObject box;

    public GameObject climbButton;


    private int targetLayer;

    private int currentLayer = 0;

    private GameObject selectedBlock;

    private bool isSokobanSelected = false;
    private Vector3 sokobanBlockOffset;

    private bool isJumping = false;
    private bool isClimbing = false;


    private Animator animator;

    private bool collisionChecks = false;

    public AudioSource audiosource;

    public Transform player;

    private void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (!isMoving)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider != null)
                        {
                            if (hit.collider.CompareTag("Movable"))
                            {
                                HandleMovableBlock(hit.collider);
                                
                            }
                            else if (hit.collider.CompareTag("Sokoban") || hit.collider.CompareTag("Pushable"))
                            {
                                if((selectedBlock != null) && (selectedBlock.CompareTag("Sokoban") || selectedBlock.CompareTag("Pushable")) && (selectedBlock == hit.collider.gameObject))
                                {
                                    HandleMovableBlock(hit.collider);
                                }
                                else
                                {
                                    HandleSokobanBlock(hit.collider);
                                    box = hit.collider.gameObject;
                                }
                            }

                            else 
                            {
                                if(currentLayer == 0)
                                {
                                    animator.SetBool("IsHolding", false);
                                    if (level == 1 && selectedBlock != null)
                                    {
                                        player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, player.transform.eulerAngles.y + 40, player.transform.eulerAngles.z);
                                    }
                                }
                                if(selectedBlock != null)
                                {
                                    selectedBlock.transform.GetChild(0).gameObject.SetActive(false);
                                    selectedBlock = null;
                                    isSokobanSelected = false;
                                    climbButton.gameObject.SetActive(false);
                                }                                
                            }
                        }
                        
                    }
                }
            }
        }

        

        if (box != null)
        {
            collisionChecks = box.GetComponent<BoxScript>().collisionCheck;
        }
        if (isSokobanSelected && selectedBlock != null)
        {
            Vector3 directionToBlock = selectedBlock.transform.position - transform.position;
            Vector3 directionXZ = new Vector3(directionToBlock.x, 0, directionToBlock.z);
            CalculateRotation(directionXZ);
            transform.rotation = targetRotation;
        }

        

        if (selectedBlock != null && !collisionChecks && currentLayer != 1)
        {
            selectedBlock.transform.position = transform.position + sokobanBlockOffset;
                       
        }
    }

    private void HandleMovableBlock(Collider movableCollider)
    {
        Vector3 hitPosition = movableCollider.bounds.center;
        Vector3 direction = hitPosition - transform.position;

        int hitLayer = Mathf.RoundToInt(hitPosition.y);
        int maxLayerDifference = 1;

        bool hasSokobanOnSameTile = false;
        bool hasDiffSokobanOnSameTile = false;
        Collider[] hits = Physics.OverlapSphere(hitPosition, 0.1f);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Sokoban")|| hit.CompareTag("Pushable"))
            {
                hasSokobanOnSameTile = true;
                if((hit.CompareTag("Sokoban")|| hit.CompareTag("Pushable")) && hit.gameObject != selectedBlock)
                {
                    hasDiffSokobanOnSameTile = true;
                }
                break;
            }
        }
        

        

        if ((selectedBlock != null && selectedBlock.CompareTag("Sokoban") && !hasDiffSokobanOnSameTile && currentLayer != 1) 
        || (selectedBlock != null && selectedBlock.CompareTag("Pushable") && !hasDiffSokobanOnSameTile && currentLayer != 1))
        {
            
            if (Mathf.Abs(hitLayer - currentLayer) <= maxLayerDifference)
            {
                    hitPosition.y = transform.position.y;

                    if (IsAdjacent(hitPosition, transform.position))
                    {
                        targetPosition = hitPosition;

                        CalculateRotation(direction);

                        transform.rotation = targetRotation;

                        StartCoroutine(MovePlayer());
                    }
                
            }
        }
        else if (hasSokobanOnSameTile)
        {
            return;
        }

        else
        {
            RaycastHit sokobanHit;
            if (!Physics.Raycast(hitPosition, Vector3.up, out sokobanHit, 1.0f) || !sokobanHit.collider.CompareTag("Sokoban") || !sokobanHit.collider.CompareTag("Pushable"))
            {
                
                if (Mathf.Abs(hitLayer - currentLayer) <= maxLayerDifference)
                {
                    if (hitLayer == currentLayer + 1 || hitLayer == currentLayer - 1)
                    {
                        if (IsAdjacent(hitPosition, transform.position))
                        {
                            targetPosition = hitPosition + Vector3.up;

                            CalculateRotation(direction);

                            transform.rotation = targetRotation;


                            if (hitLayer == currentLayer + 1)
                            {
                                isClimbing = true;
                            }
                            if (hitLayer == currentLayer - 1)
                            {
                                isJumping = true;
                            }
                            targetLayer = hitLayer;
                            StartCoroutine(MovePlayer());
                        }
                    }
                    else
                    {
                        hitPosition.y = transform.position.y;

                        if (IsAdjacent(hitPosition, transform.position))
                        {
                            targetPosition = hitPosition;

                            CalculateRotation(direction);

                            transform.rotation = targetRotation;

                            StartCoroutine(MovePlayer());
                        }
                    }
                }
            }
        }
    }

    private void HandleSokobanBlock(Collider sokobanCollider)
    {
        if (selectedBlock == null)
        {
            int hitLayer = Mathf.RoundToInt(sokobanCollider.transform.position.y);

            if (Mathf.Abs(hitLayer - currentLayer) <= 1 && IsAdjacent(sokobanCollider.transform.position, transform.position))
            {
                if (sokobanCollider.CompareTag("Sokoban"))
                {
                    climbButton.gameObject.SetActive(true);
                }
                
                selectedBlock = sokobanCollider.gameObject;
                if (currentLayer == 0)
                {
                    animator.SetBool("IsHolding", true);
                    if (level == 1)
                    {
                        Vector3 currentRotation = player.transform.rotation.eulerAngles;
                        currentRotation.y -= 40f;
                        player.transform.rotation = Quaternion.Euler(currentRotation);
                    }
                }
                selectedBlock.transform.GetChild(0).gameObject.SetActive(true);
                
                isSokobanSelected = true;
                sokobanBlockOffset = selectedBlock.transform.position - transform.position;
                
                
                
            }
        }
        else if ((selectedBlock == sokobanCollider.gameObject && sokobanCollider.CompareTag("Sokoban"))|| (selectedBlock == sokobanCollider.gameObject && sokobanCollider.CompareTag("Pushable")))
        { 
            if(currentLayer == 0)
            {
                animator.SetBool("IsHolding", false);
                if (level == 1)
                {
                    Vector3 currentRotation = player.transform.rotation.eulerAngles;
                    currentRotation.y += 40f;
                    player.transform.rotation = Quaternion.Euler(currentRotation);
                }
            }
            selectedBlock.transform.GetChild(0).gameObject.SetActive(false);
            selectedBlock = null;
            isSokobanSelected = false;
            climbButton.gameObject.SetActive(false);
        }
        else
        {
            int hitLayer = Mathf.RoundToInt(sokobanCollider.transform.position.y);
            if (Mathf.Abs(hitLayer - currentLayer) <= 1 && IsAdjacent(sokobanCollider.transform.position, transform.position))
            {
                selectedBlock = sokobanCollider.gameObject;

                if(currentLayer == 0)
                {
                    animator.SetBool("IsHolding", true);
                    if (level == 1)
                    {
                        Vector3 currentRotation = player.transform.rotation.eulerAngles;
                        currentRotation.y -= 40f;
                        player.transform.rotation = Quaternion.Euler(currentRotation);
                    }
                }
                selectedBlock.transform.GetChild(0).gameObject.SetActive(true);
                
                isSokobanSelected = true;
                sokobanBlockOffset = selectedBlock.transform.position - transform.position;
            }
        }
    }

    

    private IEnumerator MovePlayer()
    {
        SetInitialPosition(transform.position);
        isMoving = true;
        if (isJumping == true)
        {
            animator.SetBool("IsJumping", true);
            if (level == 1)
            {
                Vector3 currentRotation = player.transform.rotation.eulerAngles;
                currentRotation.y -= 40f;
                player.transform.rotation = Quaternion.Euler(currentRotation);
            }
            box.GetComponent<BoxScript>().SetActiveOnBoxLight(false);
        }
        else if (isClimbing == true)
        {
            animator.SetBool("IsClimbing", true);
            if (level == 1)
            {
                Vector3 currentRotation = player.transform.rotation.eulerAngles;
                currentRotation.y -= 40f;
                player.transform.rotation = Quaternion.Euler(currentRotation);
            }
            box.GetComponent<BoxScript>().SetActiveOnBoxLight(true);
        }
        else if (isSokobanSelected == true)
        {
            animator.SetBool("IsPushing", true);
        }
        else
        {
            animator.SetBool("IsMoving", true);
        }
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
        currentLayer = targetLayer;
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsClimbing", false);
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsPushing", false);


        if (selectedBlock != null)
        {
            isClimbing = false;
            currentLayer = targetLayer;
            if(collisionChecks) 
            {
                transform.position = initialPosition;
            }
            box.GetComponent<BoxScript>().collisionCheck = false;
            box.GetComponent<BoxScript>().SetInitialPosition(box.transform.position);
        }
        if (isJumping == true)
        {
            if (level == 1)
            {
                Vector3 currentRotation = player.transform.rotation.eulerAngles;
                currentRotation.y += 40f;
                player.transform.rotation = Quaternion.Euler(currentRotation);
            }
            isJumping = false;
            currentLayer = targetLayer;
        }
        if (isClimbing == true)
        {
            animator.SetBool("IsHolding", false);
            if (level == 1)
            {
                Vector3 currentRotation = player.transform.rotation.eulerAngles;
                currentRotation.y += 40f;
                player.transform.rotation = Quaternion.Euler(currentRotation);
            }
            isClimbing = false;
            currentLayer = targetLayer;
        }

    }

    private bool IsAdjacent(Vector3 position1, Vector3 position2)
    {
        float distanceXZ = Vector2.Distance(new Vector2(position1.x, position1.z), new Vector2(position2.x, position2.z));
        float heightDifference = Mathf.Abs(position1.y - position2.y);

        return (distanceXZ <= 2.05f && heightDifference <= 1.05f) || (distanceXZ <= 2.05f && heightDifference >= layerHeight - 0.05f);
    }

    private void CalculateRotation(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        angle = Mathf.Round(angle / 90) * 90;
        targetRotation = Quaternion.Euler(0, angle, 0);
    }

    public void SetInitialPosition(Vector3 actualPos)
    {
        initialPosition = actualPos;
    }

}