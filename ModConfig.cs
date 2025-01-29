using System;

namespace AutoWalk
{
	[Serializable]
	public class ModConfig
	{
        /// <summary>A list of locations which can be teleported to.</summary>
        public FastTravelPoint[] FastTravelPoints { get; set; }
	}
}