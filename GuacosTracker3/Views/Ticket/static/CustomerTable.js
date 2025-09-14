class CustomerTable {
    constructor() {
        this.element = this.createElements();
    }

    appendTo(element) {
        element.append(this.element.table);
    }

    refresh(data) {
        while (this.element.header.lastChild) this.element.header.lastChild.remove();
        while (this.element.body.lastChild) this.element.body.lastChild.remove();

        this.createHeaderRows(data);
        this.createBodyRows(data);
    }

    createElements() {
        const table = document.createElement('table');
        table.classList.add('table', 'table-striped');

        const header = document.createElement('thead');
        table.append(header);

        const body = document.createElement('tbody');
        table.append(body);

        return { "table": table, "header": header, "body": body };
    }

    createHeaderRows(data) {
        const row = document.createElement('tr');

        if (data.length < 1) {
            return
        }
        else {
            Object.keys(data[0]).forEach((key) => {
                if (key.toLowerCase() !== 'id') {
                    row.append(this.createHeaderColumn(key));
                }
            });
        }

        row.append(this.createHeaderColumn());

        this.element.header.append(row);
    }

    createHeaderColumn(value = '') {
        const th = document.createElement('th');
        th.innerText = value;
        return th;
    }

    createBodyRows(data) {
        data.forEach((obj) => {
            const row = document.createElement('tr');

            Object.keys(obj).forEach((key) => {
                if (key.toLowerCase() !== 'id') {
                    row.append(this.createRowColumn(obj[key]));
                }
            });

            const td = document.createElement('td');
            td.append(this.createAnchor(obj.id));
            row.append(td);

            this.element.body.append(row);
        });
    }

    createRowColumn(value = '') {
        const td = document.createElement('td');
        td.innerText = value;
        return td;
    }

    createAnchor(id) {
        const anchor = document.createElement('a');
        anchor.innerText = 'Select';
        anchor.className = 'btn btn-secondary btn-sm';
        anchor.href = `?customerID=${id}`

        return anchor;
    }
}