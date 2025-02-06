
#include <string>
#include <iostream>
#include "Windows.h"

// LoadLibraryA() and GetProcAddress() are used because that is how you
// must load a dll and call its functions in the absence of having a lib.
extern "C"
{
    typedef int (*fExpParse)(const char* expression);
    typedef std::string(*fExpParseExpressionGet)();
}

int main()
{
    auto hdl = LoadLibraryA("ParserCore.dll");
    if (hdl)
    {
        auto ExpParse = reinterpret_cast<fExpParse>(GetProcAddress(hdl, "ExpParse"));
        if (!ExpParse)
            std::cout << "Cannot find ExpParse()\n";

        auto ExpParseExpressionGet = reinterpret_cast<fExpParseExpressionGet>(GetProcAddress(hdl, "ExpParseExpressionGet"));
        if (!ExpParseExpressionGet)
            std::cout << "Cannot find ExpParseExpressionGet()\n";

        if (ExpParse && ExpParseExpressionGet)
        {
            while (1)
            {
                std::string expr;

                std::cout << "expr:";
                std::getline(std::cin, expr);

                // TODO: Fix the lexer. Whatever the reason, the rule for ignoring whitespace is not working as expected.
                expr.erase(remove_if(expr.begin(), expr.end(), isspace), expr.end());

                if (expr[0] == 'q')
                    break;

                int error = ExpParse(expr.c_str());
                if (error)
                    std::cout << "\n";

                std::string sval = ExpParseExpressionGet();
                std::cout << "val <" << sval << ">\n";
            }
        }

        FreeLibrary(hdl);
    }
    else
    {
        std::cout << "Cannot load ParserCore.dll\n";
    }
}
