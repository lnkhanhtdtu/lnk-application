function registerDataTable(elementName, columns, urlApi) {
    $(elementName).DataTable({
        scrollY: 300,
        scrollX: true,
        processing: true,
        serverSide: true,
        columns: columns,
        ajax: {
            url: urlApi,
            type: 'POST',
            dataType: 'json'
        }
    });
}