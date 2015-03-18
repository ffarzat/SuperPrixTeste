using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GoogleDirections
{
  /// <summary>
  /// Class representing a step within a leg of a route
  /// </summary>
  public class RouteStep
  {
    internal RouteStep(XmlElement step)
    {
      distance = int.Parse(step.SelectSingleNode("distance/value").InnerText);
      duration = int.Parse(step.SelectSingleNode("duration/value").InnerText);
      startLocation = new LatLng((XmlElement)step.SelectSingleNode("start_location"));
      endLocation = new LatLng((XmlElement)step.SelectSingleNode("end_location"));
      htmlInstructions = step.SelectSingleNode("html_instructions").InnerText;
    }

    private int duration;
    /// <summary>
    /// Gets the duration of this step in seconds.
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
    /// Gets the distance in metres for this step.
    /// </summary>
    public int Distance
    {
      get
      {
        return distance;
      }
    }

    private LatLng startLocation;
    /// <summary>
    /// Gets the start location for this step.
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
    /// Gets the end location of this step.
    /// </summary>
    public LatLng EndLocation
    {
      get
      {
        return endLocation;
      }
    }

    private string htmlInstructions;
    /// <summary>
    /// Gets the instructions for this step with HTML formatting.
    /// </summary>
    public string HtmlInstructions
    {
      get
      {
        return htmlInstructions;
      }
    }
  }
}
