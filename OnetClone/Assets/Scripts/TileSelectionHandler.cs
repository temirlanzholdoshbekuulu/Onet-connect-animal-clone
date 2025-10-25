using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TileSelectionHandler : MonoBehaviour
{
    [Inject] GameManager gameManager;
    [Inject] TilePathFinder matchingPathFinder;
    [Inject] Board board;
    [SerializeField] Transform emptyTile;
    [SerializeField] PopingTile tilePop;
    private GameObject selectedTile1;
    private GameObject selectedTile2;
    public static event Action OnTilesMatch;

    private const float pressScale = 0.85f; // how much to shrink on press
    private const float pressDuration = 0.1f; // how fast the press animates

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameManager.gameState == GameManager.GameState.Playing)
        {
            HandleSelection();
        }
    }

    void HandleSelection()
    {
        Vector2 raycastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.zero);

        if (selectedTile1 == null)
        {
            HandleFirstSelection(hit);
        }
        else
        {
            HandleSecondSelection(hit);
        }
    }

    void HandleFirstSelection(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            selectedTile1 = hit.collider.gameObject;
            selectedTile1.GetComponent<SpriteRenderer>().material.color -= new Color(0, 0, 0, 0.3f);

            // Play select sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySelectTileSound();

            // 🔹 Add press animation
            StartCoroutine(PressAnimation(selectedTile1.transform));
        }
    }

    void HandleSecondSelection(RaycastHit2D hit)
    {
        if (hit.collider != null && hit.collider.gameObject != selectedTile1.gameObject)
        {
            selectedTile2 = hit.collider.gameObject;
            selectedTile2.GetComponent<SpriteRenderer>().material.color -= new Color(0, 0, 0, 0.3f);

            // Play select sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySelectTileSound();

            // 🔹 Add press animation
            StartCoroutine(PressAnimation(selectedTile2.transform));

            if (selectedTile1.GetComponent<Tile>().name == selectedTile2.GetComponent<Tile>().name)
            {
                matchingPathFinder.renderLine = true;
                if (matchingPathFinder.IsPathValid(selectedTile1.GetComponent<Tile>(), selectedTile2.GetComponent<Tile>()))
                {
                    GameObject firstTilePop = Instantiate(tilePop.gameObject, selectedTile1.transform.position, Quaternion.identity);
                    firstTilePop.GetComponent<SpriteRenderer>().sprite = selectedTile1.GetComponent<SpriteRenderer>().sprite;
                    GameObject secondTilePop = Instantiate(tilePop.gameObject, selectedTile2.transform.position, Quaternion.identity);
                    secondTilePop.GetComponent<SpriteRenderer>().sprite = selectedTile2.GetComponent<SpriteRenderer>().sprite;

                    selectedTile1.GetComponent<Tile>().MakeEmpty();
                    selectedTile2.GetComponent<Tile>().MakeEmpty();

                    selectedTile1 = null;
                    selectedTile2 = null;
                    matchingPathFinder.board = board;
                    TilesMatch();
                }
                else
                {
                    ResetTileVisual(selectedTile1);
                    selectedTile1 = selectedTile2;
                    selectedTile2 = null;
                }
            }
            else
            {
                ResetTileVisual(selectedTile1);
                selectedTile1 = selectedTile2;
                selectedTile2 = null;
            }
        }
    }

    void ResetTileVisual(GameObject tile)
    {
        if (tile == null) return;
        tile.GetComponent<SpriteRenderer>().material.color += new Color(0, 0, 0, 0.3f);
        tile.transform.localScale = Vector3.one; // 🔹 restore scale
    }

    public void TilesMatch()
    {
        OnTilesMatch?.Invoke();
    }

    // 🔹 Coroutine to create press/pop animation
    IEnumerator PressAnimation(Transform tile)
    {
        Vector3 originalScale = tile.localScale;
        Vector3 pressedScale = originalScale * pressScale;
        float t = 0f;

        // Scale down
        while (t < pressDuration)
        {
            t += Time.deltaTime;
            tile.localScale = Vector3.Lerp(originalScale, pressedScale, t / pressDuration);
            yield return null;
        }

        t = 0f;
        // Scale back up
        while (t < pressDuration)
        {
            t += Time.deltaTime;
            tile.localScale = Vector3.Lerp(pressedScale, originalScale, t / pressDuration);
            yield return null;
        }

        tile.localScale = originalScale;
    }
}
