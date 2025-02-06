1,$ s/exit(.*)/return/g
1,$ s/yy/exp_yy/g
1,$ s/YY/EXP_YY/g
1,$ s/	printf/	TRACE/g
1,$ s/ printf/ TRACE/g
1,$ s/^extern int isatty/\/\/&/g
1,$ s/typedef union/#include "TokenData.h"\ntypedef union/g
