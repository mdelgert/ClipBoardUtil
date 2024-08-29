#include <WiFi.h>
#include <WebServer.h>
#include <Preferences.h>  // Include the Preferences library
#include <ArduinoJson.h>  // Include the ArduinoJson library
#include "WebPage.h"      // Include the webpage header file
#include "Config.h"       // Include the header file with the struct definition

const char *ssid = "ESP32_Access_Point";
const char *password = "12345678";

WebServer server(80);
Preferences preferences;  // Create a Preferences object

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

  // Parse the JSON and save settings
  StaticJsonDocument<2048> doc;  // Increase size for secrets
  DeserializationError error = deserializeJson(doc, body);

  if (error) {
    Serial.print(F("deserializeJson() failed: "));
    Serial.println(error.f_str());
    server.send(400, "text/plain", "Invalid JSON");
    return;
  }

  // Open Preferences with a namespace name
  preferences.begin("settings", false);

  // Save WiFi settings
  if (doc["wifi"]["ssid"].isNull() == false) {
    preferences.putString("wifi_ssid", doc["wifi"]["ssid"].as<String>());
  }
  if (doc["wifi"]["password"].isNull() == false) {
    preferences.putString("wifi_password", doc["wifi"]["password"].as<String>());
  }

  // Save MQTT settings
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

  // Save Device settings
  if (doc["device"]["name"].isNull() == false) {
    preferences.putString("device_name", doc["device"]["name"].as<String>());
  }
  preferences.putBool("device_jiggler", doc["device"]["jiggler"]);
  preferences.putBool("device_setup_mode", doc["device"]["setup_mode"]);
  preferences.putBool("device_keyboard_enable", doc["device"]["keyboard_enable"]);

  // Save User secrets
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

  // Save Device secrets
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

  preferences.end();  // Close Preferences

  server.send(200, "text/plain", "Settings and secrets saved and processed");
}

void setup() {
  Serial.begin(115200);

  preferences.begin("settings", true);  // Open Preferences in read-only mode

  // Load WiFi settings
  String savedSSID = preferences.getString("wifi_ssid", "defaultSSID");
  String savedPassword = preferences.getString("wifi_password", "defaultPassword");

  // Load MQTT settings
  String savedBroker = preferences.getString("mqtt_broker", "defaultBroker");
  int savedPort = preferences.getInt("mqtt_port", 1883);
  String savedTopic = preferences.getString("mqtt_topic", "defaultTopic");
  String savedMQTTUser = preferences.getString("mqtt_user", "defaultUser");
  String savedMQTTPassword = preferences.getString("mqtt_password", "defaultMQTTPass");
  String savedCertificate = preferences.getString("mqtt_certificate", "defaultCertificate");

  // Load Device settings
  String savedDeviceName = preferences.getString("device_name", "defaultDevice");
  bool savedJiggler = preferences.getBool("device_jiggler", false);
  bool savedSetupMode = preferences.getBool("device_setup_mode", false);
  bool savedKeyboardEnable = preferences.getBool("device_keyboard_enable", false);

  // Load User secrets
  String savedUserSecret1 = preferences.getString("user_secret_1", "");
  String savedUserSecret2 = preferences.getString("user_secret_2", "");
  String savedUserSecret3 = preferences.getString("user_secret_3", "");
  String savedUserSecret4 = preferences.getString("user_secret_4", "");
  String savedUserSecret5 = preferences.getString("user_secret_5", "");
  String savedUserSecret6 = preferences.getString("user_secret_6", "");
  String savedUserSecret7 = preferences.getString("user_secret_7", "");
  String savedUserSecret8 = preferences.getString("user_secret_8", "");
  String savedUserSecret9 = preferences.getString("user_secret_9", "");
  String savedUserSecret10 = preferences.getString("user_secret_10", "");

  // Load Device secrets
  String savedDeviceSecret1 = preferences.getString("device_secret_1", "");
  String savedDeviceSecret2 = preferences.getString("device_secret_2", "");
  String savedDeviceSecret3 = preferences.getString("device_secret_3", "");
  String savedDeviceSecret4 = preferences.getString("device_secret_4", "");
  String savedDeviceSecret5 = preferences.getString("device_secret_5", "");
  String savedDeviceSecret6 = preferences.getString("device_secret_6", "");
  String savedDeviceSecret7 = preferences.getString("device_secret_7", "");
  String savedDeviceSecret8 = preferences.getString("device_secret_8", "");
  String savedDeviceSecret9 = preferences.getString("device_secret_9", "");
  String savedDeviceSecret10 = preferences.getString("device_secret_10", "");

  preferences.end();  // Close Preferences

  // Output loaded values to Serial
  Serial.println("Loaded settings:");
  Serial.println("SSID: " + savedSSID);
  Serial.println("WiFi Password: " + savedPassword);
  Serial.println("MQTT Broker: " + savedBroker);
  Serial.println("MQTT Port: " + String(savedPort));
  Serial.println("MQTT Topic: " + savedTopic);
  Serial.println("MQTT User: " + savedMQTTUser);
  Serial.println("MQTT Password: " + savedMQTTPassword);
  Serial.println("MQTT Certificate: " + savedCertificate);
  Serial.println("Device Name: " + savedDeviceName);
  Serial.println("Jiggler: " + String(savedJiggler));
  Serial.println("Setup Mode: " + String(savedSetupMode));
  Serial.println("Keyboard Enable: " + String(savedKeyboardEnable));

  // Print User Secrets
  Serial.println("User Secrets:");
  Serial.println("Secret 1: " + savedUserSecret1);
  Serial.println("Secret 2: " + savedUserSecret2);
  Serial.println("Secret 3: " + savedUserSecret3);
  Serial.println("Secret 4: " + savedUserSecret4);
  Serial.println("Secret 5: " + savedUserSecret5);
  Serial.println("Secret 6: " + savedUserSecret6);
  Serial.println("Secret 7: " + savedUserSecret7);
  Serial.println("Secret 8: " + savedUserSecret8);
  Serial.println("Secret 9: " + savedUserSecret9);
  Serial.println("Secret 10: " + savedUserSecret10);

  // Print Device Secrets
  Serial.println("Device Secrets:");
  Serial.println("Secret 1: " + savedDeviceSecret1);
  Serial.println("Secret 2: " + savedDeviceSecret2);
  Serial.println("Secret 3: " + savedDeviceSecret3);
  Serial.println("Secret 4: " + savedDeviceSecret4);
  Serial.println("Secret 5: " + savedDeviceSecret5);
  Serial.println("Secret 6: " + savedDeviceSecret6);
  Serial.println("Secret 7: " + savedDeviceSecret7);
  Serial.println("Secret 8: " + savedDeviceSecret8);
  Serial.println("Secret 9: " + savedDeviceSecret9);
  Serial.println("Secret 10: " + savedDeviceSecret10);

  WiFi.softAP(ssid, password);

  IPAddress IP = WiFi.softAPIP();
  Serial.print("AP IP address: ");
  Serial.println(IP);

  server.on("/", HTTP_GET, handleRoot);
  server.on("/reset", HTTP_POST, handleReset);

  server.begin();
  Serial.println("HTTP server started");
}

void loop() {
  server.handleClient();
}
