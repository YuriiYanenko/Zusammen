let modelData = getModelData();

let message = document.getElementById("chat-messages");
let userName = getUserName();

let connection = new signalR.HubConnectionBuilder().withUrl("/Video/Room/chat").build();

connection.on("RecieveMessage", function(userName, message){
    $("#discussion").append("<li>" + userName + ': ' + message + "</li>");    
});

$("#send").click(function(){
    connection.invoke("SendMessage", parseInt(modelData.id), userName, message.value);
});