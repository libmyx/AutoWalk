using System;
using Microsoft.Xna.Framework;

namespace AutoWalk
{
	/// <summary>A fast travel point on the map.</summary>
	[Serializable]
	public struct FastTravelPoint
	{
		public string Name;
		public int pointX;
		public int pointY;
    }
}