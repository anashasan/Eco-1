$('#email').change((event) => {
    var message = $("#emailMessage");
    const headers = new Headers();
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");
    fetch(`/Account/IsEmailExist/email/${event.target.value}`, {
        credentials: "same-origin",
        headers: headers,
        method: "GET"
    })
        .then(function (response) {
            if (response.ok) {
                response.json()
                    .then(bool => {
                        if (bool) {
                            message.css("color", "red");
                            message.css("fontSize", "small")
                            message.html("Email is NOT available");
                            document.getElementById('btn').setAttribute("disabled", "disabled");
                        }
                        else {
                            message.css("color", "green");
                            message.css("fontSize", "small")
                            message.html("Email is  available");
                            document.getElementById('btn').removeAttribute("disabled");

                        }
                    });
            }
        })
        .catch(ex => {
        });
});

$('#userName').change((event) => {
    var message = $("#userNameMessage");
    const headers = new Headers();
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");
    fetch(`/Account/IsUerNameExist/usesrName/${event.target.value}`, {
        credentials: "same-origin",
        headers: headers,
        method: "GET"
    })
        .then(function (response) {
            if (response.ok) {
                response.json()
                    .then(bool => {
                        if (bool) {
                            message.css("color", "red");
                            message.css("fontSize", "small")
                            message.html("UserName is NOT available");
                            document.getElementById('btn').setAttribute("disabled", "disabled");
                        }
                        else {
                            message.css("color", "green");
                            message.css("fontSize", "small")
                            message.html("UserName is  available");
                            document.getElementById('btn').removeAttribute("disabled");

                        }
                    });
            }
        })
        .catch(ex => {
        });
});


function ClearMessage() {
    $("#message").html("");
};

const checkEquality = (confirmPasswordText) => {
    const password = document.getElementById('Password').value;
    if (password === confirmPasswordText) {
        const btRegister = document.getElementById('btRegister');
        btRegister.disabled = "";
        const validationMessage = document.getElementById('msgConfirmPassword');
        validationMessage.style = 'display:none';
        return;
    } else {
        const btRegister = document.getElementById('btRegister');
        btRegister.disabled = "disabled";
        const validationMessage = document.getElementById('msgConfirmPassword');
        validationMessage.style = 'display:inline';
        return;
    }
};

const checkPasswordStrength = () => {
    let isPasswordStrenghtValid = true;
    const password = document.getElementById('Password').value;

    if (!password) {
        return false;
    }

    //const lowerCaseLetters = /[a-z]/g;
    //if (!lowerCaseLetters.test(password)) {
    //    const lowerCaseTextBox = document.getElementById('lowerCase');
    //    lowerCaseTextBox.style = 'color: red;';
    //    isPasswordStrenghtValid = false;
    //} else {
    //    const lowerCaseTextBox = document.getElementById('lowerCase');
    //    lowerCaseTextBox.style = 'color: black;';
    //}

    //// Validate capital letters
    //const upperCaseLetters = /[A-Z]/g;
    //if (!upperCaseLetters.test(password)) {
    //    const upperCaseTextBox = document.getElementById('upperCase');
    //    upperCaseTextBox.style = 'color: red;';
    //    isPasswordStrenghtValid = false;
    //} else {
    //    const upperCaseTextBox = document.getElementById('upperCase');
    //    upperCaseTextBox.style = 'color: black;';
    //}

    // Validate Non Alpha numeric
    const nonAlphaNumeric = /[^A-z\s\d][\\\^]?/g;
    if (!nonAlphaNumeric.test(password)) {
        const nonAlphaNumericCaseTextBox = document.getElementById('nonAlphaNumeric');
        nonAlphaNumericCaseTextBox.style = 'color: red;';
        isPasswordStrenghtValid = false;
    } else {
        const nonAlphaNumericCaseTextBox = document.getElementById('nonAlphaNumeric');
        nonAlphaNumericCaseTextBox.style = 'color: black;';
    }

    // Validate numer
    const number = /\d/;
    if (!number.test(password)) {
        const numericTestCase = document.getElementById('numbers');
        numericTestCase.style = 'color: red;';
        isPasswordStrenghtValid = false;
    } else {
        const numericTestCase = document.getElementById('numbers');
        numericTestCase.style = 'color: black;';
    }

    // Validate length
    if (password.length < 7) {
        const lenghtCaseTextBox = document.getElementById('sevenCharacter');
        lenghtCaseTextBox.style = 'color: red;';
        isPasswordStrenghtValid = false;
    } else {
        const lenghtCaseTextBox = document.getElementById('sevenCharacter');
        lenghtCaseTextBox.style = 'color: black;';
    }
    return isPasswordStrenghtValid;
};
