using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace GoogleDirections
{
  /// <summary>
  /// Static class providing methods to retrieve directions between locations
  /// </summary>
  public static class RouteDirections
  {
    /// <summary>
    /// Gets a route from the Google Maps Directions web service.
    /// </summary>
    /// <param name="optimize">if set to <c>true</c> optimize the route by re-ordering the locations to minimise the
    /// time to complete the route.</param>
    /// <param name="locations">The locations.</param>
    /// <returns>The route</returns>
    public static Route GetRoute(bool optimize, params Location[] locations)
    {
      if (locations.Length < 2)
        throw new ArgumentException("locations parameter must contains 2 or more locations", "locations");

      string reqStr = "origin=" + locations[0].ToString() + "&destination=" + locations[locations.Length-1].ToString();

      if (locations.Length > 2)
      {
        reqStr += "&waypoints=optimize:" + optimize.ToString().ToLower();
        for (int i = 1; i < locations.Length - 1; i++)
        {
          reqStr += "|";
          reqStr += locations[i].ToString();
        }
      }

      return ParseResponse(HttpWebService.MakeRequest(
        "http://maps.googleapis.com/maps/api/directions/xml?sensor=false&" + reqStr));
    }

    private static Route ParseResponse(string response)
    {
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(response);
      string status = xmlDoc.SelectSingleNode("DirectionsResponse/status").InnerText;
      if (status != "OK")
        throw new RoutingException(GetStatusMessage(status));

      return new Route(xmlDoc);
    }

    private static string GetStatusMessage(string status)
    {
      switch (status)
      {
        case "ZERO_RESULTS" : return "No route found";
        case "NOT_FOUND": return "Not found"; 
        // TODO - other status messages
      }
      return status;
    }
  }
}
