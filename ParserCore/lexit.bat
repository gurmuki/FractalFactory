REM // eg. lexit ExpLexer (formerly)
REM // This generates ExpLexer.cpp
flex -L -oExpLexer.cpp ExpLexer.l
rm tmp
sed -fExpLexer.sed ExpLexer.cpp > tmp
rm ExpLexer.cpp
mv tmp ExpLexer.cpp
