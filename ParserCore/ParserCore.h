// ParserCore.h : main header file for the ParserCore DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CParserCoreApp
// See ParserCore.cpp for the implementation of this class
//

class CParserCoreApp : public CWinApp
{
public:
	CParserCoreApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
