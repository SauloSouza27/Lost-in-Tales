using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float layerHeight = 1.0f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isMoving = false;

    public GameObject box;

    private int currentLayer = 0;

    private GameObject selectedBlock;
    private Vector3 sokobanBlockOffset;

    private Animator animator;

    private bool collisionChecks = false;

    public AudioSource audiosource;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
                            else if (hit.collider.CompareTag("Sokoban"))
                            {
                                HandleSokobanBlock(hit.collider);
                                box = hit.collider.gameObject;
                                
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


        Debug.Log(collisionChecks);

        if (selectedBlock != null && !collisionChecks)
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

        if (selectedBlock != null && selectedBlock.CompareTag("Sokoban"))
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

                        StartCoroutine(MovePlayer());
                        currentLayer = hitLayer;
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
        else
        {
            RaycastHit sokobanHit;
            if (!Physics.Raycast(hitPosition, Vector3.up, out sokobanHit, 1.0f) || !sokobanHit.collider.CompareTag("Sokoban"))
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

                            StartCoroutine(MovePlayer());
                            currentLayer = hitLayer;
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
                selectedBlock = sokobanCollider.gameObject;
                sokobanBlockOffset = selectedBlock.transform.position - transform.position;
                
            }
        }
        else if (selectedBlock == sokobanCollider.gameObject)
        {
      
            int hitLayer = Mathf.RoundToInt(selectedBlock.transform.position.y);
            if (Mathf.Abs(hitLayer - currentLayer) <= 1 && IsAdjacent(selectedBlock.transform.position, transform.position))
            {
                targetPosition = selectedBlock.transform.position + Vector3.up;

                Vector3 direction = targetPosition - transform.position;
                CalculateRotation(direction);

                StartCoroutine(MovePlayer());
                currentLayer = hitLayer;
            }
            selectedBlock = null;
        }
        else
        {
            int hitLayer = Mathf.RoundToInt(sokobanCollider.transform.position.y);
            if (Mathf.Abs(hitLayer - currentLayer) <= 1 && IsAdjacent(sokobanCollider.transform.position, transform.position))
            {
                selectedBlock = sokobanCollider.gameObject;
                sokobanBlockOffset = selectedBlock.transform.position - transform.position;
            }
        }
    }

    private IEnumerator MovePlayer()
    {
        isMoving = true;
        animator.SetBool("IsMoving", true);
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
        animator.SetBool("IsMoving", false);

        if (selectedBlock != null)
        {
            selectedBlock = null;
            box.GetComponent<BoxScript>().collisionCheck = false;
            box.GetComponent<BoxScript>().SetInitialPosition(box.transform.position);
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
}