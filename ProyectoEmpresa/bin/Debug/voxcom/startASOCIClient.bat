
echo OFF

set JAVA_HOME=
set JAVA_EXEC=%JAVA_HOME%java

SHIFT

set OCI_LOGINID=%0
set OCI_PASSWORD=%1

SHIFT
SHIFT

set cmd=%JAVA_EXEC% -cp lib\VoxOciClient.jar -DOCI_LOGINID=%OCI_LOGINID% -DOCI_PASSWORD=%OCI_PASSWORD% com.broadsoft.clients.oci.OCIClient %0 %1 %2 %3 %4 %5 %6 %7 %8 %9  
%cmd%

echo ON