using System;
using AutoMapper;
using Iris.DomainClasses;
using Utilities;

namespace Iris.Web.AutoMapperConvertor
{


    public class ProductStatusToStringConvertor : ITypeConverter<ProductStatus, string>
    {
        public string Convert(ResolutionContext context)
        {
            var objEnumValue = ((Enum)context.SourceValue).GetEnumDescription();
            return string.IsNullOrWhiteSpace(objEnumValue) ? string.Empty : objEnumValue;
        }
    }



    //public class DateTimeToPersianDateTimeConverter : ITypeConverter<DateTime, string>
    //{
    //    private readonly string _separator;
    //    private readonly bool _includeHourMinute;
    //    public DateTimeToPersianDateTimeConverter(string separator = "/", bool includeHourMinute = true)
    //    {
    //        _separator = separator;
    //        _includeHourMinute = includeHourMinute;
    //    }
    //    public string Convert(ResolutionContext context)
    //    {
    //        var objDateTime = context.SourceValue;
    //        return objDateTime == null ? string.Empty : toShamsiDateTime((DateTime)context.SourceValue);
    //    }
    //    private string toShamsiDateTime(DateTime info)
    //    {
    //        var year = info.Year;
    //        var month = info.Month;
    //        var day = info.Day;
    //        var persianCalendar = new PersianCalendar();
    //        var pYear = persianCalendar.GetYear(new DateTime(year, month, day, new GregorianCalendar()));
    //        var pMonth = persianCalendar.GetMonth(new DateTime(year, month, day, new GregorianCalendar()));
    //        var pDay = persianCalendar.GetDayOfMonth(new DateTime(year, month, day, new
    //        GregorianCalendar()));
    //        return _includeHourMinute ?
    //        string.Format("{0}{1}{2}{1}{3} {4}:{5}", pYear, _separator, pMonth.ToString("00",
    //        CultureInfo.InvariantCulture), pDay.ToString("00", CultureInfo.InvariantCulture),
    //        info.Hour.ToString("00"), info.Minute.ToString("00"))
    //        : string.Format("{0}{1}{2}{1}{3}", pYear, _separator, pMonth.ToString("00",
    //        CultureInfo.InvariantCulture), pDay.ToString("00", CultureInfo.InvariantCulture));
    //    }
    //}
}