# MemoriesLoader
Provides the functionallity to automatically upload pictures from `Sony Alpha 7` Cameras without PlayMemorie's annoying delay.

I once had to set up a pc that automatically receives pictures uploaded by a `Sony Alpha 7` camera.  
At first I looked up for an official program and found a programm called `PlayMemories` provided by Sony.

Sadly this program is struggling with massive delays (about 5 mins per upload) until it starts uploading the pictures, which is very annoying.

For that reason I decided to write a tool that is able to receive those pictures on my own.

## Requirements

Following components need to be installed on your PC in order to use `MemoriesLoader`:
  - Bash
    - Bash needs to be added to `PATH`
    - Gphoto2 needs to be installed and fully updated in your bash-environment  
      > ***Note:***  
      > Have a look at [this Repo][gphoto2Updater] in order to update gphoto2.
  - .NET-Framework as of Version `4.6.1`

Please make sure you have everything installed so you can start using `MemoriesLoader`.

## Preparations
- Download and install [PlayMemories]
- Pair your camera with your PC using PlayMemories
- Uninstall PlayMemories or disable its services  
  > ***Note:***  
  > You need to do this since PlayMemories would block the required ports.
- Grab the latest release of `MemoriesLoader` [from here][LatestRelease] and extract it to your desired location.
- Edit the `Cameras.json`-file.  
  > ***Note:***  
  > This is a set of IP-Addresses and directories to move the received files to.  
  > So let's say you have two camreas with their IP-Addresses set to `10.0.0.1` and `10.0.0.2` then you may want to create a JSON-file looking like this:
  > ```json
  > [
  >     {
  >         "IPAddress": "10.0.0.1",
  >         "Directory": ".\\Sony Alpha I"
  >     },
  >     {
  >         "IPAddress": "10.0.0.2",
  >         "Directory": ".\\Sony Alpha II"
  >     }
  > ]
  > ```

## Usage
Make sure your PC is connected to the same network like your camera.  
Start `MemoriesLoader` and notice the notification-icon appearing in Windows' status-bar.

You can view its log and its log-file by right-clicking onto the icon and choosing the propper menu-item.

The program won't exit if you close the log-window.  
You can exit the application by right-clicking the icon and choosing "Exit".

<!--- References -->
[gphoto2Updater]: https://github.com/gonzalo/gphoto2-updater
[PlayMemories]: http://support.d-imaging.sony.co.jp/www/disoft/int/download/playmemories-home/
[LatestRelease]: https://github.com/manuth/MemoriesLoader/releases/latest