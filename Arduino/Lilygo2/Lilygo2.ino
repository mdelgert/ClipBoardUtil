#include <WiFi.h>
#include <WebServer.h>

const char *ssid = "ESP32_Access_Point";
const char *password = "12345678";

WebServer server(80);

// Define a struct to hold the parsed data (not used in this simple version but kept for future use)
struct Config {
  struct {
    String ssid;
    String password;
  } wifi;

  struct {
    String broker;
    int port;
    String topic;
    String user;
    String password;
    String certificate;
  } mqtt;

  struct {
    String name;
    bool jiggler;
    bool setup_mode;
    bool keyboard_enable;
  } device;

  struct {
    String secret_1;
    String secret_2;
    String secret_3;
    String secret_4;
    String secret_5;
    String secret_6;
    String secret_7;
    String secret_8;
    String secret_9;
    String secret_10;
  } user_secrets, device_secrets;
};

// Function to handle the /reset endpoint
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

  // Set up the access point
  WiFi.softAP(ssid, password);

  IPAddress IP = WiFi.softAPIP();
  Serial.print("AP IP address: ");
  Serial.println(IP);

  // Define the endpoint
  server.on("/reset", HTTP_POST, handleReset);

  // Start the server
  server.begin();
  Serial.println("HTTP server started");
}

void loop() {
  server.handleClient();
}
