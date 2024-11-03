# Working File Watcher
This application is designed to watch files and directories for changes and copy them to your active projects. The idea being you might have raw source assets in your 'working files' folder like max, psd and so on that you would not like to ship with your final application however assets which are exported from the source like PNG assets you do. To allow you to reference these within the engine this application will spot changes in these files and copy (and replace) them in the engine location.

# Features
   1. Watch a single file and copy the file to a directory on change
   2. Read in paths from console or from a file

# In Progress
   1. [[#2](https://github.com/ScottGarryFoster/TOOL-WorkingFileWatcher/issues/2)] Watch a directory for new files adding anything of a particular type
   2. [[#3](https://github.com/ScottGarryFoster/TOOL-WorkingFileWatcher/issues/3)] Relative pathing
   3. [[#4](https://github.com/ScottGarryFoster/TOOL-WorkingFileWatcher/issues/4)] Link to another file within a file for easier distribution
   4. [[#5](https://github.com/ScottGarryFoster/TOOL-WorkingFileWatcher/issues/5)] Args to pass in, instead of entering in commands directly

# How to use
The application is only built for **Windows** and is a console application. Feel free to use the C# code for other applications outside of this.

## Commands
When you load up the application there are three commands:
1. from file
2. directly
3. start

### From File
Upon entering this, the next item to enter should be a file formated for the application to read from file, see below for the format.

#### Loading paths from file
To load a path from file at the moment you need to use the command 'from file' then enter the direct path to the file. The file then looks like the below file, change to suit your needs.
``` xml
<FileWatcher>
    <Files>
        <File 
            Filepath="E:\Development\Game-PacmanInGodot\Assets\Tilesets\Pacman\PacmanBarrierMediumInner.png" 
            Destination="E:\Development\Game-PacmanInGodot\Pacman\Media\Tilesets\"
            />
    </Files>
</FileWatcher>
```
* Files contains all the files to watch. Add as many as you need.
* Filepath and Destination are direct paths.

### Directly
Upon entering this the next thing to enter is the file then the directory.

### Start
Start will start the watcher.

# Final Thoughts
This application is designed to be simple so although I will be adding some features as I need them for my projects, it should not balloon out of control. Also the 'Console' project is separated out from the main logic so feel free to rip out all the 'Windows Console' bit from this and use it else where.