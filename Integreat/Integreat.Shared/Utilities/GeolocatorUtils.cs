using System;
using System.Collections.Generic;
using System.Text;

namespace Integreat.Shared.Utilities
{
    public static class GeolocatorUtils
    {
        public static double CalculateDistance(double latitudeStart, double longitudeStart, double latitudeEnd, double longitudeEnd, DistanceUnits units = DistanceUnits.Miles)
        {
            if (latitudeEnd == latitudeStart && longitudeEnd == longitudeStart)
                return 0;

            double rlat1 = Math.PI * latitudeStart / 180.0;
            double rlat2 = Math.PI * latitudeEnd / 180.0;
            double theta = longitudeStart - longitudeEnd;
            double rtheta = Math.PI * theta / 180.0;
            double dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180.0 / Math.PI;
            var final = dist * 60.0 * 1.1515;
            if (double.IsNaN(final) || double.IsInfinity(final) || double.IsNegativeInfinity(final) ||
                double.IsPositiveInfinity(final) || final < 0)
                return 0;

            if (units == DistanceUnits.Kilometers)
                return MilesToKilometers(final);

            return final;
        }
        public static double MilesToKilometers(double miles) => miles * 1.609344;

        public static double KilometersToMiles(double kilometers) => kilometers * .62137119;

        public enum DistanceUnits
        {
            Kilometers,
            Miles
        }
    }
}
