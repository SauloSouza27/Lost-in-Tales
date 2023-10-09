using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float layerHeight = 1.0f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    private int currentLayer = 0;

    private GameObject selectedBlock;
    private Vector3 sokobanBlockOffset;

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
                            }
                        }
                    }
                }
            }
        }

        if (selectedBlock != null)
        {
            selectedBlock.transform.position = transform.position + sokobanBlockOffset;
        }
    }

    private void HandleMovableBlock(Collider movableCollider)
    {
        Vector3 hitPosition = movableCollider.bounds.center;

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

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;

        if (selectedBlock != null)
        {
            selectedBlock = null;
        }

        Debug.Log("Current Layer: " + currentLayer);
    }

    private bool IsAdjacent(Vector3 position1, Vector3 position2)
    {
        float distanceXZ = Vector2.Distance(new Vector2(position1.x, position1.z), new Vector2(position2.x, position2.z));
        float heightDifference = Mathf.Abs(position1.y - position2.y);

        return (distanceXZ <= 2.05f && heightDifference <= 1.05f) || (distanceXZ <= 2.05f && heightDifference >= layerHeight - 0.05f);
    }
}
