#pragma once

#include <map>
#include <stack>
#include <string>
#include "TokenData.h"

typedef std::map<double, double> ExpCoeffs;

class PolyMath
{
public:

	PolyMath();
	~PolyMath();

	void Reset();

	std::string Expression();

	void Reduce();

	const ExpCoeffs& Terms() const { return terms; }

	TokenData Add(const TokenData& l, const TokenData& r);
	TokenData Subtract(const TokenData& l, const TokenData& r);
	TokenData Multiply(const TokenData& l, const TokenData& r);
	TokenData Divide(const TokenData& l, const TokenData& r);
	TokenData Exponentiate(const TokenData& opr, const TokenData& exp);
	TokenData Negate(const TokenData& operand);
	TokenData PushNumber(const TokenData& tok);
	TokenData PushVariable(const TokenData& tok);

private:

	void Push(const TokenData& tok);
	TokenData Pop();

	TType Operands(const TokenData& l, const TokenData& r);

private:

	// A mapping between exponents and their corresponding coefficients.
	ExpCoeffs terms;
	std::stack<TokenData> stack;
};
