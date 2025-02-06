
#include <cmath>
#include <sstream>
#include "PolyMath.h"

constexpr double ALMOST_ZERO = 1.e-32;

PolyMath::PolyMath()
{
}

PolyMath::~PolyMath()
{
}

void PolyMath::Reset()
{
	terms.clear();
	
	while (!stack.empty())
	{
		stack.pop();
	}
}

std::string PolyMath::Expression()
{
	std::stringstream ss;

	if (terms.empty() && !stack.empty())
		Reduce();

	ExpCoeffs::reverse_iterator iter = terms.rbegin();
	while (iter != terms.rend())
	{
		double coeff = iter->second;
		double expon = iter->first;

		if (std::abs(coeff) > ALMOST_ZERO)
		{
			if (!ss.str().empty() && (coeff > 0.))
				ss << "+";

			bool unity = (std::abs(1. - coeff) <= ALMOST_ZERO);
			if (!unity)
				ss << coeff;

			if (std::abs(expon) > ALMOST_ZERO)
			{
				if (!unity)
					ss << "*";

				ss << "x";

				if (std::abs(1. - expon) > ALMOST_ZERO)
					ss << "^" << expon;
			}
		}

		++iter;
	}

	return ss.str();
}

void PolyMath::Reduce()
{
	terms.clear();

	while (!stack.empty())
	{
		TokenData term = Pop();

		if (terms.count(term.expon) == 0)
			terms[term.expon] = 0.;

		terms[term.expon] += term.coeff;
	}
}

TokenData PolyMath::Add(const TokenData& l, const TokenData& r)
{
	TokenData result;
	result.type = Garbage;

	if (stack.size() < 2)
		return result;

	TokenData rho = Pop();
	TokenData lho = Pop();

	switch (Operands(lho, rho))
	{
	case NN:
		result.type = N;
		result.coeff = (lho.coeff + rho.coeff);
		result.expon = 0.;
		break;

	case NV:
		break;

	case VN:
		break;

	case VV:
		if (lho.expon == rho.expon)
		{
			result.type = V;
			result.coeff = (lho.coeff + rho.coeff);
			result.expon = lho.expon;
		}
		break;
	}

	if (result.type == Garbage)
	{
		Push(lho);
		Push(rho);
	}
	else
	{
		Push(result);
	}

	return result;
}

TokenData PolyMath::Subtract(const TokenData& l, const TokenData& r)
{
	TokenData result;
	result.type = Garbage;

	if (stack.size() < 2)
		return result;

	TokenData rho = Pop();
	TokenData lho = Pop();

	switch (Operands(lho, rho))
	{
	case NN:
		result.type = N;
		result.coeff = (lho.coeff - rho.coeff);
		result.expon = 0.;
		break;

	case NV:
		break;

	case VN:
		rho.coeff = -rho.coeff;
		break;

	case VV:
		if (lho.expon == rho.expon)
		{
			result.type = V;
			result.coeff = (lho.coeff - rho.coeff);
			result.expon = lho.expon;
		}
		break;
	}

	if (result.type == Garbage)
	{
		Push(lho);
		Push(rho);
	}
	else
	{
		Push(result);
	}

	return result;
}

TokenData PolyMath::Multiply(const TokenData& l, const TokenData& r)
{
	TokenData result;
	result.type = Garbage;

	if (stack.size() < 2)
		return result;

	TokenData rho = Pop();
	TokenData lho = Pop();

	switch (Operands(lho, rho))
	{
	case NN:
		result.type = N;
		result.coeff = lho.coeff * rho.coeff;
		result.expon = 0.;
		break;

	case NV:
		result.type = V;
		result.coeff = lho.coeff * rho.coeff;
		result.expon = rho.expon;
		break;

	case VN:
		result.type = V;
		result.coeff = lho.coeff * rho.coeff;
		result.expon = lho.expon;
		break;

	case VV:
		result.type = V;
		result.coeff = lho.coeff * rho.coeff;
		result.expon = lho.expon + rho.expon;
		break;
	}

	Push(result);

	return result;
}

TokenData PolyMath::Divide(const TokenData& l, const TokenData& r)
{
	TokenData result;
	result.type = Garbage;

	if (stack.size() < 2)
		return result;

	TokenData rho = Pop();
	TokenData lho = Pop();

	if (std::abs(rho.coeff) < ALMOST_ZERO)
		return result;

	switch (Operands(lho, rho))
	{
	case NN:
		result.type = N;
		result.coeff = lho.coeff / rho.coeff;
		result.expon = 0.;
		break;

	case NV:
		result.type = V;
		result.coeff = lho.coeff / rho.coeff;
		result.expon = -rho.expon;
		break;

	case VN:
		result.type = V;
		result.coeff = lho.coeff / rho.coeff;
		result.expon = lho.expon;
		break;

	case VV:
		result.type = V;
		result.coeff = lho.coeff / rho.coeff;
		result.expon = lho.expon - rho.expon;
		break;
	}

	Push(result);

	return result;
}

TokenData PolyMath::Exponentiate(const TokenData& opr, const TokenData& exp)
{
	TokenData result;
	result.type = Garbage;

	if (stack.size() < 2)
		return result;

	TokenData exponent = Pop();
	TokenData operand = Pop();

	switch (Operands(operand, exponent))
	{
	case NN:
		result.type = N;
		result.coeff = pow(operand.coeff, exponent.coeff);
		result.expon = 0.;
		break;

	case NV:
		// not legal
		break;

	case VN:
		result.type = V;
		result.coeff = 1.;
		result.expon = exponent.coeff;
		break;

	case VV:
		// not legal
		break;
	}

	Push(result);

	return result;
}

TokenData PolyMath::Negate(const TokenData& operand)
{
	TokenData oper = Pop();
	oper.coeff = -oper.coeff;
	Push(oper);

	return oper;
}

TokenData PolyMath::PushNumber(const TokenData& tok)
{
	TokenData result;
	result.type = N;
	result.coeff = tok.coeff;
	result.expon = 0.;

	Push(result);

	return result;
}

TokenData PolyMath::PushVariable(const TokenData& tok)
{
	TokenData var;
	var.type = V;
	var.coeff = 1.;
	var.expon = 1.;

	stack.push(var);

	return var;
}

void PolyMath::Push(const TokenData& tok)
{
	stack.push(tok);
}

TokenData PolyMath::Pop()
{
	TokenData tok;
	tok.type = Garbage;

	if (!stack.empty())
	{
		tok = stack.top();
		stack.pop();
	}

	return tok;
}

// lho - left hand operand
// rho - right hand operand
TType PolyMath::Operands(const TokenData& lho, const TokenData& rho)
{
	return (TType)((10 * lho.type) + rho.type);
}
