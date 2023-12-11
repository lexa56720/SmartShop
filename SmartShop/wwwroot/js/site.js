// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
async function Login(login, pass) {
    const response = await fetch(`db/Load?table=${list}`,
        {
            method: "POST",
            headers:
            {
                "Content-type": "text/json; charset=UTF-8"
            }
        });
    var element = document.getElementById("content");
    var text = await response.text();
    element.innerHTML = text;
}

async function Register(name, login, pass) {
    const url = 'db/Register?' +
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

    if (!response.ok)
        return;

    const json = await response.json();
}