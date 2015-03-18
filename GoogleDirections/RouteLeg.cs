using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GoogleDirections
{
  /// <summary>
  /// Class representing the leg of a route
  /// </summary>
  public class RouteLeg
  {
    internal RouteLeg(XmlElement leg)
    {
      startAddress = leg.SelectSingleNode("start_address").InnerText;
      endAddress = leg.SelectSingleNode("end_address").InnerText;
      distance = int.Parse(leg.SelectSingleNode("distance/value").InnerText);
      duration = int.Parse(leg.SelectSingleNode("duration/value").InnerText);
      startLocation = new LatLng((XmlElement)leg.SelectSingleNode("start_location"));
      endLocation = new LatLng((XmlElement)leg.SelectSingleNode("end_location"));

      XmlNodeList stepsXml = leg.SelectNodes("step");
      List<RouteStep> stepsList = new List<RouteStep>();
      foreach (XmlElement step in stepsXml)
      {
        stepsList.Add(new RouteStep(step));
      }
      steps = stepsList.ToArray();
    }

    private string startAddress;
    /// <summary>
    /// Gets the start address for this leg.
    /// </summary>
    public string StartAddress
    {
      get
      {
        return startAddress;
      }
    }

    private string endAddress;
    /// <summary>
    /// Gets the end address for this leg.
    /// </summary>
    public string EndAddress
    {
      get
      {
        return endAddress;
      }
    }

    private int duration;
    /// <summary>
    /// Gets the duration of this leg in seconds.
    /// </summary>
    public int Duration
    {
      get
      {
        return duration;
      }
    }

    private int distance;
    /// <summary>
    /// Gets the distance of this leg in metres.
    /// </summary>
    public int Distance
    {
      get
      {
        return distance;
      }
    }

    private RouteStep[] steps;
    /// <summary>
    /// Gets the steps for this leg of the route.
    /// </summary>
    public RouteStep[] Steps
    {
      get
      {
        return steps;
      }
    }

    private LatLng startLocation;
    /// <summary>
    /// Gets the start location of this leg of the route.
    /// </summary>
    public LatLng StartLocation
    {
      get
      {
        return startLocation;
      }
    }

    private LatLng endLocation;
    /// <summary>
    /// Gets the end location of this leg of the route.
    /// </summary>
    public LatLng EndLocation
    {
      get
      {
        return endLocation;
      }
    }
  }
}
