﻿using System.Linq;

namespace XamarinTSP.GoogleMapsApi
{
    internal class DistanceMatrixData
    {
        public int[][] DurationMatrix { get; }
        public int[][] DistanceMatrix { get; }
        public string[] Waypoints { get; }
        public DistanceMatrixData(DistanceMatrixResponse response)
        {
            Waypoints = response.Origin_Addresses;

            var elementsCount = response.Rows.First().Elements.Length;
            DurationMatrix = new int[elementsCount][];
            DistanceMatrix = new int[elementsCount][];
            for (int i = 0; i < elementsCount; i++)
            {
                DurationMatrix[i] = response.Rows[i].Elements.Select(element => element.Duration.Value).ToArray();
                DistanceMatrix[i] = response.Rows[i].Elements.Select(element => element.Distance.Value).ToArray();
            }

        }
    }
}