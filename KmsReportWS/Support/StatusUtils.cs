using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Support
{
    public static class StatusUtils
    {
        public static ReportStatus ParseStatus(string status) =>
            Enum.TryParse(status, out ReportStatus enumStatus) ? enumStatus : ReportStatus.Saved;

        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var member = type.GetEnumName(val);
                        if (member == null) continue;
                        
                        var memInfo = type.GetMember(member);

                        if (memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return null;
        }
    }
}