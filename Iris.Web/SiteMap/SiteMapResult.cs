using System;
using System.Collections;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Iris.Web.SiteMap
{
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class Sitemap
    {
        private ArrayList _map;

        public Sitemap()
        {
            _map = new ArrayList();
        }

        [XmlElement("url")]
        public Location[] Locations
        {
            get
            {
                Location[] items = new Location[_map.Count];
                _map.CopyTo(items);
                return items;
            }
            set
            {
                if (value == null)
                    return;
                Location[] items = (Location[])value;
                _map.Clear();
                foreach (Location item in items)
                    _map.Add(item);
            }
        }

        public int Add(Location item)
        {
            return _map.Add(item);
        }
    }

    public class Location
    {
        public enum EChangeFrequency
        {
            Always,
            Hourly,
            Daily,
            Weekly,
            Monthly,
            Yearly,
            Never
        }

        [XmlElement("loc")]
        public string Url { get; set; }

        [XmlElement("changefreq")]
        public EChangeFrequency? ChangeFrequency { get; set; }
        public bool ShouldSerializeChangeFrequency() { return ChangeFrequency.HasValue; }

        [XmlElement("lastmod",DataType = "date")]
        public DateTime? LastModified { get; set; }
        public bool ShouldSerializeLastModified() { return LastModified.HasValue; }

        [XmlElement("priority")]
        public double? Priority { get; set; }
        public bool ShouldSerializePriority() { return Priority.HasValue; }
    }

    public class SiteMapResult : ActionResult
    {
        public SiteMapResult(object objectToSerialize)
        {
            ObjectToSerialize = objectToSerialize;
        }

        public object ObjectToSerialize { get; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (this.ObjectToSerialize == null) return;

            context.HttpContext.Response.Clear();
            var xs = new System.Xml.Serialization.XmlSerializer(this.ObjectToSerialize.GetType());
            context.HttpContext.Response.ContentType = "text/xml";
            xs.Serialize(context.HttpContext.Response.Output, this.ObjectToSerialize);
        }
    }
}