namespace GuacosTracker3.Models.ViewModels
{
    public class CreateTicketViewModel
    {
        public Ticket Ticket { get; set; } = new Ticket();

        public int CustomerID { get; set; }
        public string Description { get; set; }
        public string? CustomerFName { get; set; }
        public string? CustomerLName { get; set; }

        public CreateTicketViewModel() { }

        public CreateTicketViewModel(int customerID, string description, string customerFName, string customerLName)
        {
            Ticket = new Ticket();
            Description = description;
            CustomerID = customerID;
            CustomerFName = customerFName;
            CustomerLName = customerLName;
        }

        public CreateTicketViewModel(string title, string employeeID, string description, string status, int customerID, string customerFName, string customerLName)
        {
            Ticket = new Ticket(title, employeeID, status, customerID);
            Description = description;
            CustomerID = customerID;
            CustomerFName = customerFName;
            CustomerLName = customerLName;
        }
    }
}
