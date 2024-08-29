#include <WiFi.h>
#include <WebServer.h>
#include "webpage.h"  // Include the webpage header file
#include "Config.h"   // Include the header file with the struct definition

const char *ssid = "ESP32_Access_Point";
const char *password = "12345678";

WebServer server(80);

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

  // The struct can be populated here if needed in the future
  // Config config;

  server.send(200, "text/plain", "Message received and processed");
}

void setup() {
  Serial.begin(115200);

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
