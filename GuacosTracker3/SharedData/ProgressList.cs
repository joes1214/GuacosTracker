using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuacosTracker3.SharedData
{
    public class ProgressList
    {
        public static readonly Dictionary<int, string> StatusStrings = new()
        {
            {1, "Awaiting Repair" },
            {2, "In-Progress" },
            {3, "Awaiting Customer" },
            {4, "Completed" },
            {5, "Unrepairable" }
        };

        public static readonly List<SelectListItem> StatusStringSelect = StatusStrings.Select(x => new SelectListItem
        {
            Value = x.Key.ToString(),
            Text = x.Value
        }).ToList();

        enum StatusList
        {
            AwaitingRepair = 1,
            InProgress = 2,
            AwaitingCustomer = 3,
            Completed = 4,
            Unrepairable = 5,
        }

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

        public static readonly List<string> GetStatusOrder = new()
        {
                "Awaiting Repair",
                "Awaiting Customer",
                "In-Progress",
                "Completed",
                "Unrepairable"
        };

        public static string GetStatusString(int x)
        {
            if (!StatusStrings.ContainsKey(x))
            {
                return "ERROR";
            }

            return StatusStrings[x];
        }


        public static string BgColor(string Status)
        {
            if (Status == null)
            {
                return "ERROR";
            }

            if (Status.Equals("Awaiting Repair") || Status.Equals("Awaiting Customer"))
            {
                return "table-warning";
            }

            if (Status.Equals("In-Progress"))
            {
                return "table-active";
            }

            if (Status.Equals("Completed"))
            {
                return "table-success";
            }

            if (Status.Equals("Unrepairable"))
            {
                return "table-danger";
            }

            return "table-active";
        }

    }
}

