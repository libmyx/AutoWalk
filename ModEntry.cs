﻿using System.Collections.Generic;
using System.Threading;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Text;
using StardewValley.WorldMaps;
using Microsoft.Xna.Framework;
using System;


namespace AutoWalk
{
    public class ModEntry : Mod
    {
        public static ModConfig Config;
        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            StringBuilder reportDebug = new StringBuilder();
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
		{
			// We need to perform atleast one 10 min clock update before allowing fast travel
			// so set the time back 10 mins and then perform one update when the day has started.
			Game1.timeOfDay -= 10;
			Game1.performTenMinuteClockUpdate();
		}

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
				return;
            if (e.Button == SButton.MouseLeft && Game1.activeClickableMenu is GameMenu menu && menu.currentTab == GameMenu.mapTab)
            {
                int oriX = 0, oriY = 0, newX = 0, newY = 0;
                StringBuilder str = new StringBuilder();
                GameLocation location = Game1.player.currentLocation;
                Point tile = new Point((int)(Game1.player.Position.X/64), (int)(Game1.player.Position.Y/64));
                MapAreaPositionWithContext? mapAreaPos = WorldMapManager.GetPositionData(location, tile);
                if (mapAreaPos != null) {
                    oriX = (int) mapAreaPos.Value.GetMapPixelPosition().X;
                    oriY = (int) mapAreaPos.Value.GetMapPixelPosition().Y;
                    str.Append("from ");
                    str.Append(location.Name);
                    str.Append(": ");
                    str.Append(tile.ToString());
                    str.Append("(");
                    str.Append(oriX);
                    str.Append(",");
                    str.Append(oriY);
                    str.Append(")");
                    this.Monitor.Log(str.ToString(), LogLevel.Warn);
                }
                var mapPage = (Helper.Reflection.GetField<List<IClickableMenu>>(menu, "pages").GetValue()[3]) as MapPage;
                if (mapPage == null) return;
                int x = (int) (e.Cursor.ScreenPixels.X * Game1.options.zoomLevel);
				int y = (int) (e.Cursor.ScreenPixels.Y * Game1.options.zoomLevel);
                foreach (KeyValuePair<string, ClickableComponent> kvp in mapPage.points)
				{
                    ClickableComponent point = kvp.Value;
                    StringBuilder reportDebug = new StringBuilder();
					if (!point.containsPoint(x, y))
						continue;
                    string[] positions = point.name.Split("/");
                    string position = positions[0];
                    List<string> minor_positions = new List<string>() {"AdventureGuild", "Tent", "Railroad", "Blacksmith", "CommunityCenter"};
                    if (minor_positions.Exists(t => t == positions[1]))
                        position = positions[1];
                    int pointX = 0;
                    int pointY = 0;
                    if (Config.FastTravelPoints.Any(t => t.Name == positions[1])) {
                        pointX = Config.FastTravelPoints.First(t => t.Name == positions[1]).pointX;
                        pointY = Config.FastTravelPoints.First(t => t.Name == positions[1]).pointY;
                    }
                    else if (Config.FastTravelPoints.Any(t => t.Name == positions[0])) {
                        pointX = Config.FastTravelPoints.First(t => t.Name == positions[0]).pointX;
                        pointY = Config.FastTravelPoints.First(t => t.Name == positions[0]).pointY;
                    }
                    else {
                        Game1.addHUDMessage(new HUDMessage("place not found", 3));
                        return;
                    }
                    Game1.warpFarmer(position, pointX, pointY, false);
                    Game1.exitActiveMenu();
                    location = Game1.getLocationFromName(position);
                    tile = new Point(pointX, pointY);
                    MapAreaPositionWithContext? mapAreaPos2 = WorldMapManager.GetPositionData(location, tile);
                    if (mapAreaPos2 != null) {
                        str = new StringBuilder();
                        newX = (int) mapAreaPos2.Value.GetMapPixelPosition().X;
                        newY = (int) mapAreaPos2.Value.GetMapPixelPosition().Y;
                        str.Append("to ");
                        str.Append(location.Name);
                        str.Append(": ");
                        str.Append(tile.ToString());
                        str.Append("(");
                        str.Append(newX);
                        str.Append(",");
                        str.Append(newY);
                        str.Append(")");
                        this.Monitor.Log(str.ToString(), LogLevel.Warn);
                    }
                    // (816-432)+(279-114)=549=70min
                    // 10 min = 80 pixel
                    if (mapAreaPos == null || mapAreaPos2 == null)
                        Game1.addHUDMessage(new HUDMessage("secretwood not included", 3));
                    else {
                        int intevTime = ((int) ((Math.Max(newX, oriX) - Math.Min(newX, oriX) +
                        Math.Max(newY, oriY) - Math.Min(newY, oriY)) / 80)) * 10;
                        this.Monitor.Log(intevTime.ToString(), LogLevel.Warn);
                        int curHour = Game1.timeOfDay / 100;
                        int curMin = Game1.timeOfDay % 100;
                        curMin += intevTime % 60;
                        curHour += (int) (intevTime / 60);
                        if (curMin > 60) {
                            curMin -= 60;
                            curHour += 1;
                        }
                        Game1.timeOfDay = curHour * 100 + curMin;
                    }
                    break;
                }
            }
        }
    }   
}
