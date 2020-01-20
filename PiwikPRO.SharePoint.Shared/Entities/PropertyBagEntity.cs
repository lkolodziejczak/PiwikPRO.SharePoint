using System;
using System.Collections.Generic;
using System.Text;

namespace PiwikPRO.SharePoint.Shared
{
    public class PropertyBagEntity
    {
        public enum PropertyLevelEnum { Farm, PiwikAdmin, SiteCollection }
        public string PropertyName { get; set; }
        public PropertyLevelEnum PropertyLevel { get; set; }
        public string PropertyValue { get; set; }
        public string PropertyTitle { get; set; }

        public PropertyBagEntity(string _PropertyTitle, string _PropertyName, string _PropertyValue, PropertyLevelEnum _PropertyLevel)
        {
            PropertyTitle = _PropertyTitle;
            PropertyName = _PropertyName;
            PropertyValue = _PropertyValue;
            PropertyLevel = _PropertyLevel;
        }
    }
}
