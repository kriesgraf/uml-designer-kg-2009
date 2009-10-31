@ECHO OFF
IF (%1) == () GOTO SYNTAX
IF (%2) == () GOTO SYNTAX
IF (%3) == () GOTO SYNTAX

SET AUTORUN_CMD=%3tools\autorun.txt
SET SFX_ADDIN=%3tools\deploy.sfx
SET ARCHIVE=%3%2
SET APPLI=%1\*.*

@echo Command: %0 (%1) (%2)
CALL "C:\Program Files\WinRAR\RAR.exe" a -r -ep1 -sfx%SFX_ADDIN% -z%AUTORUN_CMD% %ARCHIVE% %APPLI%
EXIT 0

:SYNTAX
@ECHO *
@ECHO * Syntax: 
@ECHO *         %0 (publish path) (target) (project path)
@ECHO *

