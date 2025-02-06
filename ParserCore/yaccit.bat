REM // eg. yaccit ExpParser
REM // This generates both ExpParser.cpp & ExpParser.h
byacc -d -l -o ExpParser.cpp ExpParser.y

rm tmp
sed -fExpParser.sed ExpParser.cpp > tmp
rm ExpParser.cpp
mv tmp ExpParser.cpp

sed -fExpParser.sed ExpParser.h > tmp
rm ExpParser.h
mv tmp ExpParser.h
