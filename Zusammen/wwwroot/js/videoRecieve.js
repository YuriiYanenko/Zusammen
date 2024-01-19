// Отримання даних про модель.
let modelData = getModelData();

// Підключення до відео хабу. 
var connection = new signalR.HubConnectionBuilder().withUrl("/Video/Room/video").build();

// Вимкнення елементу кнопки допоки не відбудеться підключення до відео хабу.
document.getElementById("start").disabled = true;

// Метод, який відтворюється при відправлення хабом відповіді.
connection.on("ReceiveVideo", function (videoPath) {
    let film = document.getElementById("film-video");
    film.setAttribute("src", videoPath);
});

// Початок підключення до хабу. Виклик методу підключення до кімнати (групи перегляду).
connection.start().then(function () {
    document.getElementById("start").disabled = false;
    connection.invoke("JoinRoom", modelData.id).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

// Звернення до методу відтворення відео при натисканні кнопки відтворення.
document.getElementById("start").addEventListener("click", function (event) {
    connection.invoke("SendVideo", modelData.id, modelData.videoPath).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

// Add an event listener to leave the room when the page is closed or navigated away
window.addEventListener("beforeunload", function () {
    connection.invoke("LeaveRoom", roomElement.value).catch(function (err) {
        return console.error(err.toString());
    });
});