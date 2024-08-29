#include <WiFi.h>
#include <WebServer.h>
#include <Preferences.h>
#include <ArduinoJson.h>
#include "WebPage.h"
#include "Config.h"

const char *ssid = "ESP32_Access_Point";
const char *password = "12345678";

WebServer server(80);
Preferences preferences;

void handleRoot() {
    server.send(200, "text/html", webpage);
}

void handleReset() {
    if (server.hasArg("plain") == false) {
        server.send(400, "text/plain", "Body not received");
        return;
    }

    String body = server.arg("plain");
    Serial.println("Received JSON:");
    Serial.println(body);

    StaticJsonDocument<2048> doc;
    DeserializationError error = deserializeJson(doc, body);

    if (error) {
        Serial.print(F("deserializeJson() failed: "));
        Serial.println(error.f_str());
        server.send(400, "text/plain", "Invalid JSON");
        return;
    }

    preferences.begin("settings", false);

    if (doc["wifi"]["ssid"].isNull() == false) {
        preferences.putString("wifi_ssid", doc["wifi"]["ssid"].as<String>());
    }
    if (doc["wifi"]["password"].isNull() == false) {
        preferences.putString("wifi_password", doc["wifi"]["password"].as<String>());
    }

    if (doc["mqtt"]["broker"].isNull() == false) {
        preferences.putString("mqtt_broker", doc["mqtt"]["broker"].as<String>());
    }
    if (doc["mqtt"]["port"].isNull() == false) {
        preferences.putInt("mqtt_port", doc["mqtt"]["port"]);
    }
    if (doc["mqtt"]["topic"].isNull() == false) {
        preferences.putString("mqtt_topic", doc["mqtt"]["topic"].as<String>());
    }
    if (doc["mqtt"]["user"].isNull() == false) {
        preferences.putString("mqtt_user", doc["mqtt"]["user"].as<String>());
    }
    if (doc["mqtt"]["password"].isNull() == false) {
        preferences.putString("mqtt_password", doc["mqtt"]["password"].as<String>());
    }
    if (doc["mqtt"]["certificate"].isNull() == false) {
        preferences.putString("mqtt_certificate", doc["mqtt"]["certificate"].as<String>());
    }

    if (doc["device"]["name"].isNull() == false) {
        preferences.putString("device_name", doc["device"]["name"].as<String>());
    }
    preferences.putBool("device_jiggler", doc["device"]["jiggler"]);
    preferences.putBool("device_setup_mode", doc["device"]["setup_mode"]);
    preferences.putBool("device_keyboard_enable", doc["device"]["keyboard_enable"]);

    if (doc["secrets"]["user"].isNull() == false) {
        preferences.putString("user_secret_1", doc["secrets"]["user"]["secret_1"].as<String>());
        preferences.putString("user_secret_2", doc["secrets"]["user"]["secret_2"].as<String>());
        preferences.putString("user_secret_3", doc["secrets"]["user"]["secret_3"].as<String>());
        preferences.putString("user_secret_4", doc["secrets"]["user"]["secret_4"].as<String>());
        preferences.putString("user_secret_5", doc["secrets"]["user"]["secret_5"].as<String>());
        preferences.putString("user_secret_6", doc["secrets"]["user"]["secret_6"].as<String>());
        preferences.putString("user_secret_7", doc["secrets"]["user"]["secret_7"].as<String>());
        preferences.putString("user_secret_8", doc["secrets"]["user"]["secret_8"].as<String>());
        preferences.putString("user_secret_9", doc["secrets"]["user"]["secret_9"].as<String>());
        preferences.putString("user_secret_10", doc["secrets"]["user"]["secret_10"].as<String>());
    }

    if (doc["secrets"]["device"].isNull() == false) {
        preferences.putString("device_secret_1", doc["secrets"]["device"]["secret_1"].as<String>());
        preferences.putString("device_secret_2", doc["secrets"]["device"]["secret_2"].as<String>());
        preferences.putString("device_secret_3", doc["secrets"]["device"]["secret_3"].as<String>());
        preferences.putString("device_secret_4", doc["secrets"]["device"]["secret_4"].as<String>());
        preferences.putString("device_secret_5", doc["secrets"]["device"]["secret_5"].as<String>());
        preferences.putString("device_secret_6", doc["secrets"]["device"]["secret_6"].as<String>());
        preferences.putString("device_secret_7", doc["secrets"]["device"]["secret_7"].as<String>());
        preferences.putString("device_secret_8", doc["secrets"]["device"]["secret_8"].as<String>());
        preferences.putString("device_secret_9", doc["secrets"]["device"]["secret_9"].as<String>());
        preferences.putString("device_secret_10", doc["secrets"]["device"]["secret_10"].as<String>());
    }

    preferences.end();

    server.send(200, "text/plain", "Settings and secrets saved and processed");
}

void handleGetPreferences() {
    preferences.begin("settings", true);  // Open Preferences in read-only mode

    StaticJsonDocument<4096> doc;

    doc["wifi"]["ssid"] = preferences.getString("wifi_ssid", "defaultSSID");
    doc["wifi"]["password"] = preferences.getString("wifi_password", "defaultPassword");

    doc["mqtt"]["broker"] = preferences.getString("mqtt_broker", "defaultBroker");
    doc["mqtt"]["port"] = preferences.getInt("mqtt_port", 1883);
    doc["mqtt"]["topic"] = preferences.getString("mqtt_topic", "defaultTopic");
    doc["mqtt"]["user"] = preferences.getString("mqtt_user", "defaultUser");
    doc["mqtt"]["password"] = preferences.getString("mqtt_password", "defaultMQTTPass");
    doc["mqtt"]["certificate"] = preferences.getString("mqtt_certificate", "defaultCertificate");

    doc["device"]["name"] = preferences.getString("device_name", "defaultDevice");
    doc["device"]["jiggler"] = preferences.getBool("device_jiggler", false);
    doc["device"]["setup_mode"] = preferences.getBool("device_setup_mode", false);
    doc["device"]["keyboard_enable"] = preferences.getBool("device_keyboard_enable", false);

    for (int i = 1; i <= 10; i++) {
        String key = "user_secret_" + String(i);
        doc["secrets"]["user"][key] = preferences.getString(key.c_str(), "");
    }

    for (int i = 1; i <= 10; i++) {
        String key = "device_secret_" + String(i);
        doc["secrets"]["device"][key] = preferences.getString(key.c_str(), "");
    }

    String response;
    serializeJson(doc, response);
    server.send(200, "application/json", response);

    preferences.end();
}

void setup() {
    Serial.begin(115200);

    WiFi.softAP(ssid, password);

    IPAddress IP = WiFi.softAPIP();
    Serial.print("AP IP address: ");
    Serial.println(IP);

    server.on("/", HTTP_GET, handleRoot);
    server.on("/reset", HTTP_POST, handleReset);
    server.on("/preferences", HTTP_GET, handleGetPreferences);  // Register the new route

    server.begin();
    Serial.println("HTTP server started");
}

void loop() {
    server.handleClient();
}
