
#ifndef _MATHCONST_H
#define _MATHCONST_H

#include <math.h>  // for fabs()
#include <float.h>


//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// NOTE: When changing values in this header, be sure to
// make the corresponding change in Bbi/System/Const.java
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

const double UNDEFINED    = 1.e9;
const int	 IUNDEFINED	  = 1000000000;

const double SMALL        = 1.e-6;
const double VECTOR_SMALL = 1.e-12;
const double LARGE		  = 1.e8;

const double CLOSED_ENOUGH = 2.e-3;

const double PI           = 3.14159265358979323846;
const double QUARTERPI    = (0.25 * PI);
const double HALFPI       = (0.5 * PI);
const double THREEHALFPI  = (1.5 * PI);
const double TWOPI        = (2.0 * PI);
const double FOURPI       = (4.0 * PI);

const double RAD2DEG      = (180.0 / PI);
const double DEG2RAD      = (PI / 180.0);

const int CW    = -1;
const int CCW   =  1;

const int RIGHT = -1;
const int NONE  =  0;
const int LEFT  =  1;

#define EQUAL( a, b )		(fabs( (double)(a - b) ) < SMALL)
#define CLOSE( a, b, tol )	(fabs( (double)(a - b) ) < tol)
#define ZERO( a )			(fabs( (double) a ) < SMALL)
#define ZEROVEC( a )		(fabs( (double) a ) < VECTOR_SMALL)
#define ONE( a )			(fabs( (double) a ) >= SMALL)
#define DEFINED( a )		( !EQUAL( fabs( (double) a ), UNDEFINED ) )
#define BETWEEN(a, b, c)	((b > (a-SMALL)) && (b < (c+SMALL)))
#define BETWEEN0(a, b, c)	((b > a) && (b < c))

#define SGN( dval )			(ZERO(dval) ? 0 : ((dval < 0) ? -1 : 1))
#define ISGN( ival )		((ival == 0) ? 0 : ((ival < 0) ? -1 : 1))

#if ORIGINAL_CODE

// 2006.09.15 (PE) -- ROUND() was causing part overlap in nesting when
// using large numbers eg. ROUND( 2177.32, 1.e-6 ) because casting it
// to (int) causes a change in sign.
#define ROUND(in_a, in_b)			( (double)((int)(in_a / in_b)) * in_b )

#else

// NOTE: We may have to use ceil() when (in_a / in_b) is negative?
#define ROUND( a, b )  (((a > 0) ? floor( a/b ) : ceil( a/b )) * b)

#endif

// Ordinate indices
const int ORD_X = 0;
const int ORD_Y = 1;
const int ORD_Z = 3;
#define FLIP_XY( ord )				(1-ord)

#endif
