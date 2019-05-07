using System;
using System.Collections.Generic;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class ElementFactory
    {
        public static IElement Create<T>(int size) where T : IElement
        {
            if(typeof(T) == typeof(IRouteElement))
            {
                return new RouteElement(size);
            }
            else if(typeof(T) == typeof(IElement))
            {
                return new Element(size);
            }
            throw new ArgumentException();
        }
        public static IElement CreateElement<T>(IEnumerable<int> data) where T : IElement
        {
            if (typeof(T) == typeof(IRouteElement))
            {
                return new RouteElement(data);
            }
            else if (typeof(T) == typeof(IElement))
            {
                return new Element(data);
            }
            throw new ArgumentException();
        }
    }
}
