#pragma once

enum TType
{
	Garbage = 0, N = 1, V = 2, NN = 11, NV = 12, VN = 21, VV = 22
};

struct TokenData
{
	TType  type;
	double coeff;
	double expon;
};
