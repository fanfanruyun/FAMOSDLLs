DLL configuration in FAMOS
==========================
(Found in a ".def" file in "C:\ProgramData\imc\Common\Def" - e.g. "C:\ProgramData\imc\Common\Def\ExternalDLLs.def")
DLL32 "ExternalDLLSample.C,AddTwoNumbers.AddTwoNumbers, int.ne , int.ne , int.ne "
DLL32 "ExternalDLLSample.C,AddTwoStrings.AddTwoStrings,p text.tx ,p text.tx ,p text.tx "
DLL32 "ExternalDLLSample.C,GimmeAScript.GimmeAScript,p text.tx "
DLL32 "ExternalDLLSample.C,SendAWave.SendAWave, void.vd ,rp dsf.ne *"
DLL32 "ExternalDLLSample.C,GetAWave.GetAWave,h dsf.ne *,p text.tx "

To Debug
========
 - Run Visual Studio as an Administrator
 - Make sure "Build --> Platform target" is set to "x86" explicitly (NOT "Any CPU" - that won't work)
 - Set "Build --> Output path" to "C:\Program Files (x86)\imc\shared\Extensions\"
 - Set "Debug --> Start Action" to "Start external program", "C:\Program Files (x86)\imc\Famos\Bin\Famos.exe"
 - Set "Debug --> Start Options --> Command line arguments" to "/VE"
 - Configure the external DLL in FAMOS as per the above
 - Set a breakpoint and hit [F5]
