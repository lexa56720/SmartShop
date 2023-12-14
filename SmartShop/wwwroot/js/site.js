// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function GetApiUrl(method)
{
    return document.location.origin + '/api/' + method + '?';
}
async function Login(login, pass)
{
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
                'Content-Type': 'application/json' // Change content type if passing JSON
            }
        });

    if (response.status == 409)
        document.getElementById('warning').style.visibility = 'visible';
    if (!response.ok)
        return;

    window.open(document.location.origin, "_self");
}

async function Register(name, login, pass)
{
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
                'Content-Type': 'application/json' // Change content type if passing JSON
            }
        });

    if (response.status == 409)
        document.getElementById('warning').style.visibility = 'visible';

    if (!response.ok)
        return;

    window.open(document.location.origin, "_self");
}


function LogOut()
{
    DeleteCookie("id");
    DeleteCookie("token");
    DeleteCookie("name");

    location.reload();
}

function DeleteCookie(name)
{
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}
