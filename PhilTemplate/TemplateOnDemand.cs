
using System;
using System.Text;

namespace PhilTemplate
{
    public static class TemplateOnDemand
    {
        public static string ApplyStringTemplate(string template, Func<string, string> getValue, string keyStartMarker = "{{", string keyEndMarker = "}}")
        {
            var markerFoundAtIndex = template.IndexOf(keyStartMarker);

            if (markerFoundAtIndex == -1) { return template; }

            var returnValue = new StringBuilder();

            var keyStartMarkerLength = keyStartMarker.Length;
            var keyEndMarkerLength = keyEndMarker.Length;

            returnValue.Append(template[0..markerFoundAtIndex]);

            while (markerFoundAtIndex > -1)
            {
                var markerBegin = markerFoundAtIndex + keyStartMarkerLength;
                var markerEnd = template.IndexOf(keyEndMarker, markerBegin);
                var contentBegin = markerEnd + keyEndMarkerLength;

                var key = template[markerBegin..markerEnd];

                // if key is blank or equals key start marker, insert the keyStartMarker instead
                if (key == keyStartMarker || key == "")
                {
                    returnValue.Append(keyStartMarker);
                }
                else
                {
                    returnValue.Append(getValue(key));
                }


                markerFoundAtIndex = template.IndexOf(keyStartMarker, markerEnd);

                if (markerFoundAtIndex == -1)
                {
                    returnValue.Append(template[contentBegin..]);
                }
                else
                {
                    returnValue.Append(template[contentBegin..markerFoundAtIndex]);
                }
            }

            return returnValue.ToString();
        }
    }
}