#include "M5Atom.h"
#include "BluetoothSerial.h"

BluetoothSerial SerialBT;

void setup() {
    M5.begin(true, false,true);  
    delay(50);
    M5.dis.drawpix(0, 0x00ff00);
    Serial.begin(115200);
    SerialBT.begin("ESP32_BT");
    Serial.println("The device started, now you can pair it with Bluetooth!");
}

void loop() {
    if (SerialBT.available()) {
      String message = SerialBT.readString();
      Serial.print("Received: ");
      Serial.println(message);
      M5.dis.drawpix(0, 0x0000f0);  // BLUE
      delay(500);
    }

    delay(50);
    M5.dis.drawpix(0, 0x00ff00);  // GREEN
    M5.update();  // Read the press state of the key.
}