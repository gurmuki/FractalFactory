
namespace FractalFactory.Math
{
    public class DomainInterpolator
    {
        private Domain d0;
        private Domain step;

        public DomainInterpolator(Domain startingDomain, Domain endingDomain, int divisions)
        {
            d0 = new Domain(startingDomain);
            step = new Domain(
                (endingDomain.Xmin - startingDomain.Xmin) / divisions,
                (endingDomain.Ymin - startingDomain.Ymin) / divisions,
                (endingDomain.Xmax - startingDomain.Xmax) / divisions,
                (endingDomain.Ymax - startingDomain.Ymax) / divisions);
        }

        public Domain Interpolate(int div)
        {
            return new Domain(
                d0.Xmin + (div * step.Xmin),
                d0.Ymin + (div * step.Ymin),
                d0.Xmax + (div * step.Xmax),
                d0.Ymax + (div * step.Ymax));
        }
    }
}
