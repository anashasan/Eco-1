

const getBranchDropdownById = (id) => {

    const stationData = {}
    const headers = new Headers();
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");
    fetch(`/Company/GetBranchByCompanyId/id/${id}`, {
        credentials: "same-origin",
        headers: headers,
        method: "GET"
    })
        .then(function (response) {
            if (response.ok) {
                response.json().then(data => {
                    var element = document.getElementById("BranchId");
                    for (var i = 0; i < data.length; i++) {
                        var opt = data[i];
                        var el = document.createElement("data");
                        el.textContent = opt;
                        el.value = opt;
                        select.appendChild(el);
                    }​
                }).catch(ex => {
                });
            };
        })
        .catch(ex => {
        });
};


