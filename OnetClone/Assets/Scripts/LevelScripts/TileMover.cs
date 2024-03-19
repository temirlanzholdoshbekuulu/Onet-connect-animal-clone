using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMover
{
	private ICollapseStrategy movementStrategy;

	
	public void SetMovementStrategy(ICollapseStrategy strategy)
	{
		this.movementStrategy = strategy;
	}
	public ICollapseStrategy GetMovementStrategy()
    {
        return this.movementStrategy;
    }

	public void MoveTilesBasedOnLevel()
    {
        if (movementStrategy != null)
        {
            movementStrategy.MoveTiles();
        }
    }
}
