using System.Globalization;
using System.Xml;

namespace GoogleDirections
{
  /// <summary>
  /// Class representing a latitude/longitude pair
  /// </summary>
  public class LatLng
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="LatLng"/> class.
    /// </summary>
    /// <param name="latitude">The latitude.</param>
    /// <param name="longitude">The longitude.</param>
    public LatLng(double latitude, double longitude)
    {
      this.latitude = latitude;
      this.longitude = longitude;
    }

    internal LatLng(XmlElement locationElement)
    {
      latitude = double.Parse(locationElement.SelectSingleNode("lat").InnerText, CultureInfo.InvariantCulture);
      longitude = double.Parse(locationElement.SelectSingleNode("lng").InnerText, CultureInfo.InvariantCulture);
    }

    private double latitude;
    /// <summary>
    /// Gets the latitude.
    /// </summary>
    public double Latitude
    {
      get
      {
        return latitude;
      }
    }

    private double longitude;
    /// <summary>
    /// Gets the longitude.
    /// </summary>
    public double Longitude
    {
      get
      {
        return longitude;
      }
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public override string ToString()
    {
      return latitude.ToString() + ", " + longitude.ToString();
    }
  }
}
