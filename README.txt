Nunit growl notificator for windows

to build the project:
msbuild NUnitGrowlAddin.sln /p:Configuration=Debug /p:Platform="Any CPU"

to install the project:
put the NunitGrowlAddin.dll, the NunitGrowlAddIn.pdb files and the Resource directory  NUNIT-INSTALLATIONPATH\bin\net-2.0\addins
put there also:

Growl.Connector.dll
Growl.CoreLibrary.dll


Tested on nunit 2.5.7.

binaries for nunit 2.5.7 included.


blog: http://tonyxzt.blogspot.com/2010/11/nunit-growl-notification.html



