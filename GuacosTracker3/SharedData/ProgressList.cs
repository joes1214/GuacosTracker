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

    }
}

    //public enum StatusList
    //{
    //    //In-Progress | Awaiting Repair | Completed | Awaiting Customer
    //    [Display(Name = "Awaiting Repair")]
    //    Awaiting,
    //    [Display(Name = "In-Progress")]
    //    InProgress,
    //    [Display(Name = "Awaiting Customer")]
    //    AwaitingCustomer,
    //    [Display(Name = "Completed")]
    //    Completed,
    //    [Display(Name = "Unrepairable")]
    //    Unrepairable
    //}
    //[Keyless]
    //[NotMapped]
    //public static class ProgressList
    //{
    //    public static string StatusString(int Value) //This is super ugly and I hate it
    //    {
    //        switch (Value)
    //        {
    //            case 0:
    //                return "Awaiting Repair";

    //            case 1:
    //                return "In-Progress";


    //            case 2:
    //                return "Awaiting Customer";


    //            case 3:
    //                return "Completed";


    //            case 4:
    //                return "Unrepairable";


    //            default:
    //                return "ERROR";
    //        }


    //    }
    //public List<string> StatusListString = new List<string>()
    //{
    //    "Awaiting Repair",
    //    "In-Progress",
    //    "Awaiting Customer",
    //    "Completed",
    //    "Unrepairable"
    //};

