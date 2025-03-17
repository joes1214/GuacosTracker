async function main(url, lname = '', fname = '') {
    const requestURL = createURL(url, lname, fname);
    const data = await getCustomers(requestURL).then();
    const table = new CustomerTable();
    disableAndCloseDetails(isCustomerSelected());

    const resultsDiv = document.getElementById('customer-results');
    table.appendTo(resultsDiv, data);
}

function createURL(url, lname, fname) {
    return `${url}/api/customer/get?lname=${lname}&${fname}`;
}

function getCustomers(url) {
    return fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .catch(error => {
            throw new Error(error)
        });
}

function isCustomerSelected() {
    const customerID = document.getElementById('CustomerID').value;
    const customerFName = document.getElementById('CustomerFName').value;
    const customerLName = document.getElementById('CustomerLName').value;

    return customerID !== ""
        && customerFName !== ""
        && customerLName !== "";
}

function disableAndCloseDetails(isCustomerSelected) {
    const details = document.getElementById('customer-details');
    details.open = !isCustomerSelected; 
}

class CustomerTable {
    constructor() {
        this.element = document.createElement('table');
        this.element.className = 'table table-striped';
    }

    appendTo(element, data) {
        this.createHead(data);
        this.createBody(data);
        element.append(this.element);
    }

    createHead(data) {
        const header = document.createElement('thead');
        const row = document.createElement('tr');

        if (data.length < 1) {
            return
        }
        else {
            Object.keys(data[0]).forEach((key) => {
                const th = document.createElement('th');
                th.innerText = key;
                row.append(th);
            });
        }

        const th = document.createElement('th');
        row.append(th);

        header.append(row);
        this.element.append(header);
    }

    createBody(data) {
        const body = document.createElement('tbody');

        data.forEach((obj) => {
            const row = document.createElement('tr');

            Object.keys(obj).forEach((key) => {
                const td = document.createElement('td');
                td.innerText = obj[key];
                row.append(td);
            });

            const td = document.createElement('td');
            td.append(this.createAnchor(obj.id));
            row.append(td);

            body.append(row);
        });

        this.element.append(body);
    }

    createAnchor(id) {
        const anchor = document.createElement('a');
        anchor.innerText = 'Select';
        anchor.className = 'btn btn-secondary btn-sm';
        anchor.href = `?customerID=${id}`

        return anchor;
    }
}