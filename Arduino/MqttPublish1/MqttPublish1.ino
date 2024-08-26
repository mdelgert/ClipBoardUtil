#include "M5Atom.h"

uint8_t DisBuff[2 + 5 * 5 * 3]; // Used to store RGB color values.

void setBuff(uint8_t Rdata, uint8_t Gdata, uint8_t Bdata)
{ // Set the colors of the LED, and save the relevant data to DisBuff[].
  DisBuff[0] = 0x05;
  DisBuff[1] = 0x05;
  for (int i = 0; i < 25; i++)
  {
    DisBuff[2 + i * 3 + 0] = Rdata; // Set red color value.
    DisBuff[2 + i * 3 + 1] = Gdata; // Set green color value.
    DisBuff[2 + i * 3 + 2] = Bdata; // Set blue color value.
  }
}

void setup()
{
  M5.begin(true, false, true); // Init Atom-Matrix (Initialize serial port, LED).
  delay(10);                   // Delay 10 ms.
  setBuff(0xff, 0x00, 0x00);   // Set LED to red color (R = 0xff, G = 0x00, B = 0x00).
  M5.dis.displaybuff(DisBuff); // Display the DisBuff color on the LED.
}

void loop()
{

  if (M5.Btn.wasPressed())
  {
    Serial.println("Button pressed.");
    setBuff(0x40, 0x00, 0x00);
    M5.dis.displaybuff(DisBuff);
    delay(1000); 
  }

  // setBuff(0x40, 0x00, 0x00);   // Set LED to dim red (R = 0x40, G = 0x00, B = 0x00).
  // setBuff(0x00, 0x40, 0x00);   // Set LED to dim green (R = 0x00, G = 0x40, B = 0x00).
  // setBuff(0x00, 0x00, 0x40);   // Set LED to dim blue (R = 0x00, G = 0x00, B = 0x40).
  setBuff(0x20, 0x20, 0x20);   // Set LED to dim white (R = 0x20, G = 0x20, B = 0x20).
  M5.dis.displaybuff(DisBuff); // Display the selected color on the LED.
  delay(50);                   // Delay 50 ms.
  M5.update();                 // Read the press state of the key.
}
