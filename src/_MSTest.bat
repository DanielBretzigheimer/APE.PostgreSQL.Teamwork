rem navigate to current directory
cd /d %~dp0

del APE.PostgreSQL.Teamwork.ViewModel.TestResult.trx
"%PROGRAMFILES(x86)%\Microsoft Visual Studio 11.0\Common7\IDE\MSTest.exe" /testcontainer:"APE.PostgreSQL.Teamwork.ViewModel.Test\bin\Release\APE.PostgreSQL.Teamwork.ViewModel.Test.dll" /resultsfile:"APE.PostgreSQL.Teamwork.ViewModel.TestResult.trx" /detail:errormessage /detail:errorstacktrace /detail:stdout /category:!Adminrights
