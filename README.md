# AutoWalk
A stardew valley MOD to warp you to speicific locations on map, with time cost.

# Usage
Open the map and click any location that pops up with text, you will be warped to that position. Current time of the game will be increased according to the distance you are warped. 
The ratio is 80 tiles per 10 minutes, which is estimated. 

# Installation
Since this is a Work-In-Progress MOD, the zip file has not been uploaded to Nexus. If you need to use this, follow these tips:
1. Install and install the [SMAPI](https://smapi.io/)
2. Install and install the [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
3. Reference the [Pathoschild.Stardew.ModBuildConfig](https://www.nuget.org/packages/Pathoschild.Stardew.ModBuildConfig) NuGet package
4. download this project and run
   ```
   cd /path/to/this/project
   dotnet build
   ```

# TODOs
- [ ] Support UI parameter selection (ratio)
- [ ] Move output to debug mode
- [ ] Add multiplayer support: waiting for a friend
- [ ] Add horse support: horse cabin not constructed

# Reference
Referred to some implementations in [SDV-FastTravel](https://github.com/Mckenon/SDV-FastTravel). Created a new MOD here because I would like to add some time punishment.
