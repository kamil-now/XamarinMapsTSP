using System.Linq;

namespace XamarinTSP.GoogleMapsApi
{
    internal class DistanceMatrixResponse
    {
        public string Status { get; set; }
        public string[] Origin_Addresses { get; set; }
        public string[] Destination_Addresses { get; set; }
        public Row[] Rows { get; set; }
        public DistanceMatrixResponse() { }
        public DistanceMatrixResponse Merge(DistanceMatrixResponse response)
        {
            Status = response.Status;
            Origin_Addresses = Origin_Addresses.Concat(response.Origin_Addresses).ToArray();

            Destination_Addresses = Destination_Addresses.Concat(response.Destination_Addresses).ToArray();
            Rows = Rows.Concat(response.Rows).ToArray();
            return this;
        }
    }

    internal class Row
    {
        public Element[] Elements { get; set; }
    }

    internal class Element
    {
        public string Status { get; set; }
        public Item Duration { get; set; }
        public Item Distance { get; set; }
    }

    internal class Item
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}
