using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuacosTracker3.SharedData
{
    public class ProgressList
    {
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

        public static string BgColor(string Status)
        {
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

