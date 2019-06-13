
rem echo OFF

set JAVA_HOME=
set JAVA_EXEC=%JAVA_HOME%java
set JAVA_EXEC=C:\Program Files\Java\jdk1.8.0_211\bin\java

SHIFT

set OCI_LOGINID=%0
set OCI_PASSWORD=%1

SHIFT
SHIFT

rem set cmd=%JAVA_EXEC% -cp lib\VoxOciClient.jar -DOCI_LOGINID=%OCI_LOGINID% -DOCI_PASSWORD=%OCI_PASSWORD% com.broadsoft.clients.oci.OCIClient %0 %1 %2 %3 %4 %5 %6 %7 %8 %9  
set cmd=%JAVA_EXEC% -jar lib\VoxOciClient.jar -DOCI_LOGINID=%OCI_LOGINID% -DOCI_PASSWORD=%OCI_PASSWORD% com.broadsoft.clients.oci.OCIClient %0 %1 %2 %3 %4 %5 %6 %7 %8 %9  
%cmd%

rem echo ON
rem exit