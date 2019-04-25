using System.Linq;

namespace XamarinTSP.Utilities
{
    public class DistanceMatrixResponse
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

    public class Row
    {
        public Element[] Elements { get; set; }
    }

    public class Element
    {
        public string Status { get; set; }
        public Item Duration { get; set; }
        public Item Distance { get; set; }
    }

    public class Item
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}
