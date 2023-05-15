/*
    This class is used to maintain possible status options. 
    I'm sure there's a better way to go about this, but this is what I chose to go with.
*/

using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuacosTracker3.SharedData
{
    public class ProgressList
    {
        //Dictionary for Bootstrap class when displaying in table
        private static readonly Dictionary<string, string> StatusStrings = new()
        {
            {"Awaiting Repair", "table-warning" },
            {"In-Progress", "table-active" },
            {"Awaiting Customer", "table-warning" },
            {"Completed", "table-success" },
            {"Unrepairable", "table-danger" }
        };

        public static List<SelectListItem> GetStatusList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Awaiting Repair", Value = "Awaiting Repair" },
                new SelectListItem { Text = "In-Progress", Value = "In-Progress" },
                new SelectListItem { Text = "Awaiting Customer", Value = "Awaiting Customer" },
                new SelectListItem { Text = "Completed", Value = "Completed" },
                new SelectListItem { Text = "Unrepairable", Value = "Unrepairable" }
            };
        }

        public static List<SelectListItem> GetStatusListOpen()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem { Text = "Awaiting Repair", Value = "Awaiting Repair" },
                new SelectListItem { Text = "In-Progress", Value = "In-Progress" },
                new SelectListItem { Text = "Awaiting Customer", Value = "Awaiting Customer" },
            };
        }

        public static readonly List<string> GetStatusOrder = new()
        {
                "Awaiting Repair",
                "Awaiting Customer",
                "In-Progress",
                "Completed",
                "Unrepairable"
        };

        public static string BgColor(string Status)
        {
            if (!StatusStrings.ContainsKey(Status))
            {
                return "ERROR";
            }

            return StatusStrings[Status];
        }
    }
}

