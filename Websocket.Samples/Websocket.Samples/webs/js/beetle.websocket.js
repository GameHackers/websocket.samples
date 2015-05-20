
function WSClient() {
    this.connected = false;
    this.websocket = null;
    this.host = null;
    this.lastError = null;
    this.timeoutSecond = 0;
    this.messageid = 0;
    this.callbackMap = new Object();
    this.activeTime = new Date();
    this.typeMethod = new Object();
}

WSClient.prototype.detectTimeout = function (second) {
    this.timeoutSecond = second;
    var client = this;
    setInterval(function () {
        if (client.connected && client.timeoutSecond > 0) {
            if ((Date.now() / 1000 - client.activeTime.getTime() / 1000) >= client.timeoutSecond) {
                client.websocket.close();
            }
        }
    }, 1000);
};

WSClient.prototype.onConnect = {};

WSClient.prototype.onClose = {};

WSClient.prototype.onError = {};

WSClient.prototype.onReceive = {};

WSClient.prototype.registrAction = function (command, fun) {
    this.typeMethod[command.toString().toLowerCase()] = fun;
};

WSClient.prototype.send = function (command, data, callback) {
    this.messageid++;
    var id = this.messageid.toString();
    var sd = { Command: command, ID: id, Data: data };
    if (callback)
        this.callbackMap[id] = callback;
    this.websocket.send(JSON.stringify(sd));
}

WSClient.prototype.connect = function (host) {
   
    var client = this;
    client.lastError = null;
    this.host = host;
    var ws = new WebSocket(this.host);
    this.websocket = ws;
    this.websocket.onopen = function (evt) {
        client.connected = true;
        client.activeTime = new Date();
        if (client.onConnect)
            client.onConnect(client);
    };
    this.websocket.onclose = function (evt) {
        client.connected = false;
        if (client.onClose)
            client.onClose();
    };
    this.websocket.onmessage = function (evt) {
        client.activeTime = new Date();
        var message = JSON.parse(evt.data);
        var id = message.ID;
        var command = null;
        if(message.Command)
            command= message.Command.toString().toLowerCase();
        var exception = message.Execption;
        var data = message.Data;
        if (client.callbackMap[id]) {
            client.callbackMap[id](data, exception);
        }
        else if (client.typeMethod[command]) {
            client.typeMethod[command](data, exception);
        }
        else {
            if (client.onReceive)
                client.onReceive(data, exception);
        }
    };
    this.websocket.onerror = function (evt) {
        lastError = evt;
        if(client.onError)
            client.onError(evt)
    };
};
