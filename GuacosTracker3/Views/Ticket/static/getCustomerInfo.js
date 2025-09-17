const Globals = {}

function main(url, lname = '', fname = '') {
    Globals.url = url;
    Globals.table = new CustomerTable();
    Globals.isSearchOnCooldown = false;
    Globals.firstNameSearchInput = document.getElementById('customer-lname-input');
    Globals.lastNameSearchInput = document.getElementById('customer-fname-input');

    const requestURL = createURL(Globals.url, lname, fname);
    getCustomers(requestURL);
    disableAndCloseDetails(isCustomerSelected());
    setSearchEvents();

    const resultsDiv = document.getElementById('customer-results');
    Globals.table.appendTo(resultsDiv);
}

function createURL(url, lname, fname) {
    return `${url}?lname=${lname}&fname=${fname}`;
}

function getCustomers(url) {
    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            Globals.table.refresh(data);
        })
        .catch(error => {
            throw new Error(error)
        });
}

function isCustomerSelected() {
    const customerID = document.getElementById('CustomerID').value;
    //const customerFName = document.getElementById('CustomerFName').value;
    //const customerLName = document.getElementById('CustomerLName').value;

    return customerID !== "";
        //&& customerFName !== ""
        //&& customerLName !== "";
}

function disableAndCloseDetails(isCustomerSelected) {
    const details = document.getElementById('customer-details');
    details.open = !isCustomerSelected; 
}

function setSearchEvents() {
    const customerSearchContainer = document.getElementById('customer-search-container');
    customerSearchContainer.addEventListener('keydown', (event) => {
        if (event.key === 'Enter') {
            event.preventDefault();

            searchCustomersEvent();
        }
    });

    const searchBtn = document.getElementById('customer-lookup-btn');
    searchBtn.addEventListener('click', () => {
        searchCustomersEvent();
    });
}

function searchCustomersEvent() {
    if (!Globals.isSearchOnCooldown) {
        Globals.isSearchOnCooldown = true;

        const firstName = Globals.firstNameSearchInput.value.trim();
        const lastName = Globals.lastNameSearchInput.value.trim();

        const url = createURL(Globals.url, firstName, lastName);
        getCustomers(url);

        setTimeout(() => {
            Globals.isSearchOnCooldown = false;
        }, 1000);
    } else {
        console.log('Search is rate-limited. Please wait...');
    }
}
