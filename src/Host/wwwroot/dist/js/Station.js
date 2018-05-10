const addModel = () => {
    $(document).ready(function () {

        $("#myModal").modal();
    });
};

const editModel = (id) => {

    getStationById(id);
    $(document).ready(function () {
        $("#editModal").modal();
    });

};

const changeIt = () => {
    var i = 1;
    my_div.innerHTML = my_div.innerHTML + "</br> <input type='text' name='mytext'+ i > "

};


const GetDynamicTextBox = (value) => {
    return '<input name="DynamicTextBox" class="form-control col-sm-5" type="text" placeholder="Activity Name" value="' + value + '" /> ' +
        '<a onclick="RemoveTextBox(this)"><span class="glyphicon glyphicon-remove" ></span></a >'
};

const AddTextBox = () => {
    var div = document.createElement('DIV');
    div.className = "form-group";
    div.innerHTML = GetDynamicTextBox("");
    document.getElementById("TextBoxContainer").appendChild(div);
};

function RemoveTextBox(div) {
    document.getElementById("TextBoxContainer").removeChild(div.parentNode);
}

const RecreateDynamicTextboxes = () => {
    var values = eval('<%=Values%>');
    if (values != null) {
        var html = "";
        for (var i = 0; i < values.length; i++) {
            html += "<div>" + GetDynamicTextBox(values[i]) + "</div>";
        }
        document.getElementById("TextBoxContainer").innerHTML = html;
    }
};

const ActivityTextBox = (activity) => {
    var s = "";
    AddTextBox();
    for (var i = 0; i < activity.length; i++) {
        s += "<div>" + GetDynamicTextBox(activity[i].name) + "</div>"; //Create one textbox as HTML
        document.getElementById("TextBoxContainer").innerHTML = s;
    }

};
//window.onload = RecreateDynamicTextboxes;

const postStation = async () => {

    var stationName = document.getElementById("stationName").value;
    var stationDescription = document.getElementById("stationDescription").value;

    const stationModel = {
        Name: stationName,
        Description: stationDescription,
        StationId: 0
    };

    const headers = new Headers();
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");

    const profileUrl = "/Company/AddStation";

    const res = await fetch(profileUrl, {
        credentials: "same-origin",
        headers: headers,
        method: "POST",
        body: JSON.stringify(stationModel)
    });
    if (res.ok) {
        window.location.reload();
        return;
    }
};


const postActivity = async () => {


    var stationId = document.getElementById("editStationId").value;
    const model = {
        stationId: stationId,
        activities: []
    };
    var dataArray = $("#formActivity")
        .serializeArray();
    for (var i in dataArray) {
        model.activities.push({
            name: dataArray[i].value
        });
    };

    const headers = new Headers();
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");

    const activityUrl = "/Activity/AddActivity";


    const res = await fetch(activityUrl, {
        credentials: "same-origin",
        headers: headers,
        method: "POST",
        body: JSON.stringify(model)
    });
    if (res.ok) {
        window.location.reload();
        return;
    }
    var jsonModel = JSON.stringify(model);
    console.log(jsonModel);

};



const getStationById = (id) => {

    const stationData = {}
    const headers = new Headers();
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");
    fetch(`/Company/GetActivityByStationId/id/${id}`, {
        credentials: "same-origin",
        headers: headers,
        method: "GET"
    })
        .then(function (response) {
            if (response.ok) {
                response.json().then(data => {
                    document.getElementById("editUserName").value = data.stationName;
                    document.getElementById("editStationId").value = data.stationId;
                    if (data.activities.length > 0) {
                        ActivityTextBox(data.activities);

                    }
                    else {
                        var elem = document.getElementById("TextBoxContainer");
                        elem.removeAttribute();
                    }
                }).catch(ex => {
                });
            };
        })
        .catch(ex => {
        });
};