using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TilePathFinder : MonoBehaviour
{
	[Inject] public Board board;
	[SerializeField] PathRenderer Line;
	public bool renderLine;

	// Directions: right, left, up, down
	private static readonly Vector2Int[] directions = {
		new Vector2Int(1, 0), new Vector2Int(-1, 0),
		new Vector2Int(0, 1), new Vector2Int(0, -1)
	};

	public bool IsPathValid(Tile start, Tile end)
	{
		if (start == end || start.isEmpty || end.isEmpty)
			return false;

		int width = board.gridWidth;
		int height = board.gridHeight;
		var visited = new bool[width, height, 3]; // [x, y, bends]
		var queue = new Queue<PathNode>();
		queue.Enqueue(new PathNode(start, null, 0, -1)); // -1: no direction yet

		while (queue.Count > 0)
		{
			var node = queue.Dequeue();
			int x = Mathf.RoundToInt(node.tile.transform.position.x);
			int y = Mathf.RoundToInt(node.tile.transform.position.y);

			// Bounds check for x, y
			if (x < 0 || x >= width || y < 0 || y >= height)
				continue;
			if (node.bends > 2 || visited[x, y, node.bends])
				continue;
			visited[x, y, node.bends] = true;

			if (node.tile == end)
			{
				if (renderLine)
					RenderPath(node);
				return true;
			}

			for (int dir = 0; dir < 4; dir++)
			{
				int nx = x + directions[dir].x;
				int ny = y + directions[dir].y;
				int newBends = node.direction == -1 || node.direction == dir ? node.bends : node.bends + 1;

				while (nx >= 0 && nx < width && ny >= 0 && ny < height)
				{
					// Defensive: check for null tile
					Tile nextTile = board.tiles[nx, ny];
					if (nextTile == null)
						break;
					if (nextTile != end && !nextTile.isEmpty)
						break;

					if (newBends <= 2 && !visited[nx, ny, newBends])
						queue.Enqueue(new PathNode(nextTile, node, newBends, dir));

					if (nextTile == end)
						break;

					nx += directions[dir].x;
					ny += directions[dir].y;
				}
			}
		}
		return false;
	}

	private void RenderPath(PathNode endNode)
	{
		var points = new List<Vector3>();
		for (var node = endNode; node != null; node = node.prev)
			points.Add(node.tile.transform.position);
		points.Reverse();
		for (int i = 0; i < points.Count - 1; i++)
		{
			var lineInstance = Instantiate(Line, new Vector3(0, 0, -1), Quaternion.identity);
			lineInstance.startPoint = points[i];
			lineInstance.endPoint = points[i + 1];
		}
	}

	private class PathNode
	{
		public Tile tile;
		public PathNode prev;
		public int bends;
		public int direction;
		public PathNode(Tile tile, PathNode prev, int bends, int direction)
		{
			this.tile = tile;
			this.prev = prev;
			this.bends = bends;
			this.direction = direction;
		}
	}
}