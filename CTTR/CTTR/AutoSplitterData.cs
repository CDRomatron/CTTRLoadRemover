// This is a minimum example for adding autosplitter support to a game written
// in C#. You should make any and all details related to this struct public
// that way someone working on an autosplitter doesn't have to figure this out
// for themselves. You should also exclude this from any obfuscation you might
// be using.

using System.Runtime.InteropServices;

// Do not remove this attribute as its needed to ensure stuff is laid out
// in memory correctly.
[StructLayout(LayoutKind.Sequential)]
static class AutoSplitterData
{
	// This value will be used for a signature scan. You should not modify it.
	public static long magicNumber = 0x1337133713371337;
	// This should match any in game timer you have.
	public static double inGameTime = 0;
	// Set to 1 if the game is currently loading. 0 if otherwise.
	public static int isLoading = 0;
	// Update this every time the level changes.
	public static int levelID = 0;
	// Set this to 1 if an attempt was started.
	// Set this to 2 if the game ended.
	// Set this to 0 if an attempt is ended early.
	public static int isRunning = 0;
}
