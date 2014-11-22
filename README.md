ClickOnceCacheClear
===================

Simple application that backs up and clears the ClickOnce application cache of anything matching the specified Regex expression search terms in the App.config's pipe delimited 'SearchTerms' variable. You'll need to modify the default search terms included in the App.config to suite your needs.

For situations where "Mage.exe -cc" and rundll32 %windir%\system32\dfshim.dll CleanOnlineAppCache" don't do the job.

### What it backs up

The application will force you to back up the ClickOnce application cache prior to any clean up.

The location '%userprofile%\AppData\Local\Apps\2.0' is backed up to a zip archive that you specify.

The registry key 'HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0' is backed up to a .reg registry file.

### What it searches

The application will search the location '%userprofile%\AppData\Local\Apps\2.0', and any sub-folders, for any file or directory that matches the regular expression search terms defined in the App.config's pipe delimited 'SearchTerms' variable.

The application will also search the ClickOnce application cache registry key locations below for any sub-keys that match the regular expression search terms defined in the App.config's pipe delimiated 'SearchTerms' variable.

* HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Assemblies
* HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Categories
* HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Components
* HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Installations
* HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Marks
* HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\StateManager\Applications
* HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\StateManager\Families
* HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Visibility
* HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\VisibilityRoots
* HKEY_CURRENT_USER\Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\PackageMetadata

## What it deletes

It will delete anything matched using the methods above.

## Requirements

Requires .NET Framework 4.0

## Disclaimer

This was written quickly and as a small utility to fix issues with specific application ClickOnce deployment issues, so it may not work correctly for all ClickOnce deployment issues. 

You may need to browse your existing ClickOnce application cache to figure out what search terms you should use for your specific issue.
