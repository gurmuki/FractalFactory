@echo off
set imgpath=%1
set wait=%2

echo OpenPicture %imgpath% %wait%
echo.

IF %imgpath%.==. GOTO Invocation

REM Ugh, f'ing NanoMush sure didn't make this easy to discover. The answer to opening an image with their Photos App
REM was found at https://superuser.com/questions/1414402/command-line-interface-for-photos-app-in-windows-10
REM But there is was an additional problem: the app does not like the '^' character in the file name, even though
REM (by NanoMush's documentation) it is a valid file name character. Quoting the file name solves solves that problem,
C:\Windows\system32\explorer.exe %imgpath%
GOTO WaitForInput

:Invocation
echo OpenPicture fully_qualified_image_path

:WaitForInput
IF NOT %wait%==0 set /p DUMMY=Hit ENTER to continue...

:TheEnd
