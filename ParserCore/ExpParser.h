#define NUMBER 257
#define VAR 258
#define OP_EQ 259
#define OP_LE 260
#define OP_GE 261
#define OP_LT 262
#define OP_GT 263
#define OP_NE 264
#define OP_AND 265
#define OP_OR 266
#define OP_XOR 267
#define OP_MIN 268
#define OP_MAX 269
#define OP_LSHIFT 270
#define OP_RSHIFT 271
#define OP_SIN 272
#define OP_COS 273
#define OP_TAN 274
#define OP_ASIN 275
#define OP_ACOS 276
#define OP_ATAN 277
#define OP_RAD 278
#define OP_DEG 279
#define UMINUS 280
#ifdef EXP_YYSTYPE
#undef  EXP_YYSTYPE_IS_DECLARED
#define EXP_YYSTYPE_IS_DECLARED 1
#endif
#ifndef EXP_YYSTYPE_IS_DECLARED
#define EXP_YYSTYPE_IS_DECLARED 1
#include "TokenData.h"
typedef union
{
	TokenData token;
} EXP_YYSTYPE;
#endif /* !EXP_YYSTYPE_IS_DECLARED */
extern EXP_YYSTYPE exp_yylval;
