using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ReshuffleTiles : MonoBehaviour
{
    [Inject] GameManager gameManager;
    [Inject] TileSpawner tileSpawner;
    [Inject] TilesMatchChecker tilesMatchChecker;
    [SerializeField] GameObject grid;
    [SerializeField] TextMeshProUGUI remainedShuffelsText;
    private bool isShuffling = false;

    void Awake()
    {
        grid = GameObject.Find("Grid");
    }

    void Update()
    {
        remainedShuffelsText.text = gameManager.remainedShuffles.ToString();
    }

    public void ReshuffleButton()
    {
        Reshuffle();
    }

    public bool Reshuffle()
    {
        if (isShuffling || gameManager.remainedShuffles <= 0 || 
            gameManager.gameState != GameManager.GameState.Playing)
        {
            return false;
        }

        gameManager.remainedShuffles--;
        StartCoroutine(ShuffleRoutine());
        return true;
    }

    private IEnumerator ShuffleRoutine()
    {
        isShuffling = true;
        int maxAttempts = 10; // Prevent infinite loop
        int attempts = 0;
        bool hasValidMatch = false;

        // Get all non-empty tiles
        List<Tile> tiles = new List<Tile>();
        for (int x = 0; x < tileSpawner.gridWidth; x++)
        {
            for (int y = 0; y < tileSpawner.gridHeight; y++)
            {
                var tile = tileSpawner.tiles[x, y];
                if (tile != null && !tile.isEmpty)
                {
                    tiles.Add(tile);
                }
            }
        }

        if (tiles.Count > 1)
        {
            // Keep shuffling until we find a valid match or hit max attempts
            while (!hasValidMatch && attempts < maxAttempts)
            {
                attempts++;
                Debug.Log($"Shuffle attempt {attempts}");

                // Store the original data
                var sprites = new List<Sprite>();
                var names = new List<string>();
                foreach (var tile in tiles)
                {
                    var sr = tile.GetComponent<SpriteRenderer>();
                    sprites.Add(sr.sprite);
                    names.Add(tile.name);
                }

                // Shuffle the data
                for (int i = sprites.Count - 1; i > 0; i--)
                {
                    int k = Random.Range(0, i + 1);
                    
                    var tempSprite = sprites[i];
                    sprites[i] = sprites[k];
                    sprites[k] = tempSprite;

                    var tempName = names[i];
                    names[i] = names[k];
                    names[k] = tempName;
                }

                // Apply shuffled data
                for (int i = 0; i < tiles.Count; i++)
                {
                    var sr = tiles[i].GetComponent<SpriteRenderer>();
                    sr.sprite = sprites[i];
                    tiles[i].name = names[i];
                }

                yield return new WaitForSeconds(0.1f);

                // Check if there's at least one valid match
                hasValidMatch = tilesMatchChecker.HasAnyMatch();
                if (!hasValidMatch)
                {
                    Debug.Log("No valid matches found, reshuffling...");
                }
            }

            if (!hasValidMatch)
            {
                Debug.LogWarning("Failed to find a valid match after maximum attempts!");
                gameManager.GameOver();
            }
            else
            {
                Debug.Log($"Found valid match after {attempts} attempts");
            }
        }

        isShuffling = false;
    }
}
