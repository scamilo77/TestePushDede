cd TestResults

forfiles /s /m *.xml /c "cmd /c move @file .."

pause