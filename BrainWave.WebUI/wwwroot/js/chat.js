"use strict";
var user = document.getElementById("userInput").value;
var conversation = document.getElementById("conversationInput").value;

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;
connection.on("ReceiveMessage", function (user, message, name, surname, photo, date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear(),
        hour = '' + d.getHours(),
        minute = '' + d.getMinutes(),
        second = '' + d.getSeconds();
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.innerHTML = `<div class="author-article d-flex justify-content-start align-items-center">
                            <img class="d-block" src="/media/${photo}" alt="avatar">
                            <div>
                                <a href="/users/${user}">
                                    ${name} ${surname}
                                </a>
                            </div>
                        </div>

                        <div class="d-flex justify-content-between align-items-center">
                            <p>${message}</p>
                            <p>${day}.${month}.${year} ${hour}:${minute}:${second}</p>
                        </div>`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    connection.invoke("JoinChat", conversation).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", conversation, user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});