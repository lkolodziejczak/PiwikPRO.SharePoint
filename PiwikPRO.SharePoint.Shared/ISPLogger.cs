using System;
using System.Collections.Generic;
using System.Text;

namespace PiwikPRO.SharePoint.Shared
{
    public interface ISPLogger
    {
        void WriteLog(Category categoryName, string source, string errorMessage);
    }
    public enum Category
    {
        Unexpected,
        High,
        Medium,
        Information
    }
}
