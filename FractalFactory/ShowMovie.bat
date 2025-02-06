@echo off
set movpath=%1
set wait=%2

echo ShowMovie %movpath% %wait%
echo.

IF %movpath%.==. GOTO Invocation

"C:\Program Files\Windows Media Player\wmplayer.exe" %movpath%
GOTO WaitForInput

:Invocation
echo ShowMovie fully_qualified_movie_path

:WaitForInput
IF NOT %wait%==0 set /p DUMMY=Hit ENTER to continue...

:TheEnd
