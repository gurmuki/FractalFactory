using System.Collections.Generic;
using System.Drawing;

namespace FractalFactory.Generators
{
    class ColorTable
    {
        private List<Color> colors;

        public ColorTable(int maxColors)
        {
            colors = new List<Color>();

            int residual = maxColors % 4;
            if (residual > 0)
                maxColors += residual;

            int colorsPerInterval = maxColors / 5;

            int span = (255 - 40) + 1;
            double tmp = (double)span / colorsPerInterval;
            int delta = (int)System.Math.Ceiling(tmp);

            int indx, r, g, b;
            for (indx = 0; indx < colorsPerInterval; ++indx)
            {
                r = 10 + (indx * delta);
                if (r > 255) r = 255;

                b = 40 + (indx * delta);
                if (b > 255) b = 255;

                colors.Add(Color.FromArgb(r, 0, b));
            }

            delta = 256 / colorsPerInterval;

            for (indx = 0; indx < colorsPerInterval; ++indx)
            {
                g = indx * delta;
                if (g > 255) g = 255;

                colors.Add(Color.FromArgb(0, g, 255));
            }

            for (indx = 0; indx < colorsPerInterval; ++indx)
            {
                b = 255 - (indx * delta);
                if (b < 0) b = 0;

                colors.Add(Color.FromArgb(0, 255, b));
            }

            for (indx = 0; indx < colorsPerInterval; ++indx)
            {
                r = indx * delta;
                if (r > 0) r = 255;

                colors.Add(Color.FromArgb(r, 255, 0));
            }

            for (indx = 0; indx < colorsPerInterval; ++indx)
            {
                g = 255 - (indx * delta);
                if (g < 0) g = 0;

                //                colors.Add(Color.FromArgb(255, g, 0));
                colors.Add(Color.FromArgb(255, 0, 0));
            }

            colors.Add(Color.FromArgb(0, 0, 0));
        }

        public Color At(int indx)
        {
            bool inrange = ((indx >= 0) && (indx < colors.Count));
            return (inrange ? colors[indx] : Color.FromArgb(0, 0, 0));
        }
    }
}
