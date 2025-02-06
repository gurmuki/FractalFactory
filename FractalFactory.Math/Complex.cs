// Courtesy of https://www.codeproject.com/Articles/38514/The-beauty-of-fractals-A-simple-fractal-rendering

using System;

// Complex Numbers in Polar Form and DeMoivre's Theorem (from https://www.tjhsst.edu/~dhyatt/supercomp/n108.html)
//    For some computations, a different form of the complex number might be more useful.
//    For instance, complex numbers can also be expressed in polar form where the point is
//    specified by its radius r from the origin and some angle theta. The real and imaginary
//    components can then be expressed in terms of the sine and cosine of that angle multiplied
//    by the radius. For instance, a point Z that is r units from the origin at an angle of
//    theta degrees can be expressed in the form:
//
//        Z = r * [cos(theta) + i * sin(theta)]
//
//    This may seem rather difficult, but there is a real advantage of this form when working
//    with fractional powers. There is a famous rule called DeMoivre's Theorem that states that
//    a complex number in polar form can be raised to a power through the following process:
//
//        Z^n = [r(cos(theta) + i * sin(theta))]^n
//            = r^n * (cos(n * theta) + i * sin(n * theta))
//
//    This form can be very helpful when calculating certain powers for complex numbers since
//    the real component is [r^n * cos(n*theta)] and the imaginary part is [r^n * sin(n*theta)]
//    even if n is some strange fractional exponent.

namespace FractalFactory.Math
{
    public struct Complex : IComparable
    {
        private const double ALMOST_ZERO = 1e-16;

        static readonly private double halfOfRoot2 = 0.5 * System.Math.Sqrt(2);
        static readonly public Complex Zero = new Complex(0, 0);
        static readonly public Complex I = new Complex(0, 1);
        static readonly public Complex MaxValue = new Complex(double.MaxValue, double.MaxValue);
        static readonly public Complex MinValue = new Complex(double.MinValue, double.MinValue);

        public double Re;
        public double Im;

        public Complex(double real, double imaginary)
        {
            this.Re = (double)real;
            this.Im = (double)imaginary;
        }

        public Complex(Complex c)
        {
            this.Re = c.Re;
            this.Im = c.Im;
        }

        static public Complex CreateFromRealAndImaginary(double re, double im)
        {
            Complex c;
            c.Re = (double)re;
            c.Im = (double)im;
            return c;
        }

        static public Complex CreateFromModulusAndArgument(double mod, double arg)
        {
            Complex c;
            c.Re = (double)(mod * System.Math.Cos(arg));
            c.Im = (double)(mod * System.Math.Sin(arg));
            return c;
        }

        static public Complex Sqrt(Complex c)
        {
            double x = c.Re;
            double y = c.Im;
            double modulus = System.Math.Sqrt(x * x + y * y);

            int sign = (y < 0) ? -1 : 1;

            c.Re = (double)(halfOfRoot2 * System.Math.Sqrt(modulus + x));
            c.Im = (double)(halfOfRoot2 * sign * System.Math.Sqrt(modulus - x));

            return c;
        }

        static public Complex Pow(Complex c, double exponent)
        {
            double x = c.Re;
            double y = c.Im;

            double modulus = System.Math.Pow(x * x + y * y, exponent * 0.5);
            double argument = System.Math.Atan2(y, x) * exponent;

            c.Re = (double)(modulus * System.Math.Cos(argument));
            c.Im = (double)(modulus * System.Math.Sin(argument));

            return c;
        }

        public double GetModulus()
        {
            double x = this.Re;
            double y = this.Im;
            return (double)System.Math.Sqrt(x * x + y * y);
        }

        public double GetModulusSquared()
        {
            return (double)this.Re * this.Re + this.Im * this.Im;
        }

        public double GetArgument()
        {
            return (double)System.Math.Atan2(this.Im, this.Re);
        }

        public Complex GetConjugate()
        {
            return CreateFromRealAndImaginary(this.Re, -this.Im);
        }

        public void Normalize()
        {
            double modulus = this.GetModulus();
            if (modulus == 0)
            {
                throw new DivideByZeroException();
            }
            this.Re = (double)(this.Re / modulus);
            this.Im = (double)(this.Im / modulus);
        }

        public static explicit operator Complex(double d)
        {
            Complex c;
            c.Re = (double)d;
            c.Im = (double)0;
            return c;
        }

        public static explicit operator double(Complex c)
        {
            return (double)c.Re;
        }

        public static bool operator ==(Complex a, Complex b)
        {
            return (a.Re == b.Re) && (a.Im == b.Im);
        }

        public static bool operator !=(Complex a, Complex b)
        {
            return (a.Re != b.Re) || (a.Im != b.Im);
        }

        public override int GetHashCode()
        {
            return (this.Re.GetHashCode() ^ this.Im.GetHashCode());
        }

