del xslt\styledata.xml
del xslt\~*.*
del xslt\*.bak

mkdir bin\Debug\xslt
copy xslt\*.x* bin\Debug\xslt
copy xslt\*.GIF bin\Debug\xslt

mkdir bin\Release\xslt
copy xslt\*.x* bin\Release\xslt
copy xslt\*.GIF bin\Release\xslt
