Nunit growl notificator for windows

to build the project:
msbuild NUnitGrowlAddin.sln /p:Configuration=Debug /p:Platform="Any CPU"

to install the project:
put the NunitGrowlAddin.dll, the NunitGrowlAddIn.pdb files and the Resource directory  NUNIT-INSTALLATIONPATH\bin\net-2.0\addins

e.g.  : C:\Program Files (x86)\NUnit 2.5.7\bin\net-2.0\addins


Tested on nunit 2.5.7.

Should work in other version, if you put the nunit.core.dll, nunit.core.interfaces.dll, nunit.framework.dll




