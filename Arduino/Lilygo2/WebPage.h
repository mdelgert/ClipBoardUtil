const char webpage[] PROGMEM = R"rawliteral(
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>ESP32 Configuration</title>
    <style>
        body {
            background-color: #121212;
            color: #ffffff;
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 20px 0;
            display: flex;
            justify-content: center;
            align-items: flex-start;
            height: 100vh;
            box-sizing: border-box;
        }
        .container {
            max-width: 500px;
            padding: 20px;
            background-color: #1e1e1e;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
            box-sizing: border-box;
        }
        input, select, button, textarea {
            width: 100%;
            padding: 10px;
            margin: 10px 0;
            border-radius: 4px;
            border: 1px solid #333;
            background-color: #2c2c2c;
            color: #ffffff;
            box-sizing: border-box;
        }
        textarea {
            resize: vertical;
            min-height: 100px;
        }
        button {
            background-color: #6200ea;
            border: none;
            cursor: pointer;
        }
        button:hover {
            background-color: #3700b3;
        }
    </style>
</head>
<body>
    <div class="container">
        <h1>ESP32 Configuration</h1>
        <form id="configForm">
            <h2>WiFi Settings</h2>
            <label for="ssid">SSID:</label>
            <input type="text" id="ssid" name="ssid">

            <label for="wifiPassword">WiFi Password:</label>
            <input type="password" id="wifiPassword" name="wifiPassword">

            <h2>MQTT Settings</h2>
            <label for="broker">Broker:</label>
            <input type="text" id="broker" name="broker">

            <label for="port">Port:</label>
            <input type="number" id="port" name="port">

            <label for="topic">Topic:</label>
            <input type="text" id="topic" name="topic">

            <label for="mqttUser">MQTT User:</label>
            <input type="text" id="mqttUser" name="mqttUser">

            <label for="mqttPassword">MQTT Password:</label>
            <input type="password" id="mqttPassword" name="mqttPassword">

            <label for="certificate">Certificate:</label>
            <textarea id="certificate" name="certificate"></textarea>

            <h2>Device Settings</h2>
            <label for="deviceName">Device Name:</label>
            <input type="text" id="deviceName" name="deviceName">

            <label for="jiggler">Jiggler:</label>
            <select id="jiggler" name="jiggler">
                <option value="true">True</option>
                <option value="false">False</option>
            </select>

            <label for="setupMode">Setup Mode:</label>
            <select id="setupMode" name="setupMode">
                <option value="true">True</option>
                <option value="false">False</option>
            </select>

            <label for="keyboardEnable">Keyboard Enable:</label>
            <select id="keyboardEnable" name="keyboardEnable">
                <option value="true">True</option>
                <option value="false">False</option>
            </select>

            <button type="submit">Submit</button>
        </form>
    </div>

    <script>
        document.getElementById('configForm').addEventListener('submit', function(e) {
            e.preventDefault();

            const data = {
                wifi: {
                    ssid: document.getElementById('ssid').value,
                    password: document.getElementById('wifiPassword').value
                },
                mqtt: {
                    broker: document.getElementById('broker').value,
                    port: document.getElementById('port').value ? parseInt(document.getElementById('port').value) : null,
                    topic: document.getElementById('topic').value,
                    user: document.getElementById('mqttUser').value,
                    password: document.getElementById('mqttPassword').value,
                    certificate: document.getElementById('certificate').value
                },
                device: {
                    name: document.getElementById('deviceName').value,
                    jiggler: document.getElementById('jiggler').value === 'true',
                    setup_mode: document.getElementById('setupMode').value === 'true',
                    keyboard_enable: document.getElementById('keyboardEnable').value === 'true'
                }
            };

            fetch('/reset', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            })
            .then(response => response.text())
            .then(data => {
                alert('Success: ' + data);
            })
            .catch((error) => {
                console.error('Error:', error);
                alert('Error sending data to ESP32');
            });
        });
    </script>
</body>
</html>
)rawliteral";
