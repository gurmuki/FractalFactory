/****************************************************************************
*                     U N R E G I S T E R E D   C O P Y
* 
* You are on day 1 of your 30 day trial period.
* 
* This file was produced by an UNREGISTERED COPY of Parser Generator. It is
* for evaluation purposes only. If you continue to use Parser Generator 30
* days after installation then you are required to purchase a license. For
* more information see the online help or go to the Bumble-Bee Software
* homepage at:
* 
* http://www.bumblebeesoftware.com
* 
* This notice must remain present in the file. It cannot be removed.
****************************************************************************/

/****************************************************************************
* ExpLexer.h
* C++ header file generated from ExpLexer.l.
* 
* Date: 06/24/04
* Time: 09:53:36
* 
* ALex Version: 2.07
****************************************************************************/

#ifndef _EXPLEXER_H
#define _EXPLEXER_H

#include <yyclex.h>

#line 8 ".\\ExpLexer.l"

class CExpParser;

#line 37 "ExpLexer.h"
/////////////////////////////////////////////////////////////////////////////
// CExpLexer

#ifndef YYEXPLEXER
#define YYEXPLEXER
#endif

class YYEXPLEXER YYFAR CExpLexer : public _YL yyflexer {
public:
	CExpLexer();
	virtual ~CExpLexer();

	// backards compatibility with lex
#ifdef input
	virtual int yyinput();
#endif
#ifdef output
	virtual void yyoutput(int ch);
#endif
#ifdef unput
	virtual void yyunput(int ch);
#endif

protected:
	void yytables();
	virtual int yyaction(int action);

public:
#line 14 ".\\ExpLexer.l"

	// Operations
	public:
		BOOL	Init( CExpParser* in_parser );

		void	setString( const CString& in_str )	{ m_buf = in_str; m_buf_idx = 0; }

		char*	getToken( void )					{ return yytext; }

		// Attributes
	private:
		int yygetchar( void );

		CString	m_buf;
		int		m_buf_idx;

#line 83 "ExpLexer.h"
};

#ifndef YYLEXERNAME
#define YYLEXERNAME CExpLexer
#endif

#ifndef INITIAL
#define INITIAL 0
#endif

#endif
