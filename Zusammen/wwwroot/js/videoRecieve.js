// Отримання даних про модель.
let modelData = getModelData();

const video = document.getElementById("film-video");

// Підключення до відео хабу. 
var connection = new signalR.HubConnectionBuilder().withUrl("/Video/Room/video").build();

// Вимкнення елементу кнопки допоки не відбудеться підключення до відео хабу.
document.getElementById("start").disabled = true;

// Метод, який відтворюється при відправлення хабом відповіді.
connection.on("ReceiveVideo", function (videoPath) {
    video.setAttribute("src", videoPath);
    document.getElementById("start").disabled = true;
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

// Покидання кімнати при закритті вкладки.
window.addEventListener("beforeunload", function () {
    connection.invoke("LeaveRoom", roomElement.value).catch(function (err) {
        return console.error(err.toString());
    });
});

// Синхронізація паузи відео в кімнаті.
connection.on('Stop', function (roomId) {
    video.pause();
});

video.addEventListener("pause", function () {
    connection.invoke("StopVideo", modelData.id);
});

// Синхронізація відтворення відео після паузи.
connection.on('Play', function (roomId) {
    video.play();
});

video.addEventListener("play", function () {
    connection.invoke("PlayVideo", modelData.id);
});
// Синхронізація перемотки відео.
let lastRequestTime = 0;

connection.on("Seek", function (roomId, newPosition) {
    let currentTime = new Date().getTime();
    let timeDifference = currentTime - lastRequestTime;
    if (timeDifference >= 1000) {
        lastRequestTime = currentTime;
        video.currentTime = newPosition;
    }
});

video.addEventListener("seeked", function () {
    connection.invoke("SeekVideo", modelData.id, Math.floor(video.currentTime));
});