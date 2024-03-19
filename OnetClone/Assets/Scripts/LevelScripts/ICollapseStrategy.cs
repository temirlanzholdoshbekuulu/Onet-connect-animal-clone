using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollapseStrategy
{
	event Action OnTilesMoved;
	void MoveTiles();
}
