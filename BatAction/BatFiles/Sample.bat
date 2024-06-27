@ECHO OFF
ECHO executed bat--- trigger is SourceId=%1, SourceName=%2, SourceType=%3, >> C:\ProgramData\Milestone\execute.log
:Loop
IF [%4]==[] GOTO Continue
   ECHO -        bat--- camera is %4 >> C:\ProgramData\Milestone\execute.log
SHIFT
GOTO Loop
:Continue