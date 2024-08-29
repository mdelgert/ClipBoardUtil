#pragma once

#define FASTLED_INTERNAL // Suppress FastLED pragma messages -https://github.com/FastLED/FastLED/issues/1169

// Button pin configuration
#define BTN_PIN           0

// LED pin configuration
#define LED_DI_PIN        40
#define LED_CI_PIN        39

// TFT display pin configuration
#define TFT_CS_PIN        4
#define TFT_SDA_PIN       3
#define TFT_SCL_PIN       5
#define TFT_DC_PIN        2
#define TFT_RES_PIN       1
#define TFT_LEDA_PIN      38

// SD MMC pin configuration
#define SD_MMC_D0_PIN     14
#define SD_MMC_D1_PIN     17
#define SD_MMC_D2_PIN     21
#define SD_MMC_D3_PIN     18
#define SD_MMC_CLK_PIN    12
#define SD_MMC_CMD_PIN    16

// Screen and text properties
#define SCREEN_COLOR      TFT_BLACK
#define TEXT_COLOR        TFT_WHITE
#define TEXT_BACKGROUND   TFT_BLACK
#define TEXT_SIZE         2
#define TEXT_CURSOR_X     20
#define TEXT_CURSOR_Y     30