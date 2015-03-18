using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GoogleDirections
{
  /// <summary>
  /// Class representing a route containing directions between various locations
  /// </summary>
  public class Route
  {
    internal Route (XmlDocument route)
    {
      summary = route.DocumentElement.SelectSingleNode("route/summary").InnerText;
      XmlNodeList legsXml = route.DocumentElement.SelectNodes("route/leg");
      List<RouteLeg> legsList = new List<RouteLeg>();
      foreach (XmlElement leg in legsXml)
      {
        legsList.Add(new RouteLeg(leg));
      }
      legs = legsList.ToArray();
    }

    private string summary;
    /// <summary>
    /// Gets a summary of the roads used in the calculated route.
    /// </summary>
    public string Summary
    {
      get
      {
        return summary;
      }
    }

    private RouteLeg[] legs;
    /// <summary>
    /// Gets the legs of this route.
    /// </summary>
    public RouteLeg[] Legs
    {
      get
      {
        return legs;
      }
    }

    /// <summary>
    /// Gets the duration of the route in seconds.
    /// </summary>
    public int Duration
    {
      get
      {
        int duration = 0;
        for (int i = 0; i < legs.Length; i++)
        {
          duration += legs[i].Duration;
        }
        return duration;
      }
    }

    /// <summary>
    /// Gets the distance of the route in metres.
    /// </summary>
    public int Distance
    {
      get
      {
        int distance = 0;
        for (int i = 0; i < legs.Length; i++)
        {
          distance += legs[i].Distance;
        }
        return distance;
      }
    }
  }
}
