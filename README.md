# CTTRLoadRemover

This program is used to remove loading times in Livesplit for the game "Crash Tag Team Racing". This application bases on a video feed of the gameplay, so is intended for runs performed on a console with a capture card.

## How to use
Download the latest release from https://github.com/CDRomatron/CTTRLoadRemover/releases
Either follow the guide in the provided README.txt, or continue reading below.

### Configuring
First, run the CTTR.exe. Click start and tick the box labeled preview. Increase the X, Y, width, and height do that the top right of the preview window is the top right of the in-game minimap, and the bottom left corner is the bottom left corner of your game feed.

![Example Preview](https://i.imgur.com/4ocsN4X.png)

Once you have aligned the capture, cause the game to enter a black screen. Click stop, then click "save black-screen".

Click start, cause the game to enter a loading screen. Click stop, then click "save load-screen".

Click start again, now you will notice two sets of numbers rapidly changing in the bottom left corner, these numbers is how certain the program is that the screen is black of loading repsectivly. The closer the number is to zero, the more certain the program is.

Note how low the values go in the correct circumstances, then click stop. In the threshold boxes, enter a number which is more than the highest number the program reached in the correct circumstances. 

To test the software, click start, and watch the Is Loading checkbox. If the program believes the game is loading, the box will be ticked, and when it is not loading, it will be unticked.

When you are happy with your configuration, click "Save Settings". You can also load your settings at a later date.

### Running
Now, start Livesplit, go to Edit Layout, and add a "Scriptable Autosplitter". Select CTTR.asl. As long as you have clicked Save Settings in the above step, this should now pause the time during load screens. Please remember to swap your comparision to game time, as it does not affect real time.

## Building from source
Clone the solution, and build. This application does not currently have any dependancies. The file CTTR.asl is not present in the solution, and can be obtained the most recent release.

## Contact
If you are having issues with this application, have questions, or want to suggest improvements, please contact me on [Twitter @CDRomatron](https://twitter.com/CDRomatron), on Discord @CDRomatron (Mike)#6527, on [Speedrun.com @CDRomatron](https://www.speedrun.com/CDRomatron), or leave an issue on the project issue tracker.
