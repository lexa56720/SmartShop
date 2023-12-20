// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function GetApiUrl(method) {
    return document.location.origin + '/api/' + method + '?';
}
async function Login(login, pass) {
    document.getElementById('warning').style.visibility = 'hidden';

    const url = GetApiUrl("Login") +
        new URLSearchParams({
            login: login,
            pass: pass
        });

    const response = await fetch(url,
        {
            method: 'POST',
            headers:
            {
                'Content-Type': 'application/json'
            }
        });

    if (response.status == 409)
        document.getElementById('warning').style.visibility = 'visible';
    if (!response.ok)
        return;

    window.open(document.location.origin, "_self");
}
async function Register(name, login, pass) {
    document.getElementById('warning').style.visibility = 'hidden';

    const url = GetApiUrl("Register") +
        new URLSearchParams({
            name: name,
            login: login,
            pass: pass
        });

    const response = await fetch(url,
        {
            method: 'POST',
            headers:
            {
                'Content-Type': 'application/json'
            }
        });

    if (response.status == 409)
        document.getElementById('warning').style.visibility = 'visible';

    if (!response.ok)
        return;

    window.open(document.location.origin, "_self");
}
function AddToCart(id) {
    let element = document.getElementById("AddToCartButton " + id);
    element.disabled = true;
    element.innerHTML = "Added to cart";

    InsertToCart(id);
}
function InsertToCart(id) {
    let cookie = GetCookie('Cart');
    if (cookie === null)
        cookie = '';
    SetCookie('Cart', cookie + '|' + id, 3)
}
function RemoveFromCart(index) {
    let cookie = GetCookie('Cart');
    if (cookie === null)
        return;
    let products = cookie.split('|').filter(function (part) { return !!part; });
    products.splice(index, 1);
    SetCookie('Cart', products.join('|'), 3);
}
async function TrySendForm(formId, method, succsessElementId, failedElementId) {
    document.getElementById(failedElementId).style.visibility = 'hidden';
    document.getElementById(succsessElementId).style.visibility = 'hidden';

    if (await SendForm(formId, method))
        document.getElementById(succsessElementId).style.visibility = 'visible';
    else
        document.getElementById(failedElementId).style.visibility = 'visible';

}
async function SendForm(formId, method) {
    document.getElementById(formId);

    const inputTags = Array.from(document.getElementById(formId).querySelectorAll('input'));
    const textAreas = document.getElementById(formId).querySelectorAll('textarea')
    for (const textArea of textAreas)
        inputTags.push(textArea);
    const formData = new FormData();

    for (const inputTag of inputTags) {
        if (!inputTag.validity.valid)
            return false;
        if (inputTag.type == 'file') {
            for (const innerFile of inputTag.files)
                formData.append(innerFile.name, innerFile);
        }
        else {
            formData.append(inputTag.name, inputTag.value);
        }

    }

    const url = document.location.origin + '/api/' + method;

    const response = await fetch(url,
        {
            method: 'POST',
            body: formData
        });


    if (!response.ok)
        return false;
    return true;
}
function LogOut() {
    DeleteCookie("id");
    DeleteCookie("token");
    DeleteCookie("name");
    DeleteCookie("Cart");
    location.reload();
}
function SetCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}
function GetCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];

        while (c.charAt(0) == ' ')
            c = c.substring(1, c.length);

        if (c.indexOf(nameEQ) == 0)
            return c.substring(nameEQ.length, c.length);
    }
    return null;
}
function DeleteCookie(name) {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}
function ButtonRedirect(destination) {
    var url = document.location.origin;
    window.location = url + "/" + destination;
}

async function DeleteProduct(id)
{
    const url = GetApiUrl("DeleteProduct") +
                new URLSearchParams({id: id});

    const response = await fetch(url,
        {
            method: 'POST',
        });


    if (!response.ok)
       return window.alert("Delete error");;

    window.open(document.location.origin, "_self");
}
