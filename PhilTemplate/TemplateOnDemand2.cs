# if DEBUG
using System;
using System.Text;

namespace PhilTemplate
{
    public static class TemplateOnDemand2
    {
        public static string ApplyStringTemplate(string templateStr, Func<string, string> getValue, string keyStartMarker = "{{", string keyEndMarker = "}}")
        {
            var template = templateStr.AsSpan();

            var markerFoundAtIndex = template.IndexOf(keyStartMarker);

            if (markerFoundAtIndex == -1) { return templateStr; }

            var returnValue = new StringBuilder();

            var keyStartMarkerLength = keyStartMarker.Length;
            var keyEndMarkerLength = keyEndMarker.Length;

            returnValue.Append(template[0..markerFoundAtIndex]);

            while (markerFoundAtIndex > -1)
            {
                var markerBegin = markerFoundAtIndex + keyStartMarkerLength;

                template = template[markerBegin..];  

                var markerEnd = template.IndexOf(keyEndMarker);
                var contentBegin = markerEnd + keyEndMarkerLength;

                var key = template[..markerEnd].ToString();

                // if key is blank or equals key start marker, insert the keyStartMarker instead
                if (key == keyStartMarker || key == "")
                {
                    returnValue.Append(keyStartMarker);
                }
                else
                {
                    returnValue.Append(getValue(key));
                }

                template = template[contentBegin..];
                markerFoundAtIndex = template.IndexOf(keyStartMarker);

                if (markerFoundAtIndex == -1)
                {
                    returnValue.Append(template);
                }
                else
                {
                    returnValue.Append(template[..markerFoundAtIndex]);
                }
            }

            return returnValue.ToString();
        }
    }
}
#endif