        public override bool Equals(object? o)
        {
            if (o is Complex)
            {
                Complex c = (Complex)o;
                return (this == c);
            }
            return false;
        }

        public int CompareTo(object? o)
        {
            if (o == null)
            {
                return 1;
            }
            else if (o is Complex)
            {
                return this.GetModulus().CompareTo(((Complex)o).GetModulus());
            }
            else if (o is double)
            {
                return this.GetModulus().CompareTo((double)o);
            }
            else if (o is float)
            {
                return this.GetModulus().CompareTo((float)o);
            }
            throw new ArgumentException();
        }

        public static Complex operator +(Complex a)
        {
            return a;
        }

        public static Complex operator +(Complex a, double f)
        {
            // a.Re = (double)(a.Re + f);
            // return a;
            return new Complex((double)(a.Re + f), a.Im);
        }

        public static Complex operator +(double f, Complex a)
        {
            // a.Re = (double)(a.Re + f);
            // return a;
            return new Complex((double)(a.Re + f), a.Im);
        }

        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex((double)(a.Re + b.Re), (double)(a.Im + b.Im));
        }

        public static Complex operator -(Complex a)
        {
            return new Complex(-a.Re, -a.Im);
        }

        public static Complex operator -(Complex a, double f)
        {
            // a.Re	= (double)( a.Re - f );
            // return a;
            return new Complex((double)(a.Re - f), a.Im);
        }

        public static Complex operator -(double f, Complex a)
        {
            // a.Re	= (float)( f - a.Re );
            // a.Im	= (float)( 0 - a.Im );
            // return a;
            return new Complex((double)(f - a.Re), (double)(0 - a.Im));
        }

        public static Complex operator -(Complex a, Complex b)
        {
            return new Complex((double)(a.Re - b.Re), (double)(a.Im - b.Im));
        }

        public static Complex operator *(Complex a, double f)
        {
            return new Complex((double)(a.Re * f), (double)(a.Im * f));
        }

        public static Complex operator *(double f, Complex a)
        {
            return new Complex((double)(a.Re * f), (double)(a.Im * f));
        }

        public static Complex operator *(Complex a, Complex b)
        {
            double x = a.Re, y = a.Im;
            double u = b.Re, v = b.Im;

            return new Complex((double)(x * u - y * v), (double)(x * v + y * u));
        }

        public static Complex operator /(Complex a, double f)
        {
            if (f == 0)
            {
                throw new DivideByZeroException();
            }

            // a.Re	= (double)( a.Re / f );
            // a.Im	= (double)( a.Im / f );
            // return a;
            return new Complex((double)(a.Re / f), (double)(a.Im / f));
        }

        public static Complex operator /(Complex a, Complex b)
        {
            double x = a.Re, y = a.Im;
            double u = b.Re, v = b.Im;
            double denom = u * u + v * v;

            if (denom == 0)
            {
                throw new DivideByZeroException();
            }

            // a.Re	= (double)( ( x*u + y*v ) / denom );
            // a.Im	= (double)( ( y*u - x*v ) / denom );
            // return a;
            return new Complex((double)((x * u + y * v) / denom), a.Im = (double)((y * u - x * v) / denom));
        }

        public static Complex operator ^(Complex a, double exp)
        {
            Complex result = new Complex(a);
            if ((exp > 0) && ((exp - (int)exp) <= ALMOST_ZERO))
            {
                int count = (int)exp;
#if true
                while (count > 1)
                {
                    result = result * a;
                    --count;
                }
#else
				// This is more efficient. Perhaps I can find a general solution?
				if (count == 2)
				{
					result = (a * a);
				}
				else if (count == 3)
				{
					result = (a * a) * a;
				}
				else if (count == 4)
				{
					Complex a2 = a * a;
					result = a2 * a2;
				}
				else if (count == 5)
				{
					Complex a2 = a * a;
					Complex a4 = a2 * a2;
					result = a4 * a;
				}
				else if (count == 6)
				{
					Complex a2 = a * a;
					result = (a2 * a2) * a2;
				}
				else if (count == 7)
				{
					Complex a3 = (a * a) * a;
					result = (a3 * a3) * a;
				}
				else if (count == 8)
				{
					Complex a2 = a * a;
					Complex a4 = a2 * a2;
					result = a4 * a4;
				}
#endif
            }
            else
            {
                result = Pow(result, exp);
            }

            return result;
        }

        public override string ToString()
        {
            return $"( {this.Re}, {this.Im}i )";
        }

        static public bool IsEqual(Complex a, Complex b, double tolerance)
        {
            return (System.Math.Abs(a.Re - b.Re) < tolerance) &&
                    (System.Math.Abs(a.Im - b.Im) < tolerance);
        }
    }

}
