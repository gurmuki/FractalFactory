@echo off
set vidpath=%1
set vidname=%2
set watermark=%3
set fps=%4
set wait=%5

echo MakeMovie %vidpath% %vidname% %watermark% %fps%
echo.

IF %vidpath%.==. GOTO Invocation
IF %vidname%.==. GOTO Invocation
IF %fps%.==. GOTO Invocation

chdir "%vidpath%"

IF %watermark%=="_ignore_" (
	C:\ffmpeg-5.1.2-essentials_build\bin\ffmpeg -f image2 -r %fps% -i img%%03d.png -vf "pad=ceil(iw/2)*2:ceil(ih/2)*2" -c:v libx264 -pix_fmt yuv420p %vidname%
) ELSE (
	C:\ffmpeg-5.1.2-essentials_build\bin\ffmpeg -f image2 -r %fps% -i img%%03d.png -vf "pad=ceil(iw/2)*2:ceil(ih/2)*2" -c:v libx264 -pix_fmt yuv420p _temp_.mp4
	C:\ffmpeg-5.1.2-essentials_build\bin\ffmpeg -i _temp_.mp4 -i %watermark% -filter_complex "overlay=10:10" %vidname%
	del "_temp_.mp4" 2>nul 
)

GOTO WaitForInput

:Invocation
echo MakeMovie video_folder video_name fully_qualified_watermak_path frame_rate
echo    fps - a good value is 12

:WaitForInput
IF NOT %wait%==0 set /p DUMMY=Hit ENTER to continue...

:TheEnd
