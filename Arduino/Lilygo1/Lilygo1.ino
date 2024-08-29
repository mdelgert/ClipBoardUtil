#include "PinConfig.h"
#include <TFT_eSPI.h>    // Graphics library
#include <OneButton.h>   // Button handling library
#include <FastLED.h>     // LED control library

TFT_eSPI tft;
CRGB leds;

OneButton button(BTN_PIN, true);

void handleClick();
void handleDoubleClick();
void handleLongPressStart();
void ledTask(void *param);

void setup()
{
    // Initialize LED
    FastLED.addLeds<APA102, LED_DI_PIN, LED_CI_PIN, BGR>(&leds, 1);
    int durationInSeconds = 10;
    xTaskCreatePinnedToCore(ledTask, "ledTask", 1024, &durationInSeconds, 1, NULL, 0);

    // Initialize TFT display
    tft.init();
    tft.setRotation(3);
    tft.fillScreen(SCREEN_COLOR);
    tft.setTextColor(TEXT_COLOR, TEXT_BACKGROUND);
    tft.setTextSize(TEXT_SIZE);
    tft.setCursor(TEXT_CURSOR_X, TEXT_CURSOR_Y);
    tft.println("Start");

    // Attach button event handlers
    button.attachClick(handleClick);
    button.attachDoubleClick(handleDoubleClick);
    button.attachLongPressStart(handleLongPressStart);
}

void loop()
{
    button.tick();  // Continuously check the button status
}

void handleClick()
{
    tft.fillScreen(SCREEN_COLOR);
    tft.setCursor(TEXT_CURSOR_X, TEXT_CURSOR_Y);
    tft.println("Clicked!");
}

void handleDoubleClick()
{
    tft.fillScreen(SCREEN_COLOR);
    tft.setCursor(TEXT_CURSOR_X, TEXT_CURSOR_Y);
    tft.println("Double!");
}

void handleLongPressStart()
{
    tft.fillScreen(SCREEN_COLOR);
    tft.setCursor(TEXT_CURSOR_X, TEXT_CURSOR_Y);
    tft.println("Long!");
}

void ledTask(void *param)
{
    int duration = *(int *)param;
    unsigned long startTime = millis();

    while (millis() - startTime < duration * 1000)
    {
        static uint8_t hue = 0;
        leds = CHSV(hue++, 255, 100);
        FastLED.show();
        delay(50);
    }

    leds = CRGB::Black;
    FastLED.show();
    vTaskDelete(NULL);
}
